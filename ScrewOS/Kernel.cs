/*
=================================================================
   _____ __________  _______       __     ____  _____
  / ___// ____/ __ \/ ____/ |     / /    / __ \/ ___/
  \__ \/ /   / /_/ / __/  | | /| / (_)  / / / /\__ \ 
 ___/ / /___/ _, _/ /___  | |/ |/ /    / /_/ /___/ / 
/____/\____/_/ |_/_____/  |__/|__(_)   \____//____/ 

Main Kernel File
Licensed under GPL-3
=================================================================
*/

using IL2CPU.API.Attribs;
using ScrewOS.gui;
using ScrewOS.services.cmd;
using ScrewOS.utils.boot;
using ScrewOS.utils.cmd;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Sys = Cosmos.System;

namespace ScrewOS
{
    public class Kernel : Sys.Kernel
    {
        private BootMode bootMode;
        protected override void BeforeRun()
        {
            Console.Clear();
            bootMode = BootMenu.ChooseBootMode();

            switch (bootMode)
            {
                case BootMode.Gui:
                    GuiHost.Init();
                    break;
                case BootMode.Console:
                    InitCLI();
                    break;
                default:
                    ConsoleUtil.Message(ConsoleUtil.MessageType.ERR, "Unexpected boot mode.");
                    break;
            }
        }

        private void InitCLI()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(SystemData.asciiLogo);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\nSCREW: OS booted successfully.");
        }

        protected override void Run()
        {
            Console.Write("\nscrewos@root: ");
            Terminal.ParseCommand(Console.ReadLine());
        }
    }
}
