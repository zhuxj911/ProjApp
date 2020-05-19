using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjApp
{
    public class SPoint : ZXY.GPoint
    {
        private double _dmsB;
        /// <summary>
        /// 纬度，单位为度分秒
        /// </summary>
        public double dmsB
        {
            get => _dmsB;
            set
            {
                _dmsB = value;
                RaisePropertyChanged("dmsB");
                RaisePropertyChanged("B");
            }
        }

        /// <summary>
        /// 纬度，单位为弧度
        /// </summary>
        public double B
        {
            get => ZXY.SurMath.DMStoRAD(dmsB);
            set
            {
                dmsB = ZXY.SurMath.RADtoDMS(value);
            }
        }  


        private double _dmsL;

        /// <summary>
        /// 经度，单位为度分秒
        /// </summary>
        public double dmsL
        {
            get => _dmsL;
            set
            {
                _dmsL = value;
                RaisePropertyChanged("dmsL");
                RaisePropertyChanged("L");
            }
        }

        /// <summary>
        /// 经度，单位为弧度
        /// </summary>
        public double L
        {
            get => ZXY.SurMath.DMStoRAD(dmsL);
            set
            {
                dmsL = ZXY.SurMath.RADtoDMS(value); 
            }
        }

        private double _Gamma;
        public double Gamma
        {
            get => _Gamma;
            set
            {
                _Gamma = value;
                RaisePropertyChanged("Gamma");
                RaisePropertyChanged("GammaDMSString");
            }
        }

        public string GammaDMSString
        {
            get => ZXY.SurMath.RADtoDMSString(_Gamma); 
        }


        private double _m;
        public double m
        {
            get => _m;
            set
            {
                _m = value;
                RaisePropertyChanged("m");
            }
        }

        public override string ToString()
        {
            return $"{Name}, {dmsB}, {dmsL}, {X}, {Y}";
        }

        public string ToString(string format)
        {
            if (format == "BL" || format == "BLXY")
            {
                return $"{Name}, {dmsB}, {dmsL}, {X}, {Y}, {GammaDMSString}, {m}";
            }
            else //XY XYBL
            {
                return $"{Name}, {X}, {Y}, {dmsB}, {dmsL}, {GammaDMSString}, {m}";
            }
        }
    }
}
