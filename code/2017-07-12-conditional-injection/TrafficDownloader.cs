using System;
using System.Collections.Generic;
using System.Text;

namespace RefactoredFileProcessor
{
    class TrafficDownloader : IDownloader
    {
        public bool CanProcess(DownloadType type)
        {
            return type.HasFlag(DownloadType.Traffic);
        }

        public void Process()
        {
            Console.WriteLine("TrafficDownloader.Process");
        }
    }
}
