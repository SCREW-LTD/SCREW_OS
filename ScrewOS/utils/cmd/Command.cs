using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrewOS.utils.cmd
{
    public abstract class Command
    {
        public abstract string[] Alias { get; }
        public abstract string Help { get; }
        public abstract void Execute(List<string> args, Dictionary<string, string> kwargs);
    }
}
