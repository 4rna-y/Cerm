using System;

namespace Cerm.Ui.Watcher
{
    public interface IMemoryUsage
    {
        double GetMemoryUsageMB();
        MemoryDetails GetDetailedMemoryInfo();
    }

    public class MemoryDetails
    {
        public double WorkingSetMB { get; set; }
        public double PeakWorkingSetMB { get; set; }
        public double PrivateMemoryMB { get; set; }
        public double VirtualMemoryMB { get; set; }
        public double PagedMemoryMB { get; set; }
        public double NonPagedMemoryMB { get; set; }
        public double PagedSystemMemoryMB { get; set; }
        public double GCMemoryMB { get; set; }
        public int Gen0Collections { get; set; }
        public int Gen1Collections { get; set; }
        public int Gen2Collections { get; set; }
        public int ThreadCount { get; set; }
        public TimeSpan ProcessUptime { get; set; }
    }
}