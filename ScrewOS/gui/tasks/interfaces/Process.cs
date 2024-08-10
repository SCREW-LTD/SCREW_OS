using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace ScrewOS.gui.tasks.interfaces
{
    public interface IProcess
    {
        Guid PID { get; }
        string Name { get; }
        Guid ParentPID { get; }
        ProcessState State { get; }
        int Priority { get; set; }

        void Start();
        void Stop();
        ProcessInfo GetInfo();
    }

    public enum ProcessState
    {
        Stopped,
        Running,
        Waiting,
        Terminated
    }

    public class ProcessInfo
    {
        public Guid PID { get; private set; }
        public string Name { get; private set; }
        public Guid ParentPID { get; private set; }
        public ProcessState State { get; private set; }
        public int Priority { get; private set; }

        public ProcessInfo(Guid pid, string name, Guid parentPid, ProcessState state, int priority)
        {
            PID = pid;
            Name = name;
            ParentPID = parentPid;
            State = state;
            Priority = priority;
        }
    }
}
