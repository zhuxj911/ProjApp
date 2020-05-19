using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXY;

namespace ProjApp.ViewModel
{
    public abstract class ProjWinVM : ZXY.NotificationObject
    {
        public List<Spheroid> SpheroidList
        {
            get => SpheroidFactory.GetSpheroidList();
        }

        private Spheroid currentSpheroid = Spheroid.CreateCGCS2000();
        public Spheroid CurrentSpheroid
        {
            get => currentSpheroid;
            set
            {
                currentSpheroid = value;
                RaisePropertyChanged("CurrentSpheroid");
            }
        }

        /// <summary>
        /// 中央子午线的经度，单位:弧度
        /// </summary>
        private double _L0;

        /// <summary>
        /// 中央子午线的经度，单位:度分秒
        /// </summary>
        public double dmsL0
        {
            get => ZXY.SurMath.RADtoDMS(_L0);
            set
            {
                _L0 = ZXY.SurMath.DMStoRAD(value);
                RaisePropertyChanged("dmsL0");
                RaisePropertyChanged("L0");
            }
        }

        /// <summary>
        /// 中央子午线的经度，单位:弧度
        /// </summary>
        public double L0
        {
            get => _L0;
            set
            {
                _L0 = value;
                RaisePropertyChanged("dmsL0");
                RaisePropertyChanged("L0");
            }
        }

        private int _N = 0;
        public int N
        {
            get => _N;
            set
            {
                _N = value;
                RaisePropertyChanged("N");
            }
        }

        private double _ykm = 0;
        public double YKM
        {
            get => _ykm;
            set
            {
                _ykm = value;
                RaisePropertyChanged("YKM");
            }
        }

        private string _angleFormat = "D.MMSS";
        public string AngleFormat
        {
            get => _angleFormat;
            set
            {
                _angleFormat = value;
                RaisePropertyChanged("AngleFormat");
            }
        }

        public abstract  void BLtoXY();
        public abstract void XYtoBL();
    }
}
