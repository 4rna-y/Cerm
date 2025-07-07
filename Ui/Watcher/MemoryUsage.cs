using System;
using System.Diagnostics;

namespace Cerm.Ui.Watcher
{
    public class MemoryUsage : IMemoryUsage
    {
        private readonly Process _currentProcess;
        private readonly DateTime _processStartTime;
        private const double BytesToMB = 1024.0 * 1024.0;

        public MemoryUsage()
        {
            _currentProcess = Process.GetCurrentProcess();
            _processStartTime = _currentProcess.StartTime;
        }

        public double GetMemoryUsageMB()
        {
            _currentProcess.Refresh();
            return _currentProcess.WorkingSet64 / BytesToMB;
        }

        public MemoryDetails GetDetailedMemoryInfo()
        {
            _currentProcess.Refresh();
            
            return new MemoryDetails
            {
                WorkingSetMB = _currentProcess.WorkingSet64 / BytesToMB,
                PeakWorkingSetMB = _currentProcess.PeakWorkingSet64 / BytesToMB,
                PrivateMemoryMB = _currentProcess.PrivateMemorySize64 / BytesToMB,
                VirtualMemoryMB = _currentProcess.VirtualMemorySize64 / BytesToMB,
                PagedMemoryMB = _currentProcess.PagedMemorySize64 / BytesToMB,
                NonPagedMemoryMB = _currentProcess.NonpagedSystemMemorySize64 / BytesToMB,
                PagedSystemMemoryMB = _currentProcess.PagedSystemMemorySize64 / BytesToMB,
                GCMemoryMB = GC.GetTotalMemory(false) / BytesToMB,
                Gen0Collections = GC.CollectionCount(0),
                Gen1Collections = GC.CollectionCount(1),
                Gen2Collections = GC.CollectionCount(2),
                ThreadCount = _currentProcess.Threads.Count,
                ProcessUptime = DateTime.Now - _processStartTime
            };
        }
    }
}