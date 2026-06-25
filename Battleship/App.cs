using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Themes.Fluent;
using System.Threading;

namespace BoardTest;

public class App : Application
{
    public static readonly ManualResetEventSlim Ready = new(false);

    public override void Initialize()
    {
        Styles.Add(new FluentTheme());
    }

    public override void OnFrameworkInitializationCompleted()
    {
        Ready.Set();
        base.OnFrameworkInitializationCompleted();
    }
}