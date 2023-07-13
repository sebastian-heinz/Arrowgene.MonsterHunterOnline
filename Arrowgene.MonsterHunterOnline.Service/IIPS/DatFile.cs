using System.Collections.Generic;
using System.Text;
using Arrowgene.Buffers;
using Arrowgene.Logging;

namespace Arrowgene.MonsterHunterOnline.Service.IIPS;

public class DatFile
{
    private static readonly ILogger Logger = LogProvider.Logger(typeof(DatFile));

    public const uint DatHeaderLength = 0x10;
    public const uint DatMagic = 0x43;
    public const string TsvStart = "#============Begin of sheet============";
    public const string TsvEnd = "#============End of sheet============";
    public const string TsvContentHeader = "#TSV";

    public DatFile()
    {
        Sheets = new List<TsvSheet>();
    }

    public uint Magic { get; set; }
    public uint HeaderLength { get; set; }
    public uint ChunkCount { get; set; }

    public DatContentType ContentType { get; set; }
    public List<TsvSheet> Sheets { get; }

    public string Content { get; set; }

    public enum DatContentType
    {
        Content = 0,
        TSV = 1
    }

    public void Open(string path)
    {
        Sheets.Clear();

        StreamBuffer b = new StreamBuffer(path);
        b.SetPositionStart();
        HeaderLength = b.ReadUInt32();
        if (DatHeaderLength != HeaderLength)
        {
            if (HeaderLength == 0x56535423)
            {
                // TSV
                byte[] tsvBytes = b.GetAllBytes();
                ReadTsv(tsvBytes);
                return;
            }
            Logger.Info($"header length miss match");
        }

        Magic = b.ReadUInt32();
        if (DatMagic != Magic)
        {
            Logger.Info($"MAGIC miss match");
        }

        ChunkCount = b.ReadUInt32();
        uint unk = b.ReadUInt32();

        byte[] cipher = b.ReadBytes(b.Size - b.Position);
        byte[] plain = IIPSCrypto.DecryptDat(cipher);
        
        
        ReadTsv(plain);
    }

    private void ReadTsv(byte[] tsvBytes)
    {
        Content = Encoding.UTF8.GetString(tsvBytes);
        StreamBuffer dec = new StreamBuffer(tsvBytes);
        dec.SetPositionStart();
        string tsv = dec.ReadFixedString(4);
        if (tsv != TsvContentHeader)
        {
            ContentType = DatContentType.Content;
            return;
        }

        ContentType = DatContentType.TSV;
        while (true)
        {
            if (dec.Position >= dec.Size)
            {
                break;
            }

            if (dec.Position + TsvStart.Length >= dec.Size)
            {
                Logger.Info("Finished Reading Sheets");
                break;
            }

            SwallowCrAndLF(dec);
            string tsvStart = dec.ReadFixedString(TsvStart.Length);
            if (tsvStart != TsvStart)
            {
                Logger.Error("Expected TSV Start");
            }

            SwallowCrAndLF(dec);

            byte[] endBuffer = Encoding.UTF8.GetBytes(TsvEnd);
            int endBufferIndex = 0;
            List<byte> buffer = new List<byte>();

            while (true)
            {
                if (dec.Position >= dec.Size)
                {
                    byte[] sheetBytes = buffer.ToArray();
                    string sheet = Encoding.UTF8.GetString(sheetBytes, 0, sheetBytes.Length);
                    sheet = sheet.TrimEnd('\n').TrimEnd('\r');
                    TsvSheet tsvSheet = new TsvSheet(sheet);
                    Sheets.Add(tsvSheet);
                    buffer.Clear();
                    break;
                }
                byte read = dec.ReadByte();
                if (read == endBuffer[endBufferIndex])
                {
                    endBufferIndex++;
                }
                else
                {
                    if (endBufferIndex > 0)
                    {
                        for (int i = 0; i < endBufferIndex; i++)
                        {
                            buffer.Add(endBuffer[i]);
                        }
                    }

                    buffer.Add(read);
                    endBufferIndex = 0;
                }

                if (endBufferIndex == endBuffer.Length)
                {
                    byte[] sheetBytes = buffer.ToArray();
                    string sheet = Encoding.UTF8.GetString(sheetBytes, 0, sheetBytes.Length);
                    sheet = sheet.TrimEnd('\n').TrimEnd('\r');
                    TsvSheet tsvSheet = new TsvSheet(sheet);
                    Sheets.Add(tsvSheet);
                    buffer.Clear();
                    break;
                }
            }
        }
    }

    private void SwallowCrAndLF(IBuffer buffer)
    {
        int pos = buffer.Position;
        int read = 0;
        while (true)
        {
            byte b = buffer.ReadByte();
            if (b == '\r')
            {
                read++;
            }
            else if (b == '\n')
            {
                read++;
            }
            else
            {
                break;
            }
        }

        buffer.Position = pos + read;
    }
}