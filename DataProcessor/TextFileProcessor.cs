using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Abstractions;

namespace DataProcessor
{
    public class TextFileProcessor
    {
        private readonly IFileSystem _fileSystem;

        public string InputFilePath { get; }
        public string OutputFilePath { get; }

        // production code uses this
        public TextFileProcessor(string inputFilePath, string outputFilePath)
            : this(inputFilePath, outputFilePath, new FileSystem()) { }

        // testing code uses this
        public TextFileProcessor(string inputFilePath, string outputFilePath, IFileSystem fileSystem)
        {
            InputFilePath = inputFilePath;
            OutputFilePath = outputFilePath;
            _fileSystem = fileSystem;
        }

        /// <summary>
        /// Simply finds the second line of a text file and converts it to uppercase
        /// </summary>
        public void Process()
        {
            using (var inputStreamReader = _fileSystem.File.OpenText(InputFilePath))
            using (var outputStreamWriter = _fileSystem.File.CreateText(OutputFilePath))
            {
                var currentLineNumer = 1;
                while (!inputStreamReader.EndOfStream)
                {
                    string line = inputStreamReader.ReadLine();

                    if (currentLineNumer == 2)
                    {
                        Write(line.ToUpperInvariant());
                    }
                    else
                    {
                        Write(line);
                    }

                    currentLineNumer++;

                    void Write(string content)
                    {
                        bool isLastLine = inputStreamReader.EndOfStream;

                        if (isLastLine)
                        {
                            outputStreamWriter.Write(content);
                        }
                        else
                        {
                            outputStreamWriter.WriteLine(content);
                        }
                    }
                }
            }
        }
    }
}
