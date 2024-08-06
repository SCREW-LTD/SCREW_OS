using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrewOS.utils.cmd.commands.system
{
    public class LicenseCommand : Command
    {
        public override string[] Alias => new string[] { "license" };
        public override string Help => "Show project license";

        public override void Execute(List<string> args, Dictionary<string, string> kwargs)
        {
            if (kwargs.TryGetValue("brief", out string printBrief) && printBrief == "true")
                Console.WriteLine(License.licenseBrief);
            else
                ConsoleUtil.WriteBigText(License.license);
        }
    }
}
