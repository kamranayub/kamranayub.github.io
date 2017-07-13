using System;

namespace RefactoredFileProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            var processor = new DownloadProcessor(new IDownloader[]
            {
                new SalesDownloader(),
                new TrafficDownloader()
            });

            new ProgramImplementation(processor).Run(args);

            Console.ReadKey();
        }
    }
}