using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace Arrowgene.MonsterHunterOnline.ClientTools.Flash;

public sealed class SwfFile
{
    private const ushort TagHeaderShortLength = 2;
    private const ushort TagHeaderLongLength = 6;

    private readonly Dictionary<ushort, string> _exportNames;
    private readonly Dictionary<ushort, string> _symbolClassNames;
    private readonly byte[] _uncompressedBytes;
    private readonly List<SwfTag> _tags;
    private readonly int _tagStartOffset;

    private SwfFile(string sourcePath, byte version, uint declaredFileLength, SwfCompression compression,
        byte[] uncompressedBytes, int tagStartOffset, SwfRect frameSize, float frameRate, ushort frameCount,
        List<SwfTag> tags, Dictionary<ushort, string> exportNames, Dictionary<ushort, string> symbolClassNames)
    {
        SourcePath = sourcePath;
        Version = version;
        DeclaredFileLength = declaredFileLength;
        Compression = compression;
        _uncompressedBytes = uncompressedBytes;
        _tagStartOffset = tagStartOffset;
        FrameSize = frameSize;
        FrameRate = frameRate;
        FrameCount = frameCount;
        _tags = tags;
        _exportNames = exportNames;
        _symbolClassNames = symbolClassNames;
    }

    public string SourcePath { get; }
    public byte Version { get; }
    public uint DeclaredFileLength { get; }
    public SwfCompression Compression { get; }
    public int ActualFileLength => _uncompressedBytes.Length;
    public SwfRect FrameSize { get; }
    public float FrameRate { get; }
    public ushort FrameCount { get; }
    public IReadOnlyList<SwfTag> Tags => _tags;
    public IReadOnlyDictionary<ushort, string> ExportNames => _exportNames;
    public IReadOnlyDictionary<ushort, string> SymbolClassNames => _symbolClassNames;

    public static bool IsSwf(string path)
    {
        if (!File.Exists(path))
        {
            return false;
        }

        using FileStream stream = File.OpenRead(path);
        Span<byte> signature = stackalloc byte[3];
        int read = stream.Read(signature);
        if (read != 3)
        {
            return false;
        }

        return signature.SequenceEqual("FWS"u8) || signature.SequenceEqual("CWS"u8) || signature.SequenceEqual("ZWS"u8);
    }

