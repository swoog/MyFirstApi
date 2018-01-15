using System;

namespace Cellenza.MyFirst.Console
{
    public class FileLogger : IFileLogger
    {
        public void Info(string message)
        {
            System.Console.WriteLine(message);
        }
    }
}