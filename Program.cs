using Avalonia;
using BoardTest;
using System;
using System.Threading;
Thread logicThread = new Thread(BoardDemo.Run) { IsBackground = true };
logicThread.Start();

AppBuilder.Configure<App>()
    .UsePlatformDetect()
    .LogToTrace()
    .StartWithClassicDesktopLifetime(args);