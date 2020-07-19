using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EPLAN_Cable_Packing
{
    internal interface IReader : IDisposable
    {
        decimal? ReadEntry();
    }

    internal static class IntExtensions
    {
        public static bool IsWhiteSpace(this int c)
        {
            return c == ' ' || c == '\n' || c == '\r' || c == '\t';
        }
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
                {
                    if (line[0] != '#')
                        break;
                }

                if (line == null)
                {
                    return null;
                }

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
