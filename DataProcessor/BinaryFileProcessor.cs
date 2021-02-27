using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Abstractions;

namespace DataProcessor
{
    public class BinaryFileProcessor
    {
        private readonly IFileSystem _fileSystem;

        public string InputFilePath { get; }
        public string OutputFilePath { get; }

        public BinaryFileProcessor(string inputFilePath, string outputFilePath)
            : this(inputFilePath, outputFilePath, new FileSystem()) { }

        public BinaryFileProcessor(string inputFilePath, string outputFilePath,
            IFileSystem fileSystem)
        {
            InputFilePath = inputFilePath;
            OutputFilePath = outputFilePath;
            _fileSystem = fileSystem;
        }

        /// <summary>
        /// Simply finds the largest byte in the file and appends it to the end of the file
        /// </summary>
        public void Process()
        {
            using (Stream inputFileStream = _fileSystem.File.Open(InputFilePath, FileMode.Open, FileAccess.Read))
            using (BinaryReader binaryStreamReader = new BinaryReader(inputFileStream))
            using (Stream outputFileStream = _fileSystem.File.Create(OutputFilePath))
            using (BinaryWriter binaryStreamWriter = new BinaryWriter(outputFileStream))
            {
                byte largest = 0;

                // If we have more bytes to read, keep looping
                while (binaryStreamReader.BaseStream.Position < binaryStreamReader.BaseStream.Length)
                {
                    byte currentByte = binaryStreamReader.ReadByte();

                    binaryStreamWriter.Write(currentByte);

                    if (currentByte > largest)
                    {
                        largest = currentByte;
                    }
                }

                binaryStreamWriter.Write(largest);
            }
        }
    }
}
