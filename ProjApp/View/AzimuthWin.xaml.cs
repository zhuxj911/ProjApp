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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProjApp
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AzimuthWin : Window
    {
        private AzimuthWinVM vm;

        public AzimuthWin()
        {
            InitializeComponent();

            vm = new AzimuthWinVM();
            this.DataContext = vm;
        }

        
        private void BtnCal_Click(object sender, RoutedEventArgs e)
        {
            vm.Cal();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
