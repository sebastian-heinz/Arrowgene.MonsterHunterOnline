using System.Collections.Generic;
using System.Linq;
using Arrowgene.Buffers;
using Arrowgene.Logging;

namespace Arrowgene.MonsterHunterOnline.Cli.Command;

public class TestCommand : ICommand
{
    private static readonly ILogger Logger = LogProvider.Logger(typeof(ShowCommand));

    public string Key => "test";
    public string Description => $"test";


    private class Code
    {
        public uint start;
        public uint end;
        public byte[] data;
        public string type;
    }

    public CommandResultType Run(CommandParameter parameter)
    {
        string path =
            "C:\\Users\\nxspirit\\dev\\MHO\\TencentGame\\Monster Hunter Online\\Bin\\Client\\Bin32\\CryGame.dll";
        uint baseAddr = 0x10001000 - 0x400;
        uint jmpTableSize = 0x1A2;
        int jmpTableFileOffset = 0x1D0110;
        uint codeEndFileOffset = 0x101D0CF3 - baseAddr;
        
        // reading offset table & code
        IBuffer buf = new StreamBuffer(path);
        buf.Position = jmpTableFileOffset;
        List<uint> fileOffsets = new List<uint>();
        for (int i = 0; i < jmpTableSize; i++)
        {
            uint jmp_addr = buf.ReadUInt32();
            uint jmp_fo = jmp_addr - baseAddr;
            fileOffsets.Add(jmp_fo);
        }

        // get unique code sections
        List<Code> codes = new List<Code>();
        List<uint> fileOffsetsSorted = new List<uint>();
        foreach (uint fileOffset in fileOffsets)
        {
            if (!fileOffsetsSorted.Contains(fileOffset))
            {
                fileOffsetsSorted.Add(fileOffset);
            }
        }

        fileOffsetsSorted.Sort((a, b) => a.CompareTo(b));

        for (int i = 0; i < fileOffsetsSorted.Count; i++)
        {
            Code code = new Code();
            if (i >= fileOffsetsSorted.Count - 1)
            {
                code.start = fileOffsetsSorted[i];
                code.end = codeEndFileOffset;
            }
            else
            {
                code.start = fileOffsetsSorted[i];
                code.end = fileOffsetsSorted[i + 1];
            }

            code.data = buf.GetBytes((int)code.start, (int)(code.end - code.start));
            codes.Add(code);
        }

        // find common pattern
        byte[] read_int_read7x_int_pattern =
            { 0x8B, 0x45, 0x0C, 0x43, 0x83, 0xC0, 0x04, 0x89, 0x45, 0x0C, 0x83, 0xFB, 0x07 };
        List<Code> read_int_read7x_int = new List<Code>();

        byte[] skip_bytes_pattern = { 0x83, 0xE2, 0x0F, 0x8B, 0xCF };
        List<Code> skip_bytes = new List<Code>();

        byte[] read_int_32_bytes_pattern = { 0x8B, 0x8B, 0x64, 0x0D, 0x00, 0x00 };
        List<Code> read_int_32 = new List<Code>();
        
        
        
        for (int i =0; i < codes.Count; i++)
        {
            Code code = codes[i];
            code.type = "unk";
            int pos = PatternAt(code.data, read_int_read7x_int_pattern);
            if (pos > -1)
            {
                read_int_read7x_int.Add(code);
                code.type = "readx7";
            }

            pos = PatternAt(code.data, skip_bytes_pattern);
            if (pos > -1)
            {
                skip_bytes.Add(code);
                code.type = "skip";
            }

            pos = PatternAt(code.data, read_int_32_bytes_pattern);
            if (pos > -1)
            {
                read_int_32.Add(code);
                code.type = "readint32";
            }
        }
        
        
        // build result
        int id = 0;
        foreach (uint fileOffset in fileOffsets)
        {
            Code c = null;
            foreach (Code code in codes)
            {
                if (fileOffset == code.start)
                {
                    c = code;
                    break;
                }
            }

            if (c == null)
            {
                Logger.Error($"{fileOffset} not found");
            }
            Logger.Info($"{id} - {c.type} {c.start}");

            id++;
        }
        
        
        
        return CommandResultType.Completed;
    }

    public static int PatternAt(byte[] source, byte[] pattern)
    {
        for (int i = 0; i < source.Length; i++)
        {
            if (source.Skip(i).Take(pattern.Length).SequenceEqual(pattern))
            {
                return i;
            }
        }

        return -1;
    }


    public void Shutdown()
    {
    }
}