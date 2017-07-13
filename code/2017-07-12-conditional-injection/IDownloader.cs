using System;
using System.Collections.Generic;
using System.Text;

namespace RefactoredFileProcessor
{
    interface IDownloader
    {
        bool CanProcess(DownloadType type);
        void Process();
    }
}
