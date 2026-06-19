using Avalonia;
using BoardTest;
using System;
using System.Threading;

namespace Battleship
{
    class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Thread logicThread = new Thread(BoardDemo.Run) { IsBackground = true };
            logicThread.Start();

            AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace()
                .StartWithClassicDesktopLifetime(args);
        }
    }
}