using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrewOS.gui.tasks.interfaces
{
    public interface IWindow : GuiElement
    {
        int x { get; }
        int y { get; }
        int width { get; }
        int height { get; }

        void DragMove();
        void Close();
        void Minimize();
        void Maximize();
    }
}
