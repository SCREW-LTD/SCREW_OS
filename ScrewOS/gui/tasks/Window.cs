using Cosmos.System;
using Cosmos.System.Graphics;
using ScrewOS.gui.tasks.interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace ScrewOS.gui.tasks
{
    public class Window : IProcess, IWindow
    {
        public Guid PID { get; private set; }
        public string Name { get; private set; }
        public Guid ParentPID { get; private set; }
        public ProcessState State { get; private set; }
        public int Priority { get; set; }
        public int x { get; private set; }
        public int y { get; private set; }
        public int width { get; private set; }
        public int height { get; private set; }

        public Window(Guid pid, string name, Guid parentPid, int width, int height)
        {
            this.PID = pid;
            this.Name = name;
            this.ParentPID = parentPid;
            this.State = ProcessState.Stopped;
            this.Priority = 0;

            this.width = width;
            this.height = height;
            this.x = 50;
            this.y = 50;

            Start();
        }

        public ProcessInfo GetInfo()
        {
            return new ProcessInfo(PID, Name, ParentPID, State, Priority);
        }

        public void Render()
        {
            GuiHost.canvas.DrawFilledRectangle(Color.DarkGray, x, y, width, height);
        }

        public void Start()
        {
            State = ProcessState.Running;
        }

        public void Stop()
        {
            State = ProcessState.Stopped;
        }

        private bool isDragging;
        private int dragStartX;
        private int dragStartY;
        public void DragMove()
        {
            if(MouseManager.MouseState == MouseState.Left)
            {
                if (MouseManager.X >= this.x && MouseManager.X <= this.x + this.width && MouseManager.Y >= this.y && MouseManager.Y <= this.y + 30)
                {
                    if (!isDragging)
                    {
                        isDragging = true;
                        dragStartX = (int)MouseManager.X - this.x;
                        dragStartY = (int)MouseManager.Y - this.y;
                    }

                    this.x = (int)MouseManager.X - dragStartX;
                    this.y = (int)MouseManager.Y - dragStartY;
                }
            }
            else
            {
                isDragging = false;
            }
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public void Minimize()
        {
            throw new NotImplementedException();
        }

        public void Maximize()
        {
            throw new NotImplementedException();
        }
    }
}
