using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Windows.Input;
using ZXY;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace ProjApp
{
    public class AzimuthViewModel : ViewModelBase
    {
        public AzimuthViewModel()
        {
            if (IsInDesignMode)
            {
                // Code runs in Blend --> create design time data.
                PtA.Name = "A";
                PtA.X = 0;
                PtA.Y = 0;

                PtB.Name = "B";
                PtB.X = 1;
                PtB.Y = 1;
            }
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
        }

        private SPoint _ptA = new SPoint();

        public SPoint PtA { get => _ptA;}

        private SPoint _ptB = new SPoint();

        public SPoint PtB { get => _ptB; }

        private string _azName="坐标方位角";

        public string AzName
        {
            get { return _azName; }
            set
            {
                _azName = value;
                RaisePropertyChanged(() => AzName);
            }
        }

        private string _az;
        public string Az
        {
            get { return _az; }
            set
            {
                _az = value;
                RaisePropertyChanged(() => Az);
            }
        }

        private double _dist;

        public double Dist
        {
            get { return _dist; }
            set
            {
                _dist = value;
                RaisePropertyChanged(() => Dist);
            }
        }

        public void CalculateAzimuth()
        {
            Az = ZXY.SurMath.RADtoDMSString(
                ZXY.SurMath.Azimuth(PtA.X, PtA.Y, PtB.X, PtB.Y) );
            Dist = ZXY.SurMath.Distance(PtA.X, PtA.Y, PtB.X, PtB.Y);
            AzName = string.Format($"{PtA.Name}->{PtB.Name}坐标方位角");
        }

        #region 命令
        private RelayCommand calculateAzimuthCommand;
        public RelayCommand CalculateAzimuthCommand
        {
            get
            {
                if (calculateAzimuthCommand == null)
                    return new RelayCommand(() => CalculateAzimuth());
                return calculateAzimuthCommand;
            }
            set { calculateAzimuthCommand = value; }
        }
        #endregion
    }
}
