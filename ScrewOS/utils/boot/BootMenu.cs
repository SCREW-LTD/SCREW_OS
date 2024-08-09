using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrewOS.utils.boot
{
    public enum BootMode
    {
        Gui,
        Console,
        Cancel
    }

    internal static class BootMenu
    {
        private static void PrintOption(string text, bool selected)
        {
            Console.BackgroundColor = selected ? ConsoleColor.White : ConsoleColor.Black;
            Console.ForegroundColor = selected ? ConsoleColor.Black : ConsoleColor.White;

            Console.WriteLine(text);
        }

        private static void Render(int selIdx)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Clear();
            Console.WriteLine($"Select an option:\n");

            PrintOption("SCREW: OS (GUI)", selIdx == 0);
            PrintOption("SCREW: OS (CLI)", selIdx == 1);
            PrintOption("Shut Down", selIdx == 2);
            PrintOption("Restart", selIdx == 3);
        }

        private static BootMode Confirm(int selIdx)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;

            Console.Clear();

            Console.SetCursorPosition(0, 0);

            Console.CursorVisible = true;

            switch (selIdx)
            {
                case 0:
                    return BootMode.Gui;
                case 1:
                    return BootMode.Console;
                case 2:
                    Cosmos.System.Power.Shutdown();
                    return BootMode.Cancel;
                case 3:
                    Cosmos.System.Power.Reboot();
                    return BootMode.Cancel;
                default:
                    return BootMode.Cancel;
            }
        }

        internal static BootMode ChooseBootMode()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;

            Console.Clear();

            int selIdx = 0;

            while (true)
            {
                Render(selIdx);

                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.Enter:
                        return Confirm(selIdx);
                    case ConsoleKey.DownArrow:
                        selIdx++;
                        break;
                    case ConsoleKey.UpArrow:
                        selIdx--;
                        break;
                }

                if (selIdx < 0)
                {
                    selIdx = 3;
                }

                if (selIdx > 3)
                {
                    selIdx = 0;
                }
            }
        }
    }
}
