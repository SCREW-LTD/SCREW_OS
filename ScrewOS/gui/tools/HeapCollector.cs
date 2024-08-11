using Cosmos.Core.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrewOS.gui.tools
{
    public static class HeapCollector
    {
        private static int frames = 0;

        public static void Collect()
        {
            frames++;

            if (frames % 8 == 1)
            {
                Heap.Collect();
                frames = 0;
            }
        }
    }
}
