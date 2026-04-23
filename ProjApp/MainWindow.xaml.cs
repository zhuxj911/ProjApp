using ProjApp.Views;

namespace ProjApp;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void MenuItem_Calculate_AzimuthClick(object sender, System.Windows.RoutedEventArgs e)
    {
        AzimuthWindow azimuthWindow = new AzimuthWindow();
        azimuthWindow.ShowDialog();
    }

    private void MenuItem_Exist_Applicat_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        this.Close();
    }
}
