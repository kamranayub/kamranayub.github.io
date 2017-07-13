using System;
using System.Collections.Generic;
using System.Text;

namespace RefactoredFileProcessor
{
    class DownloadProcessor
    {
        private readonly IEnumerable<IDownloader> _downloaders;

        public DownloadProcessor(IEnumerable<IDownloader> downloaders)
        {
            _downloaders = downloaders;
        }

        public void Process(DownloadType types)
        {
            foreach (var downloader in _downloaders)
            {
                if (downloader.CanProcess(types))
                {
                    downloader.Process();
                }
            }
        }
    }
}
