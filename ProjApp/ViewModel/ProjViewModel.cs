using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using ProjApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjApp.ViewModel
{
    public class ProjViewModel : ViewModelBase
    {
        public ProjViewModel()
        {
            GaussProj = new GaussProjModel();
        }

        private GaussProjModel gaussProj;
        public GaussProjModel GaussProj
        {
            get => gaussProj;
            set
            {
                gaussProj = value;
                RaisePropertyChanged(() => GaussProj);
            }
        }

        #region Commands
        private RelayCommand readDataFileCommand;
        public RelayCommand ReadDataFileCommand
        {
            get
            {
                if (readDataFileCommand == null)
                    return new RelayCommand(() => GaussProj.ReadDataFile());
                return readDataFileCommand;
            }
            set { readDataFileCommand = value; }
        }

        private RelayCommand saveDataFileCommand;
        public RelayCommand SaveDataFileCommand
        {
            get
            {
                if (saveDataFileCommand == null)
                    return new RelayCommand(() => GaussProj.WriteDataFile());
                return saveDataFileCommand;
            }
            set { saveDataFileCommand = value; }
        }

        private RelayCommand clearBLCommand;
        public RelayCommand ClearBLCommand
        {
            get
            {
                if (clearBLCommand == null)
                    return new RelayCommand(() => GaussProj.ClearBL());
                return clearBLCommand;
            }
            set { clearBLCommand = value; }
        }

        private RelayCommand clearXYCommand;
        public RelayCommand ClearXYCommand
        {
            get
            {
                if (clearXYCommand == null)
                    return new RelayCommand(() => GaussProj.ClearXY());
                return clearXYCommand;
            }
            set { clearXYCommand = value; }
        }

        private RelayCommand onBLtoXYCommand;
        public RelayCommand OnBLtoXYCommand
        {
            get
            {
                if (onBLtoXYCommand == null)
                    return new RelayCommand(() => GaussProj.BLtoXY());
                return onBLtoXYCommand;
            }
            set { onBLtoXYCommand = value; }
        }


        private RelayCommand onXYtoBLCommand;
        public RelayCommand OnXYtoBLCommand
        {
            get
            {
                if (onXYtoBLCommand == null)
                    return new RelayCommand(() => GaussProj.XYtoBL());
                return onXYtoBLCommand;
            }
            set { onXYtoBLCommand = value; }
        }


        private RelayCommand showAzimuthWindowCommand;
        /// <summary>
        /// 显示坐标方位角计算窗体
        /// </summary>
        public RelayCommand ShowAzimuthWindowCommand
        {
            get
            {
                if (showAzimuthWindowCommand == null)
                    showAzimuthWindowCommand = new RelayCommand(() => ShowAzimuthWindow());
                return showAzimuthWindowCommand;
            }
            set { showAzimuthWindowCommand = value; }
        }

        private void ShowAzimuthWindow()
        {
            AzimuthWin dlg = new AzimuthWin();
            dlg.ShowDialog();
        }
        #endregion
    }
}
