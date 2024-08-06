using ScrewOS.services.cmd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrewOS.utils.cmd.commands.system
{
    public class HelpCommand : Command
    {
        public override string[] Alias => new string[] { "help" };
        public override string Help => "Displays this help message";

        public override void Execute(List<string> args, Dictionary<string, string> kwargs)
        {
            Console.WriteLine("Available commands:");
            foreach (var command in Terminal._commands)
            {
                Console.WriteLine($"{string.Join(", ", command.Alias)} - {command.Help}");
            }
        }
    }
}
