using System;
using System.IO;

namespace EPLAN_Cable_Packing
{
    internal interface IReader : IDisposable
    {
        decimal? ReadEntry();
    }

    internal class InputProcessor : IReader
    {
        private readonly StreamReader _reader;

        public InputProcessor(string fileName)
        {
            try
            {
                _reader = new StreamReader(fileName);
            }
            catch
            {
                Console.WriteLine("File Error");
                Environment.Exit(0);
            }
        }

        public decimal? ReadEntry()
        {
            try
            {
                string line;
                while ((line = _reader.ReadLine()) != null)
                    if (line[0] != '#')
                        break;

                if (line == null) return null;

                if (decimal.TryParse(line, out var result)) return result;

                Console.WriteLine("File Formatting Error");
                Environment.Exit(0);
            }
            catch
            {
                Console.WriteLine("File IO Error");
                Environment.Exit(0);
            }

            return null;
        }

        public void Dispose()
        {
            _reader.Dispose();
        }
    }
}