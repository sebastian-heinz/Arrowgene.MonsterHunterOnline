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
            IIPSFile f = new IIPSFile();
          //  f.Open("C:\\Users\\nxspirit\\Downloads\\MHO_FullDirectory_Final\\TencentGame\\Monster Hunter Online\\Bin\\Client\\IIPS\\iipsdownload\\base_000_2017_06_26_23_05.ifs");

            f.Open("C:\\Users\\nxspirit\\dev\\test\\base_000_2017_06_26_23_05.ifs");


         //   f.Open("C:\\Users\\nxspirit\\dev\\test\\patch_2018_03_09_10_11.ifs");


            return CommandResultType.Continue;
        }

        public void Shutdown()
        {
        }
    }
}