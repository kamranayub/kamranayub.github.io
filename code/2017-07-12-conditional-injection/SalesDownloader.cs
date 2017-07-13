using System;
using System.Collections.Generic;
using System.Text;

namespace RefactoredFileProcessor
{
    class SalesDownloader : IDownloader
    {
        public bool CanProcess(DownloadType type)
        {
            return type.HasFlag(DownloadType.Sales);
        }

        public void Process()
        {
            Console.WriteLine("SalesDownloader.Process");
        }
    }
}
