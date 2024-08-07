using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrewOS.utils.cmd.commands.system
{
    public class SystemInfoCommand : Command
    {
        public override string[] Alias => new string[] { "systeminfo" };

        public override string Help => "Fetch system info";

        public override void Execute(List<string> args, Dictionary<string, string> kwargs)
        {
            BreakLine();
            Console.WriteLine("Operation system");
            Console.WriteLine($"SCREW: OS ({SystemData.version})");
            Console.WriteLine("Screw Shell");
            BreakLine();
            Console.WriteLine("System");
            Console.WriteLine($"CPU: {Cosmos.Core.CPU.GetCPUBrandString()}");
            Console.WriteLine($"RAM: {Cosmos.Core.CPU.GetAmountOfRAM()} MB");
            Console.WriteLine($"RAM BUSY: {Cosmos.Core.GCImplementation.GetUsedRAM() / 1024 / 1024} MB");
            Console.WriteLine("Drive: 0:\\");
            BreakLine();
        }

        static void BreakLine()
        {
            string breakLine = null;
            for (int i = 0; i < 50; i++)
            {
                breakLine += '=';
            }
            Console.WriteLine(breakLine);
        }
    }
}