    public static SwfFile Open(string path)
    {
        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"SWF file not found: {path}");
        }

        return Open(path, File.ReadAllBytes(path));
    }

    public static SwfFile Open(byte[] data, string name = "(memory)")
    {
        return Open(name, data);
    }

    public byte[] GetUncompressedBytes()
    {
        return (byte[])_uncompressedBytes.Clone();
    }

    public void WriteUncompressed(string outputPath)
    {
        string? directory = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        File.WriteAllBytes(outputPath, _uncompressedBytes);
    }

    public void ReplaceTagData(int tagIndex, byte[] newData)
    {
        if (tagIndex < 0 || tagIndex >= _tags.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(tagIndex));
        }

        SwfTag tag = _tags[tagIndex];
        tag.Data = new ReadOnlyMemory<byte>(newData);
        tag.CharacterId = TryReadCharacterId(tag.Code, newData);

        if (tag.Code == 56)
        {
            MergeTagMap(_exportNames, ParseSymbolMap(newData));
        }
        else if (tag.Code == 76)
        {
            MergeTagMap(_symbolClassNames, ParseSymbolMap(newData));
        }
    }

    public void RemoveTag(int tagIndex)
    {
        if (tagIndex < 0 || tagIndex >= _tags.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(tagIndex));
        }

        _tags.RemoveAt(tagIndex);
        ReindexTags();
    }

    public SwfTag InsertTag(int tagIndex, ushort code, byte[] data)
    {
        if (tagIndex < 0 || tagIndex > _tags.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(tagIndex));
        }

        SwfTag tag = new(tagIndex, code, -1, 0, TryReadCharacterId(code, data), new ReadOnlyMemory<byte>(data));
        _tags.Insert(tagIndex, tag);
        ReindexTags();

        if (code == 56)
        {
            MergeTagMap(_exportNames, ParseSymbolMap(data));
        }
        else if (code == 76)
        {
            MergeTagMap(_symbolClassNames, ParseSymbolMap(data));
        }

        return tag;
    }

    public byte[] Build(SwfCompression? compression = null)
    {
        SwfCompression targetCompression = compression ?? Compression;
        byte[] uncompressed = BuildUncompressedBytes();

        if (targetCompression == SwfCompression.Uncompressed)
        {
            return uncompressed;
        }

        using MemoryStream output = new();
        output.WriteByte((byte)'C');
        output.WriteByte((byte)'W');
        output.WriteByte((byte)'S');
        output.WriteByte(Version);
        Span<byte> lengthBuf = stackalloc byte[4];
        BinaryPrimitives.WriteUInt32LittleEndian(lengthBuf, (uint)uncompressed.Length);
        output.Write(lengthBuf);

        using (ZLibStream zlib = new(output, CompressionLevel.Optimal, leaveOpen: true))
        {
            zlib.Write(uncompressed, 8, uncompressed.Length - 8);
        }

        return output.ToArray();
    }

    public void Save(string outputPath, SwfCompression? compression = null)
    {
        string? directory = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        File.WriteAllBytes(outputPath, Build(compression));
    }

    private byte[] BuildUncompressedBytes()
    {
        using MemoryStream ms = new();
        ms.Write(_uncompressedBytes, 0, _tagStartOffset);

        foreach (SwfTag tag in _tags)
        {
            WriteTagRecord(ms, tag.Code, tag.Data.Span);
        }

        byte[] result = ms.ToArray();
        BinaryPrimitives.WriteUInt32LittleEndian(result.AsSpan(4, 4), (uint)result.Length);
        result[0] = (byte)'F';
        result[1] = (byte)'W';
        result[2] = (byte)'S';
        return result;
    }

    private static void WriteTagRecord(MemoryStream ms, ushort code, ReadOnlySpan<byte> data)
    {
        uint length = (uint)data.Length;
        Span<byte> header = stackalloc byte[6];

        if (length < 0x3F)
        {
            BinaryPrimitives.WriteUInt16LittleEndian(header, (ushort)((code << 6) | length));
            ms.Write(header[..2]);
        }
        else
        {
            BinaryPrimitives.WriteUInt16LittleEndian(header, (ushort)((code << 6) | 0x3F));
            BinaryPrimitives.WriteUInt32LittleEndian(header[2..], length);
            ms.Write(header);
        }

        ms.Write(data);
    }

    private void ReindexTags()
    {
        for (int i = 0; i < _tags.Count; i++)
        {
            _tags[i].Index = i;
        }
    }

    public void ExtractAll(string outputDirectory, bool includeRawTagData = true, bool includeKnownAssets = true,
        bool writeManifest = true)
    {
        Directory.CreateDirectory(outputDirectory);

        string uncompressedName = $"{Path.GetFileNameWithoutExtension(SourcePath)}.uncompressed.swf";
        WriteUncompressed(Path.Combine(outputDirectory, uncompressedName));

        if (includeRawTagData)
        {
            string tagDirectory = Path.Combine(outputDirectory, "tags");
            Directory.CreateDirectory(tagDirectory);
            foreach (SwfTag tag in _tags.Where(tag => tag.Length > 0))
            {
                string tagFileName = $"{BuildTagStem(tag)}.bin";
                File.WriteAllBytes(Path.Combine(tagDirectory, tagFileName), tag.Data.ToArray());
            }
        }

        if (includeKnownAssets)
        {
            string assetDirectory = Path.Combine(outputDirectory, "assets");
            Directory.CreateDirectory(assetDirectory);
            foreach (SwfTag tag in _tags)
            {
                ExtractKnownAsset(assetDirectory, tag);
            }
        }

        if (writeManifest)
        {
            WriteManifest(outputDirectory);
        }
    }

    private static SwfFile Open(string sourcePath, byte[] sourceData)
    {
        if (sourceData.Length < 8)
        {
            throw new InvalidDataException($"SWF file '{sourcePath}' is too small to contain a valid header.");
        }

        ReadOnlySpan<byte> signature = sourceData.AsSpan(0, 3);
        SwfCompression compression = ParseCompression(signature);
        if (compression == SwfCompression.Lzma)
        {
            throw new NotSupportedException($"SWF file '{sourcePath}' uses LZMA compression (ZWS), which is not supported.");
        }

        byte version = sourceData[3];
        uint declaredFileLength = BinaryPrimitives.ReadUInt32LittleEndian(sourceData.AsSpan(4, 4));
        byte[] uncompressedBytes = compression switch
        {
            SwfCompression.Uncompressed => NormalizeUncompressedSource(sourceData),
            SwfCompression.Zlib => InflateZlibSwf(sourceData, version, declaredFileLength),
            _ => throw new InvalidDataException($"Unsupported SWF compression for '{sourcePath}'."),
        };

        if (uncompressedBytes.Length < 12)
        {
            throw new InvalidDataException($"SWF file '{sourcePath}' is truncated after decompression.");
        }

        ReadOnlySpan<byte> body = uncompressedBytes.AsSpan(8);
        SwfBitReader bitReader = new(body);
        SwfRect frameSize = ReadRect(ref bitReader);
        int headerBodyBytes = bitReader.BytesConsumed;
        if (body.Length < headerBodyBytes + 4)
        {
            throw new InvalidDataException($"SWF file '{sourcePath}' is missing frame metadata.");
        }

        ushort frameRateFixed = BinaryPrimitives.ReadUInt16LittleEndian(body.Slice(headerBodyBytes, 2));
        float frameRate = frameRateFixed / 256f;
        ushort frameCount = BinaryPrimitives.ReadUInt16LittleEndian(body.Slice(headerBodyBytes + 2, 2));

        int tagOffset = 8 + headerBodyBytes + 4;
        List<SwfTag> tags = [];
        Dictionary<ushort, string> exportNames = new();
        Dictionary<ushort, string> symbolClassNames = new();

        int tagIndex = 0;
        while (tagOffset <= uncompressedBytes.Length - 2)
        {
            int tagHeaderOffset = tagOffset;
            ushort recordHeader = BinaryPrimitives.ReadUInt16LittleEndian(uncompressedBytes.AsSpan(tagOffset, 2));
            tagOffset += 2;

            ushort code = (ushort)(recordHeader >> 6);
            uint length = (uint)(recordHeader & 0x3F);
            int headerLength = TagHeaderShortLength;
            if (length == 0x3F)
            {
                if (tagOffset > uncompressedBytes.Length - 4)
                {
                    throw new InvalidDataException($"SWF file '{sourcePath}' has a truncated long tag header.");
                }

                length = BinaryPrimitives.ReadUInt32LittleEndian(uncompressedBytes.AsSpan(tagOffset, 4));
                tagOffset += 4;
                headerLength = TagHeaderLongLength;
            }

            if (length > (uint)(uncompressedBytes.Length - tagOffset))
            {
                throw new InvalidDataException(
                    $"SWF file '{sourcePath}' has tag {GetTagName(code)}({code}) with declared length {length} beyond EOF.");
            }

            ReadOnlyMemory<byte> data = new(uncompressedBytes, tagOffset, checked((int)length));
            SwfTag tag = new(tagIndex, code, tagHeaderOffset, headerLength, TryReadCharacterId(code, data.Span), data);
            tags.Add(tag);

            if (code == 56)
            {
                MergeTagMap(exportNames, ParseSymbolMap(data.Span));
            }
            else if (code == 76)
            {
                MergeTagMap(symbolClassNames, ParseSymbolMap(data.Span));
            }

            tagOffset += checked((int)length);
            tagIndex++;

            if (code == 0)
            {
                break;
            }
        }

        foreach (SwfTag tag in tags)
        {
            if (tag.CharacterId is not ushort characterId)
            {
                continue;
            }

            if (exportNames.TryGetValue(characterId, out string? exportName))
            {
                tag.ExportName = exportName;
            }

            if (symbolClassNames.TryGetValue(characterId, out string? symbolClassName))
            {
                tag.SymbolClassName = symbolClassName;
            }
        }

        return new SwfFile(sourcePath, version, declaredFileLength, compression, uncompressedBytes, tagOffset,
            frameSize, frameRate, frameCount, tags, exportNames, symbolClassNames);
    }

    private void ExtractKnownAsset(string assetDirectory, SwfTag tag)
    {
        switch (tag.Code)
        {
            case 8:
                WriteBlob(assetDirectory, tag, "jpeg_tables", ".bin", tag.Data.Span);
                break;
            case 20:
            case 36:
                ExtractLosslessBitmap(assetDirectory, tag);
                break;
            case 21:
                ExtractJpegLikeBitmap(assetDirectory, tag, includeAlpha: false, includeDeblock: false);
                break;
            case 35:
                ExtractJpegLikeBitmap(assetDirectory, tag, includeAlpha: true, includeDeblock: false);
                break;
            case 82:
                ExtractDoAbc(assetDirectory, tag);
                break;
            case 87:
                ExtractBinaryData(assetDirectory, tag);
                break;
            case 90:
                ExtractJpegLikeBitmap(assetDirectory, tag, includeAlpha: true, includeDeblock: true);
                break;
        }
    }

    private void ExtractLosslessBitmap(string assetDirectory, SwfTag tag)
    {
        ReadOnlySpan<byte> data = tag.Data.Span;
        if (data.Length < 7)
        {
            return;
        }

        ushort characterId = BinaryPrimitives.ReadUInt16LittleEndian(data.Slice(0, 2));
        byte bitmapFormat = data[2];
        ushort width = BinaryPrimitives.ReadUInt16LittleEndian(data.Slice(3, 2));
        ushort height = BinaryPrimitives.ReadUInt16LittleEndian(data.Slice(5, 2));

        int offset = 7;
        int? colorTableSize = null;
        if (bitmapFormat == 3)
        {
            if (data.Length < 8)
            {
                return;
            }

            colorTableSize = data[offset] + 1;
            offset += 1;
        }

        byte[] bitmapData = InflateZlib(data.Slice(offset));
        string stem = BuildTagStem(tag);
        string blobPath = Path.Combine(assetDirectory, $"{stem}.lossless.bin");
        string metaPath = Path.Combine(assetDirectory, $"{stem}.lossless.json");

        File.WriteAllBytes(blobPath, bitmapData);
        string json = JsonSerializer.Serialize(new
        {
            CharacterId = characterId,
            TagCode = tag.Code,
            TagName = tag.Name,
            BitmapFormat = bitmapFormat,
            Width = width,
            Height = height,
            HasAlpha = tag.Code == 36,
            ColorTableSize = colorTableSize,
            DecodedByteLength = bitmapData.Length,
        }, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(metaPath, json);
    }

    private void ExtractJpegLikeBitmap(string assetDirectory, SwfTag tag, bool includeAlpha, bool includeDeblock)
    {
        ReadOnlySpan<byte> data = tag.Data.Span;
        int offset = 0;
        if (data.Length < 2)
        {
            return;
        }

        ushort characterId = BinaryPrimitives.ReadUInt16LittleEndian(data.Slice(offset, 2));
        offset += 2;

        uint alphaOffset = 0;
        ushort? deblockParameter = null;
        if (includeAlpha)
        {
            if (data.Length < offset + 4)
            {
                return;
            }

            alphaOffset = BinaryPrimitives.ReadUInt32LittleEndian(data.Slice(offset, 4));
            offset += 4;
        }

        if (includeDeblock)
        {
            if (data.Length < offset + 2)
            {
                return;
            }

            deblockParameter = BinaryPrimitives.ReadUInt16LittleEndian(data.Slice(offset, 2));
            offset += 2;
        }

        ReadOnlySpan<byte> bitmapData;
        ReadOnlySpan<byte> alphaData = ReadOnlySpan<byte>.Empty;
        if (includeAlpha)
        {
            if (alphaOffset > (uint)(data.Length - offset))
            {
                return;
            }

            bitmapData = data.Slice(offset, checked((int)alphaOffset));
            alphaData = data.Slice(offset + checked((int)alphaOffset));
        }
        else
        {
            bitmapData = data.Slice(offset);
        }

        string extension = DetectBinaryExtension(bitmapData);
        string stem = BuildTagStem(tag);
        File.WriteAllBytes(Path.Combine(assetDirectory, $"{stem}{extension}"), bitmapData.ToArray());

        if (!alphaData.IsEmpty)
        {
            byte[] decodedAlpha = InflateZlib(alphaData);
            File.WriteAllBytes(Path.Combine(assetDirectory, $"{stem}.alpha.bin"), decodedAlpha);

            string json = JsonSerializer.Serialize(new
            {
                CharacterId = characterId,
                TagCode = tag.Code,
                TagName = tag.Name,
                AlphaByteLength = decodedAlpha.Length,
                DeblockParameter = deblockParameter,
            }, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(Path.Combine(assetDirectory, $"{stem}.json"), json);
        }
    }

    private void ExtractBinaryData(string assetDirectory, SwfTag tag)
    {
        ReadOnlySpan<byte> data = tag.Data.Span;
        if (data.Length < 6)
        {
            return;
        }

        ReadOnlySpan<byte> blob = data.Slice(6);
        string extension = DetectBinaryExtension(blob);
        WriteBlob(assetDirectory, tag, "binary", extension, blob);
    }

    private void ExtractDoAbc(string assetDirectory, SwfTag tag)
    {
        ReadOnlySpan<byte> data = tag.Data.Span;
        if (data.Length < 4)
        {
            return;
        }

        int offset = 4; // flags
        string abcName = ReadSwfString(data, ref offset);
        ReadOnlySpan<byte> abcData = data.Slice(offset);
        string stem = BuildTagStem(tag);
        if (!string.IsNullOrWhiteSpace(abcName))
        {
            stem = $"{stem}_{SanitizePathComponent(abcName)}";
        }

        File.WriteAllBytes(Path.Combine(assetDirectory, $"{stem}.abc"), abcData.ToArray());
    }

    private void WriteManifest(string outputDirectory)
    {
        var manifest = new
        {
            Type = "SWF",
            SourceFile = Path.GetFileName(SourcePath),
            Compression = Compression.ToString(),
            Version,
            DeclaredFileLength,
            ActualFileLength,
            FrameSize = new
            {
                FrameSize.XMin,
                FrameSize.XMax,
                FrameSize.YMin,
                FrameSize.YMax,
                FrameSize.WidthTwips,
                FrameSize.HeightTwips,
                FrameSize.WidthPixels,
                FrameSize.HeightPixels,
            },
            FrameRate,
            FrameCount,
            ExportCount = _exportNames.Count,
            SymbolClassCount = _symbolClassNames.Count,
            Tags = _tags.Select(tag => new
            {
                tag.Index,
                tag.Code,
                tag.Name,
                tag.Length,
                tag.Offset,
                tag.HeaderLength,
                tag.CharacterId,
                tag.ExportName,
                tag.SymbolClassName,
            }),
        };

        string manifestPath = Path.Combine(outputDirectory, "manifest.json");
        string json = JsonSerializer.Serialize(manifest, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(manifestPath, json);
    }

    private static byte[] NormalizeUncompressedSource(byte[] sourceData)
    {
        byte[] normalized = (byte[])sourceData.Clone();
        normalized[0] = (byte)'F';
        normalized[1] = (byte)'W';
        normalized[2] = (byte)'S';
        return normalized;
    }

    private static byte[] InflateZlibSwf(byte[] sourceData, byte version, uint declaredFileLength)
    {
        using MemoryStream input = new(sourceData, 8, sourceData.Length - 8, writable: false);
        using ZLibStream zlib = new(input, CompressionMode.Decompress);
        using MemoryStream output = declaredFileLength > 0 ? new MemoryStream(checked((int)declaredFileLength)) : new MemoryStream();
        output.WriteByte((byte)'F');
        output.WriteByte((byte)'W');
        output.WriteByte((byte)'S');
        output.WriteByte(version);

        Span<byte> length = stackalloc byte[4];
        BinaryPrimitives.WriteUInt32LittleEndian(length, declaredFileLength);
        output.Write(length);
        zlib.CopyTo(output);
        return output.ToArray();
    }

    private static byte[] InflateZlib(ReadOnlySpan<byte> compressedData)
    {
        using MemoryStream input = new(compressedData.ToArray(), writable: false);
        using ZLibStream zlib = new(input, CompressionMode.Decompress);
        using MemoryStream output = new();
        zlib.CopyTo(output);
        return output.ToArray();
    }

    private static SwfCompression ParseCompression(ReadOnlySpan<byte> signature)
    {
        if (signature.SequenceEqual("FWS"u8))
        {
            return SwfCompression.Uncompressed;
        }

        if (signature.SequenceEqual("CWS"u8))
        {
            return SwfCompression.Zlib;
        }

        if (signature.SequenceEqual("ZWS"u8))
        {
            return SwfCompression.Lzma;
        }

        throw new InvalidDataException("Input is not a supported SWF file.");
    }

    private static SwfRect ReadRect(ref SwfBitReader bitReader)
    {
        int bitCount = checked((int)bitReader.ReadUnsignedBits(5));
        int xMin = bitReader.ReadSignedBits(bitCount);
        int xMax = bitReader.ReadSignedBits(bitCount);
        int yMin = bitReader.ReadSignedBits(bitCount);
        int yMax = bitReader.ReadSignedBits(bitCount);
        bitReader.AlignByte();
        return new SwfRect(xMin, xMax, yMin, yMax);
    }

    private static ushort? TryReadCharacterId(ushort code, ReadOnlySpan<byte> data)
    {
        if (data.Length < 2)
        {
            return null;
        }

        return code switch
        {
            2 or 6 or 7 or 10 or 11 or 13 or 14 or 20 or 21 or 22 or 32 or 33 or 34 or 35 or 36 or 37 or 39 or 46
                or 48 or 60 or 62 or 73 or 74 or 75 or 83 or 84 or 87 or 88 or 90 or 91
                => BinaryPrimitives.ReadUInt16LittleEndian(data.Slice(0, 2)),
            _ => null,
        };
    }

    private static Dictionary<ushort, string> ParseSymbolMap(ReadOnlySpan<byte> data)
    {
        Dictionary<ushort, string> map = new();
        if (data.Length < 2)
        {
            return map;
        }

        int offset = 0;
        ushort count = BinaryPrimitives.ReadUInt16LittleEndian(data.Slice(offset, 2));
        offset += 2;

        for (int i = 0; i < count && offset < data.Length; i++)
        {
            if (offset > data.Length - 2)
            {
                break;
            }

            ushort characterId = BinaryPrimitives.ReadUInt16LittleEndian(data.Slice(offset, 2));
            offset += 2;
            string name = ReadSwfString(data, ref offset);
            if (!string.IsNullOrEmpty(name))
            {
                map[characterId] = name;
            }
        }

        return map;
    }

    private static string ReadSwfString(ReadOnlySpan<byte> data, ref int offset)
    {
        int start = offset;
        while (offset < data.Length && data[offset] != 0)
        {
            offset++;
        }

        string value = Encoding.UTF8.GetString(data.Slice(start, offset - start));
        if (offset < data.Length && data[offset] == 0)
        {
            offset++;
        }

        return value;
    }

    private static void MergeTagMap(Dictionary<ushort, string> destination, Dictionary<ushort, string> source)
    {
        foreach ((ushort key, string value) in source)
        {
            destination[key] = value;
        }
    }

    private static string BuildTagStem(SwfTag tag)
    {
        List<string> parts =
        [
            tag.Index.ToString("D4", CultureInfo.InvariantCulture),
            tag.Name,
        ];

        if (tag.CharacterId is ushort characterId)
        {
            parts.Add(characterId.ToString("D5", CultureInfo.InvariantCulture));
        }

        string? assetName = tag.ExportName ?? tag.SymbolClassName;
        if (!string.IsNullOrWhiteSpace(assetName))
        {
            parts.Add(SanitizePathComponent(assetName));
        }

        return string.Join("_", parts);
    }

    private static void WriteBlob(string outputDirectory, SwfTag tag, string suffix, string extension,
        ReadOnlySpan<byte> data)
    {
        string stem = $"{BuildTagStem(tag)}_{suffix}";
        File.WriteAllBytes(Path.Combine(outputDirectory, $"{stem}{extension}"), data.ToArray());
    }

    private static string DetectBinaryExtension(ReadOnlySpan<byte> data)
    {
        if (data.Length >= 8 &&
            data[0] == 0x89 &&
            data[1] == 0x50 &&
            data[2] == 0x4E &&
            data[3] == 0x47 &&
            data[4] == 0x0D &&
            data[5] == 0x0A &&
            data[6] == 0x1A &&
            data[7] == 0x0A)
        {
            return ".png";
        }

        if (data.Length >= 3 && data[0] == 0xFF && data[1] == 0xD8 && data[2] == 0xFF)
        {
            return ".jpg";
        }

        if (data.Length >= 6 &&
            ((data[0] == (byte)'G' && data[1] == (byte)'I' && data[2] == (byte)'F' && data[3] == (byte)'8' && data[4] == (byte)'7' && data[5] == (byte)'a') ||
             (data[0] == (byte)'G' && data[1] == (byte)'I' && data[2] == (byte)'F' && data[3] == (byte)'8' && data[4] == (byte)'9' && data[5] == (byte)'a')))
        {
            return ".gif";
        }

        if (data.Length >= 3 &&
            ((data[0] == (byte)'F' && data[1] == (byte)'W' && data[2] == (byte)'S') ||
             (data[0] == (byte)'C' && data[1] == (byte)'W' && data[2] == (byte)'S') ||
             (data[0] == (byte)'Z' && data[1] == (byte)'W' && data[2] == (byte)'S')))
        {
            return ".swf";
        }

        if (data.Length >= 4 && data[0] == (byte)'P' && data[1] == (byte)'K' && data[2] == 0x03 && data[3] == 0x04)
        {
            return ".zip";
        }

        if (data.Length >= 4 && data[0] == (byte)'R' && data[1] == (byte)'I' && data[2] == (byte)'F' && data[3] == (byte)'F')
        {
            return ".riff";
        }

        if (LooksLikeXml(data))
        {
            return ".xml";
        }

        return ".bin";
    }

    private static bool LooksLikeXml(ReadOnlySpan<byte> data)
    {
        int offset = 0;
        while (offset < data.Length && char.IsWhiteSpace((char)data[offset]))
        {
            offset++;
        }

        return offset + 5 <= data.Length &&
               data[offset] == (byte)'<' &&
               data[offset + 1] == (byte)'?' &&
               data[offset + 2] == (byte)'x' &&
               data[offset + 3] == (byte)'m' &&
               data[offset + 4] == (byte)'l';
    }

    private static string SanitizePathComponent(string value)
    {
        char[] invalid = Path.GetInvalidFileNameChars();
        StringBuilder builder = new(value.Length);
        foreach (char c in value)
        {
            builder.Append(Array.IndexOf(invalid, c) >= 0 || c == '/' || c == '\\' ? '_' : c);
        }

        string sanitized = builder.ToString().Trim();
        return string.IsNullOrEmpty(sanitized) ? "unnamed" : sanitized;
    }

    private static string GetTagName(ushort code)
    {
        return SwfFileTagName.Get(code);
    }
}

public enum SwfCompression
{
    Uncompressed,
    Zlib,
    Lzma,
}

public readonly record struct SwfRect(int XMin, int XMax, int YMin, int YMax)
{
    public int WidthTwips => XMax - XMin;
    public int HeightTwips => YMax - YMin;
    public float WidthPixels => WidthTwips / 20f;
    public float HeightPixels => HeightTwips / 20f;
}

public sealed class SwfTag
{
    internal SwfTag(int index, ushort code, int offset, int headerLength, ushort? characterId,
        ReadOnlyMemory<byte> data)
    {
        Index = index;
        Code = code;
        Offset = offset;
        HeaderLength = headerLength;
        CharacterId = characterId;
        Data = data;
    }

    public int Index { get; internal set; }
    public ushort Code { get; }
    public string Name => SwfFileTagName.Get(Code);
    public uint Length => (uint)Data.Length;
    public int Offset { get; }
    public int HeaderLength { get; }
    public ushort? CharacterId { get; internal set; }
    public string? ExportName { get; internal set; }
    public string? SymbolClassName { get; internal set; }
    public ReadOnlyMemory<byte> Data { get; internal set; }
}

internal static class SwfFileTagName
{
    public static string Get(ushort code)
    {
        return code switch
        {
            0 => "End",
            1 => "ShowFrame",
            2 => "DefineShape",
            4 => "PlaceObject",
            5 => "RemoveObject",
            6 => "DefineBits",
            7 => "DefineButton",
            8 => "JPEGTables",
            9 => "SetBackgroundColor",
            10 => "DefineFont",
            11 => "DefineText",
            12 => "DoAction",
            13 => "DefineFontInfo",
            14 => "DefineSound",
            15 => "StartSound",
            17 => "DefineButtonSound",
            18 => "SoundStreamHead",
            19 => "SoundStreamBlock",
            20 => "DefineBitsLossless",
            21 => "DefineBitsJPEG2",
            22 => "DefineShape2",
            24 => "Protect",
            26 => "PlaceObject2",
            28 => "RemoveObject2",
            32 => "DefineShape3",
            33 => "DefineText2",
            34 => "DefineButton2",
            35 => "DefineBitsJPEG3",
            36 => "DefineBitsLossless2",
            37 => "DefineEditText",
            39 => "DefineSprite",
            41 => "ProductInfo",
            43 => "FrameLabel",
            45 => "SoundStreamHead2",
            46 => "DefineMorphShape",
            48 => "DefineFont2",
            56 => "ExportAssets",
            57 => "ImportAssets",
            58 => "EnableDebugger",
            59 => "DoInitAction",
            60 => "DefineVideoStream",
            61 => "VideoFrame",
            62 => "DefineFontInfo2",
            64 => "EnableDebugger2",
            65 => "ScriptLimits",
            66 => "SetTabIndex",
            69 => "FileAttributes",
            70 => "PlaceObject3",
            71 => "ImportAssets2",
            73 => "DefineFontAlignZones",
            74 => "CSMTextSettings",
            75 => "DefineFont3",
            76 => "SymbolClass",
            77 => "Metadata",
            78 => "DefineScalingGrid",
            82 => "DoABC",
            83 => "DefineShape4",
            84 => "DefineMorphShape2",
            86 => "DefineSceneAndFrameLabelData",
            87 => "DefineBinaryData",
            88 => "DefineFontName",
            89 => "StartSound2",
            90 => "DefineBitsJPEG4",
            91 => "DefineFont4",
            93 => "Telemetry",
            _ => $"Tag{code}",
        };
    }
}

internal ref struct SwfBitReader
{
    private readonly ReadOnlySpan<byte> _data;
    private int _bitOffset;

    public SwfBitReader(ReadOnlySpan<byte> data)
    {
        _data = data;
        _bitOffset = 0;
    }

    public int BytesConsumed => (_bitOffset + 7) / 8;

    public uint ReadUnsignedBits(int bitCount)
    {
        uint value = 0;
        for (int i = 0; i < bitCount; i++)
        {
            int byteIndex = _bitOffset / 8;
            if (byteIndex >= _data.Length)
            {
                throw new EndOfStreamException("Unexpected end of SWF bit stream.");
            }

            int bitIndex = 7 - (_bitOffset % 8);
            value = (value << 1) | (uint)((_data[byteIndex] >> bitIndex) & 1);
            _bitOffset++;
        }

        return value;
    }

    public int ReadSignedBits(int bitCount)
    {
        if (bitCount == 0)
        {
            return 0;
        }

        uint value = ReadUnsignedBits(bitCount);
        int shift = 32 - bitCount;
        return ((int)value << shift) >> shift;
    }

    public void AlignByte()
    {
        int remainder = _bitOffset % 8;
        if (remainder != 0)
        {
            _bitOffset += 8 - remainder;
        }
    }
}
