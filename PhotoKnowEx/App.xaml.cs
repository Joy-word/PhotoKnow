using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using PhotoKnowEx.ViewModels;
using PhotoKnowEx.Views;

namespace PhotoKnowEx {
    public class App : Application {
        public override void Initialize() {
            AvaloniaXamlLoader.Load(this);
        }

        public static Avalonia.Controls.Window Main;

        public override void OnFrameworkInitializationCompleted() {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop) {
                desktop.MainWindow = new MainWindow {
                    DataContext = new MainWindowViewModel(),
                   
                };
                Main = desktop.MainWindow;
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
