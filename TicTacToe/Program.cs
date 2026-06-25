using Avalonia;
using BoardTest;
using System.Threading;

var gameThread = new Thread(() =>
{
    BoardDemo.Run();
}) { IsBackground = true };
gameThread.Start();

AppBuilder.Configure<App>()
    .UsePlatformDetect()
    .LogToTrace()
    .StartWithClassicDesktopLifetime(args);