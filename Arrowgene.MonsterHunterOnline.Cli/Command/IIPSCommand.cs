using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.ClientTools.Dat;
using Arrowgene.MonsterHunterOnline.ClientTools.IIPS;

namespace Arrowgene.MonsterHunterOnline.Cli.Command
{
    public class IIPSCommand : ICommand
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(IIPSCommand));


        public string Key => "iips";
        public string Description => "iips";


        public IIPSCommand()
        {
        }

        public CommandResultType Run(CommandParameter parameter)
        {
            if (parameter.Arguments.Count >= 3 && parameter.Arguments[0] == "dat")
            {
                string inDir = parameter.Arguments[1];
                string outDir = parameter.Arguments[2];

                if (!Directory.Exists(inDir))
                {
                    Logger.Error($"Input directory does not exist: {inDir}");
                    return CommandResultType.Completed;
                }

                if (!Directory.Exists(outDir))
                {
                    Directory.CreateDirectory(outDir);
                }

                DatFile df = new DatFile();
                List<string> files = new List<string>(Directory.GetFiles(inDir));
                files.Sort();
                foreach (string staticFile in files)
                {
                    if (staticFile.EndsWith(".dat"))
                    {
                        FileInfo fi = new FileInfo(staticFile);
                        df.Open(staticFile);
                        if (df.ContentType == DatFile.DatContentType.TSV)
                        {
                            foreach (TsvSheet sheet in df.Sheets)
                            {
                                string outPath = Path.Combine(outDir, $"{fi.Name}_{sheet.Name}.csv");
                                File.WriteAllText(outPath, sheet.ToCsv());
                            }
                        }
                        else
                        {
                            string outPath = Path.Combine(outDir, $"{fi.Name}.txt");
                            File.WriteAllText(outPath, df.Content);
                        }
                    }
                }

                return CommandResultType.Completed;
            }

            if (parameter.Arguments.Count >= 2 && parameter.Arguments[0] == "ifs")
            {
                string inDir = parameter.Arguments[1];

                if (!Directory.Exists(inDir))
                {
                    Logger.Error($"Input directory does not exist: {inDir}");
                    return CommandResultType.Completed;
                }


                List<string> files = new List<string>(Directory.GetFiles(inDir));
                files = files.OrderBy(f =>
                {
                    string fileName = Path.GetFileName(f);
                    if (fileName.StartsWith("base_")) return 0;
                    if (fileName.StartsWith("patch_")) return 1;
                    return 2;
                }).ThenBy(f => f).ToList();
                string outDir = parameter.Arguments.Count >= 3 ? parameter.Arguments[2] : null;

                foreach (string staticFile in files)
                {
                    if (staticFile.EndsWith(".ifs"))
                    {
                        using IIPSArchive archive = IIPSArchive.Open(staticFile);

                        Logger.Info($"Archive: {Path.GetFileName(staticFile)}");
                        Logger.Info($"  Entries: {archive.Entries.Count}, ArchivePaths: {archive.ArchivePaths.Count}");

                        int named = 0;
                        foreach (var entry in archive.Entries)
                        {
                            if (!string.IsNullOrEmpty(entry.ArchivePath)) named++;
                        }
                        Logger.Info($"  Named entries: {named}");

                        if (outDir != null)
                        {
                            archive.ExtractAll(outDir);
                        }
                    }
                }

                return CommandResultType.Completed;
            }

            if (parameter.Arguments.Count == 3 && parameter.Arguments[0] == "dump-sections")
            {
                string inPath = parameter.Arguments[1];
                string outPrefix = parameter.Arguments[2];
                byte[] raw = File.ReadAllBytes(inPath);
                ulong hetOff = BitConverter.ToUInt64(raw, 0x1c);
                ulong hetLen = BitConverter.ToUInt64(raw, 0x34);
                ulong betOff = BitConverter.ToUInt64(raw, 0x14);
                ulong betLen = BitConverter.ToUInt64(raw, 0x3c);
                byte[] het = new byte[hetLen - 12];
                Array.Copy(raw, (long)hetOff + 12, het, 0, het.Length);
                IIPSArchiveCrypto.IfsSectionDecrypt(het);
                File.WriteAllBytes(outPrefix + ".het.dec", het);
                byte[] bet = new byte[betLen - 12];
                Array.Copy(raw, (long)betOff + 12, bet, 0, bet.Length);
                IIPSArchiveCrypto.IfsSectionDecrypt(bet);
                File.WriteAllBytes(outPrefix + ".bet.dec", bet);
                Logger.Info($"Wrote {outPrefix}.het.dec ({het.Length}B) and {outPrefix}.bet.dec ({bet.Length}B)");
                return CommandResultType.Completed;
            }

            if (parameter.Arguments.Count == 2 && parameter.Arguments[0] == "dump")
            {
                string inPath = parameter.Arguments[1];
                using IIPSArchive archive = IIPSArchive.Open(inPath);
                Logger.Info($"Entries in {inPath}:");
                foreach (var e in archive.Entries)
                {
                    Logger.Info($"  [{e.Index}] pos=0x{e.FileOffset:X6} path={e.ArchivePath ?? "(null)"} size={e.Length} stored={e.StoredLength} flags=0x{(uint)e.Flags:X8} md5={e.Md5}");
                }
                return CommandResultType.Completed;
            }

            if (parameter.Arguments.Count == 3 && parameter.Arguments[0] == "resave")
            {
                string inPath = parameter.Arguments[1];
                string outPath = parameter.Arguments[2];
                using IIPSArchive archive = IIPSArchive.Open(inPath);
                archive.Save(outPath, new IIPSArchiveSaveOptions { IncludeListFile = false, PreserveUnchangedEntries = true });
                Logger.Info($"Resaved {inPath} -> {outPath}");
                return CommandResultType.Completed;
            }

            if (parameter.Arguments.Count >= 3 && parameter.Arguments[0] == "patch-del")
            {
                string outPath = parameter.Arguments[1];
                IEnumerable<string> targets = parameter.Arguments.Skip(2);

                using IIPSArchive archive = IIPSArchive.CreateNew();
                foreach (string target in targets)
                {
                    IIPSArchiveEntry entry = archive.MarkDeleted(target);
                    Logger.Info($"Marked deleted: {entry.ArchivePath} (flags=0x{(uint)entry.Flags:X8})");
                }

                archive.Save(outPath);
                Logger.Info($"Wrote patch archive: {outPath}");
                return CommandResultType.Completed;
            }

            if (parameter.Arguments.Count >= 4 && (parameter.Arguments.Count - 2) % 2 == 0 && parameter.Arguments[0] == "patch-replace")
            {
                string outPath = parameter.Arguments[1];
                using IIPSArchive archive = IIPSArchive.CreateNew();
                for (int i = 2; i + 1 < parameter.Arguments.Count; i += 2)
                {
                    string archivePath = parameter.Arguments[i];
                    string sourceFile = parameter.Arguments[i + 1];
                    if (!File.Exists(sourceFile))
                    {
                        Logger.Error($"Source file not found: {sourceFile}");
                        return CommandResultType.Completed;
                    }

                    byte[] content = File.ReadAllBytes(sourceFile);
                    IIPSArchiveEntry entry = archive.Add(archivePath, content, new IIPSArchiveEntryOptions
                    {
                        StorageMode = IIPSArchiveStorageMode.SingleUnit,
                        Compress = false,
                        Encrypt = false,
                    });
                    Logger.Info($"Added: {entry.ArchivePath} ({content.Length} bytes from {sourceFile})");
                }

                archive.Save(outPath);
                Logger.Info($"Wrote patch archive: {outPath}");
                return CommandResultType.Completed;
            }

            Logger.Info("Usage: iips dat <inDir> <outDir>");
            Logger.Info("Usage: iips ifs <inDir> [outDir]");
            Logger.Info("Usage: iips patch-del <outPath.ifs> <archivePath> [archivePath...]");
            Logger.Info("Usage: iips patch-replace <outPath.ifs> <archivePath> <sourceFile> [<archivePath> <sourceFile>...]");

            return CommandResultType.Completed;
        }

        public void Shutdown()
        {
        }
    }
}
