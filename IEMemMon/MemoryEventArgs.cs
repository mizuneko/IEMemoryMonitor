using System;

namespace IEMemMon
{
    public class MemoryEventArgs : EventArgs
    {
        public DateTime Time { get; private set; }
        public long WorkingSet { get; private set; }

        public MemoryEventArgs(DateTime time, long workingSet)
        {
            this.Time = time;
            this.WorkingSet = workingSet;
        }
    }
}