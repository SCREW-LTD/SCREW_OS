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

using Cosmos.System.FileSystem.VFS;
using Cosmos.System.FileSystem;
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
        private static BootMode bootMode;
        private static CosmosVFS fileSystem = new CosmosVFS();
        private static GuiHost guiHost = new GuiHost();
        protected override void BeforeRun()
        {
            Console.Clear(); 
            VFSManager.RegisterVFS(fileSystem);

            SwitchBootMode(BootMenu.ChooseBootMode());
        }

        public static void SwitchBootMode(BootMode mode)
        {
            switch (mode)
            {
                case BootMode.Gui:
                    guiHost.Init();
                    break;
                case BootMode.Console:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine(SystemData.asciiLogo);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("\nSCREW: OS booted successfully.");
                    break;
                default:
                    ConsoleUtil.Message(ConsoleUtil.MessageType.ERR, "Unexpected boot mode.");
                    break;
            }
            bootMode = mode;
        }

        protected override void Run()
        {
            if (bootMode == BootMode.Gui)
            {
                guiHost.Run();
            }
            else if (bootMode == BootMode.Console)
            {
                Console.Write("\nscrewos@root: ");
                Terminal.ParseCommand(Console.ReadLine());
            }
        }
    }
}
