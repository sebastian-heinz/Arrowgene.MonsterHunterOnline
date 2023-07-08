using System.IO;
using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.IIPS;

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
            //     IIPSFile f = new IIPSFile();
            //  f.Open("C:\\Users\\nxspirit\\Downloads\\MHO_FullDirectory_Final\\TencentGame\\Monster Hunter Online\\Bin\\Client\\IIPS\\iipsdownload\\base_000_2017_06_26_23_05.ifs");

            //     f.Open("C:\\Users\\nxspirit\\dev\\test\\base_000_2017_06_26_23_05.ifs");


            //   f.Open("C:\\Users\\nxspirit\\dev\\test\\patch_2018_03_09_10_11.ifs");


            string outDir = "C:\\Users\\nxspirit\\dev\\MHO_TOOL\\static_csv\\";
            if (!Directory.Exists(outDir))
            {
                Directory.CreateDirectory(outDir);
            }

            DatFile df = new DatFile();
            foreach (string staticFile in Directory.GetFiles(
                         "C:\\Users\\nxspirit\\dev\\MHO_TOOL\\extracted\\common\\staticdata\\"))
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


            // df.Open("C:\\Users\\nxspirit\\dev\\MHO_TOOL\\extracted\\common\\staticdata\\entry_info_hub.dat");

            return CommandResultType.Continue;
        }

        public void Shutdown()
        {
        }
    }
}