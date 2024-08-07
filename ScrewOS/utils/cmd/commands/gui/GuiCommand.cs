using Cosmos.System.Graphics;
using ScrewOS.gui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrewOS.utils.cmd.commands.gui
{
    internal class GuiCommand : Command
    {
        public override string[] Alias => new string[] { "gui" };

        public override string Help => "Launches the gui interface";

        private readonly HashSet<string> validResolutions = new HashSet<string>
        {
            "320x240",
            "640x480",
            "800x600",
            "1024x768",
            "1280x720",
            "1280x768",
            "1280x1024",
            "1366x768",
            "1680x1050",
            "1920x1080",
            "1920x1200"
        };
        public override void Execute(List<string> args, Dictionary<string, string> kwargs)
        {
            string resolution = "640x480";

            if (kwargs.TryGetValue("r", out string specifiedResolution))
            {
                if (validResolutions.Contains(specifiedResolution))
                {
                    resolution = specifiedResolution;
                }
                else
                {
                    ConsoleUtil.Message(ConsoleUtil.MessageType.ERR, $"Invalid resolution specified: {specifiedResolution}. Using default resolution {resolution}.");
                    return;
                }
            }

            var dimensions = resolution.Split('x');
            int width = int.Parse(dimensions[0]);
            int height = int.Parse(dimensions[1]);

            GuiHost.Init((uint)width, (uint)height);
        }
    }
}
