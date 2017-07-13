using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommandLine;

namespace RefactoredFileProcessor
{
    class Options
    {
        [Option('t', "types", Required = true,
            HelpText = "Download file type(s) to process")]        
        public IEnumerable<DownloadType> Types { get; set; }
    }

    class ProgramImplementation
    {
        private readonly DownloadProcessor _processor;

        public ProgramImplementation(DownloadProcessor processor)
        {
            _processor = processor;
        }

        public void Run(string[] args)
        {
            var parser = new Parser(settings => settings.CaseInsensitiveEnumValues = true);
            var result = parser.ParseArguments<Options>(args);
            
            result.WithParsed(options =>
            {
                // convert to flags
                var types = options.Types.Aggregate((i, t) => i | t);

                _processor.Process(types);
            });            
        }
    }
}
