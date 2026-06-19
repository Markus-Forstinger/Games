using Avalonia;
using BoardTest;
using System.Threading;

// Avalonia must own the main thread. The demo logic runs on a background thread.
Thread logicThread = new Thread(BoardDemo.Run) { IsBackground = true };
logicThread.Start();

AppBuilder.Configure<App>()

    .UsePlatformDetect()
    .LogToTrace()
    .StartWithClassicDesktopLifetime(args);
