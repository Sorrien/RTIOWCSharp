using System;
using System.Diagnostics;

namespace RTIOWSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            var application = new RTIOWApplication();
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            application.Run();
            stopWatch.Stop();
            Console.WriteLine($"{stopWatch.ElapsedMilliseconds/1000.0f}");
        }
    }
}