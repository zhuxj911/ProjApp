using ProjApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProjApp
{
    /// <summary>
    /// Interaction logic for StartupWindow.xaml
    /// </summary>
    public partial class StartupWindow : Window
    {
        private ProjWinVM vm;

        public StartupWindow()
        {
            InitializeComponent();
            this.DataContext = vm = new ProjWinVM();
        }
        private void OnShowDialog<T>() where T : Window, new()
        {
            T frm = new T();
            frm.Owner = this;
            frm.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            frm.ShowDialog();
        }


        private void OnAzimuth_Click(object sender, RoutedEventArgs e)
        {
            OnShowDialog<AzimuthWin>();
        }

        private void OnReadDataFile(object sender, RoutedEventArgs e)
        {
            vm.ReadDataFile();
        }

        private void OnWriteDataFile(object sender, RoutedEventArgs e)
        {
            vm.WriteDataFile();
        }

        private void OnClearBL(object sender, RoutedEventArgs e)
        {
            vm.ClearBL();
        }

        private void OnClearXY(object sender, RoutedEventArgs e)
        {
            vm.ClearXY();
        }

        private void OnBLtoXYProj(object sender, RoutedEventArgs e)
        {
            vm.BLtoXY();
        }

        private void OnXYtoBLProj(object sender, RoutedEventArgs e)
        {
            vm.XYtoBL();
        }
    }
}
