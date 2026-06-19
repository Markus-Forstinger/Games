using Avalonia;
using Avalonia.Themes.Fluent;
using System.Threading;

namespace BoardTest;

public class App : Application
{
    internal static readonly ManualResetEventSlim Ready = new(false);

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
