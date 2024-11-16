using System.Windows;
using Wpf.Ui.Appearance;

namespace ProjApp.Pages;

public partial class SettingsPage
{
    public SettingsPage()
    {
        InitializeComponent();

        AppVersionTextBlock.Text = $"WPF UI - Simple Demo - {GetAssemblyVersion()}";

        if (ApplicationThemeManager.GetAppTheme() == ApplicationTheme.Dark)
        {
            DarkThemeRadioButton.IsChecked = true;
        }
        else
        {
            LightThemeRadioButton.IsChecked = true;
        }
    }

    private void OnLightThemeRadioButtonChecked(object sender, RoutedEventArgs e)
    {
        ApplicationThemeManager.Apply(ApplicationTheme.Light);
    }

    private void OnDarkThemeRadioButtonChecked(object sender, RoutedEventArgs e)
    {
        ApplicationThemeManager.Apply(ApplicationTheme.Dark);
    }

    private static string GetAssemblyVersion()
    {
        return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString()
               ?? string.Empty;
    }
}