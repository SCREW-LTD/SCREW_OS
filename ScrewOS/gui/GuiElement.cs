using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrewOS.gui
{
    public interface GuiElement
    {
        int zIndex { get; }
        void Render();
    }

    public interface GuiElementWithRerender : GuiElement
    {
        void ReRender();
    }
}
