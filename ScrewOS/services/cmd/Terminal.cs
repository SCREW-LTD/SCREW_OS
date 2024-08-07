using ScrewOS.utils.cmd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrewOS.services.cmd
{
    public class Terminal
    {
        public static readonly List<Command> _commands = new();
        static Terminal()
        {
            RegisterCommand(new ScrewOS.utils.cmd.commands.system.HelpCommand());
            RegisterCommand(new ScrewOS.utils.cmd.commands.system.LicenseCommand());
            RegisterCommand(new ScrewOS.utils.cmd.commands.system.RestartCommand());
            RegisterCommand(new ScrewOS.utils.cmd.commands.system.SystemInfoCommand());
            RegisterCommand(new ScrewOS.utils.cmd.commands.gui.GuiCommand());
            RegisterCommand(new ScrewOS.utils.cmd.commands.network.NetConfigCommand());
        }

        public static void RegisterCommand(Command command)
        {
            _commands.Add(command);
        }

        static (List<string>, Dictionary<string, string>) ParseCommandLine(string cmd)
        {
            List<string> args = new List<string>();
            Dictionary<string, string> kwargs = new Dictionary<string, string>();
            bool insideQuotes = false;
            int startIndex = 0;

            for (int i = 0; i < cmd.Length; i++)
            {
                if (cmd[i] == '"')
                {
                    insideQuotes = !insideQuotes;
                }
                else if (cmd[i] == ' ' && !insideQuotes)
                {
                    if (i > startIndex)
                    {
                        string arg = cmd.Substring(startIndex, i - startIndex);
                        args.Add(arg);
                    }
                    startIndex = i + 1;
                }
            }

            if (startIndex < cmd.Length)
            {
                string lastArg = cmd.Substring(startIndex);
                args.Add(lastArg);
            }

            for (int i = 0; i < args.Count; i++)
            {
                if (args[i].StartsWith('"') && args[i].EndsWith('"'))
                {
                    args[i] = args[i].Substring(1, args[i].Length - 2);
                }
            }

            for (int i = args.Count - 1; i >= 0; i--)
            {
                if (args[i].StartsWith("--"))
                {
                    string key = args[i].Substring(2);
                    string value = "true";

                    if (i + 1 < args.Count && !args[i + 1].StartsWith("--"))
                    {
                        value = args[i + 1];
                        args.RemoveAt(i + 1);
                    }

                    kwargs[key] = value;
                    args.RemoveAt(i);
                }
            }

            return (args, kwargs);
        }

        public static void ParseCommand(string cmd)
        {
            if (string.IsNullOrWhiteSpace(cmd))
                return;

            if (cmd.Trim().StartsWith("//")) return;

            (List<string> args, Dictionary<string, string> kwargs) = ParseCommandLine(cmd);

            if (args.Count == 0)
                return;

            Command command = _commands.FirstOrDefault(c => c.Alias.Contains(args[0], StringComparer.OrdinalIgnoreCase));
            if (command != null)
            {
                try
                {
                    command.Execute(args, kwargs);
                }
                catch (Exception ex)
                {
                    ConsoleUtil.Message(ConsoleUtil.MessageType.ERR, $"Error executing command: {ex.Message}");
                }
            }
            else
            {
                ConsoleUtil.Message(ConsoleUtil.MessageType.ERR, $"{args[0]} is not a known command or executable");
            }
        }
    }
}
