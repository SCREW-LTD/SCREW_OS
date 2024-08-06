using ScrewOS.services.cmd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrewOS.utils.cmd.commands.system
{
    internal class RestartCommand : Command
    {
        public override string[] Alias => new string[] { "reboot" };
        public override string Help => "Reboot SCREW: OS";

        public override void Execute(List<string> args, Dictionary<string, string> kwargs)
        {
            Cosmos.System.Power.Reboot();
        }
    }
}
