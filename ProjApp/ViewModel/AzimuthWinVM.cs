using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Windows.Input;
using ZXY;

namespace ProjApp
{
    class AzimuthWinVM : NotificationObject
    {
        public AzimuthWinVM()
        {
        }

        private GPoint _ptA = new GPoint() 
        {Name="A", X=0, Y=0};

        public GPoint PtA { get => _ptA;}

        private GPoint _ptB = new GPoint() 
        { Name = "B", X =1 , Y = 1 };

        public GPoint PtB { get => _ptB; }

        private string _azName="坐标方位角";

        public string AzName
        {
            get { return _azName; }
            set
            {
                _azName = value;
                RaisePropertyChanged("AzName");
            }
        }

        private string _az;
        public string Az
        {
            get { return _az; }
            set
            {
                _az = value;
                RaisePropertyChanged("Az");
            }
        }

        private double _dist;

        public double Dist
        {
            get { return _dist; }
            set
            {
                _dist = value;
                RaisePropertyChanged("Dist");
            }
        }

        public void Cal()
        {
            Az = ZXY.SurMath.RADtoDMSString(
                ZXY.SurMath.Azimuth(PtA.X, PtA.Y, PtB.X, PtB.Y) );
            Dist = ZXY.SurMath.Distance(PtA.X, PtA.Y, PtB.X, PtB.Y);
            AzName = string.Format($"{PtA.Name}->{PtB.Name}坐标方位角");
        }
    }
}
