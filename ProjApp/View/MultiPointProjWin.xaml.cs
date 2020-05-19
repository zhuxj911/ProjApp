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
    /// Interaction logic for ProjWindow.xaml
    /// </summary>
    public partial class MultiPointProjWin : Window
    {
        private MultiPointProjWinVM vm;

        public MultiPointProjWin()
        {
            InitializeComponent();
            this.DataContext = vm = new MultiPointProjWinVM();
        }

        private void OnReadDataFile(object sender, RoutedEventArgs e)
        {
            if (true == this.BLtoXYRadioButton.IsChecked)
                vm.ReadDataFile();
            else if (true == this.XYtoBLRadioButton.IsChecked)
                vm.ReadDataFile();
        }

        private void OnWriteDataFile(object sender, RoutedEventArgs e)
        {
            vm.WriteDataFile();
        }

        private void OnClose(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OnProj(object sender, RoutedEventArgs e)
        {
            if (true == this.BLtoXYRadioButton.IsChecked)
                vm.BLtoXY();
            else if (true == this.XYtoBLRadioButton.IsChecked)
                vm.XYtoBL();
        }

        private void OnClearBL(object sender, RoutedEventArgs e)
        {
            vm.ClearBL();
        }

        private void OnClearXY(object sender, RoutedEventArgs e)
        {
            vm.ClearXY();
        }
    }
}
