using Avalonia;
using BoardTest;
using System.Threading;

var gameThread = new Thread(() =>
{
    Console.WriteLine("Thread gestartet");
    BoardDemo.Run();
    Console.WriteLine("BoardDemo fertig");
}) { IsBackground = true };
gameThread.Start();

Console.WriteLine("Avalonia startet...");
AppBuilder.Configure<App>()
    .UsePlatformDetect()
    .LogToTrace()
    .StartWithClassicDesktopLifetime(args);