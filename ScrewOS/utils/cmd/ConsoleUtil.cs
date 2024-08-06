using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrewOS.utils.cmd
{
    public class ConsoleUtil
    {
        public enum MessageType
        {
            INFO, WARN, ERR, SUCCESS
        };
        public static ConsoleColor[] colors = new ConsoleColor[]
        {
            ConsoleColor.Blue, ConsoleColor.Yellow, ConsoleColor.Red, ConsoleColor.Green
        };

        public static void Message(MessageType type, string message)
        {
            ConsoleColor cachedbg = Console.BackgroundColor;
            ConsoleColor cachedfg = Console.ForegroundColor;

            int idx = type switch
            {
                MessageType.INFO => 0,
                MessageType.WARN => 1,
                MessageType.ERR => 2,
                MessageType.SUCCESS => 3,
                _ => 0
            };
            string name = type switch
            {
                MessageType.INFO => "INFO",
                MessageType.WARN => "WARN",
                MessageType.ERR => "ERR",
                MessageType.SUCCESS => "SUCCESS",
                _ => "INFO"
            };
            int freq = type switch
            {
                MessageType.INFO => 600,
                MessageType.WARN => 500,
                MessageType.ERR => 500,
                MessageType.SUCCESS => 800,
                _ => 600
            };

            Console.BackgroundColor = colors[idx];
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine($"[{name}] -> {message}");

#pragma warning disable CA1416
            Console.Beep(freq, 20);
#pragma warning restore CA1416

            Console.BackgroundColor = cachedbg;
            Console.ForegroundColor = cachedfg;
        }

        public static void WriteBigText(string text, string bottomLine = "[RETURN -> SCROLL DOWN | ESC -> EXIT]")
        {
            string[] lines = text.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                Console.WriteLine(lines[i]);
                Console.Write(bottomLine);
                ConsoleKey key = Console.ReadKey(true).Key;
                Console.SetCursorPosition(0, Console.CursorTop);
                for (int _ = 0; _ < bottomLine.Length; _++) Console.Write(' ');
                Console.SetCursorPosition(0, Console.CursorTop);
                switch (key)
                {
                    case ConsoleKey.Escape: return;
                    default: continue;
                }
            }
        }
    }
}
