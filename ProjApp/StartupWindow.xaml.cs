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
        public StartupWindow()
        {
            InitializeComponent();
        }
        private void OnShowDialog<T>() where T:Window, new()
        {
            T frm = new T();
            frm.Owner = this;
            frm.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            frm.ShowDialog();
        }


        private void OnSinglePointProj_Click(object sender, RoutedEventArgs e)
        {
            OnShowDialog<SinglePointProjWin>();            
        }

        private void OnMultiPointProj_Click(object sender, RoutedEventArgs e)
        {          
            OnShowDialog<MultiPointProjWin>();
        }

        private void OnAzimuth_Click(object sender, RoutedEventArgs e)
        {
            OnShowDialog<AzimuthWin>();
        }
    }
}
