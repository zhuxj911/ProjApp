using System;
using System.Windows;
using System.Windows.Controls;

namespace ProjApp.Views
{
    /// <summary>
    /// Interaction logic for ProjPage.xaml
    /// </summary>
    public partial class ProjPage : Page
    {
        public ProjPage()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("./Views/AzimuthPage.xaml", UriKind.Relative));
        }
    }
}