using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZXY
{
    /// <summary>
    /// 参考椭球
    /// </summary>
    public class Spheroid : NotificationObject
    {
        public string Id { get; set; }
        public string Name { get; set; }

        private double _a;
        public double a { 
            get => _a;
            set
            {
                if (value > 6371000)
                {
                    _a = value;
                    InitSpheroid(_a, _f);
                    RaisePropertyChanged("a");
                }
            } 
        }
        /// <summary>
        /// 扁率的分母 α = (a-b)/a = 1/f
        /// </summary>       
        private double _f;
        public double f
        {
            get => _f;
            set
            {
                if (value > 298 && value < 299)
                {
                    _f = value;
                    InitSpheroid(_a, _f);
                    RaisePropertyChanged("f");
                }
            }
        }


        private double b { get; set; }
        private double e2 { get;  set; }
        private double eT2 { get;  set; }
        private double A0 { get;  set; }
        private double A2 { get;  set; }
        private double A4 { get;  set; }
        private double A6 { get;  set; }
        private double A8 { get;  set; }

        public static Spheroid CreateBeiJing1954()
        {
            return new Spheroid(6378245, 298.3){ Id = "BJ54", Name = "1954北京坐标系" };
        }

        public static Spheroid CreateXiAn1980()
        {
            return new Spheroid(6378140, 298.257) { Id="XA80", Name="1980西安坐标系"};
        }

        public static Spheroid CreateWGS84()
        {
            var spheroid = new Spheroid(6378137, 298.257223560)
            {
                Id = "WGS84",
                Name = "WGS84大地坐标系"
            };
            return spheroid;
        }

        public static Spheroid CreateCGCS2000()
        {
            return new Spheroid(6378137, 298.257222101) { Id = "CGCS2000", Name = "CGCS2000大地坐标系" };
        }


        public static Spheroid CreateCustomSpheroid(double semimajor_axis,
            double inverse_flattening)
        {
            return new Spheroid(semimajor_axis, inverse_flattening) { Id = "CS00", Name ="自定义坐标系"};
        }

        private void InitSpheroid(double semimajor_axis, double inverse_flattening)
        {
            b = a * (1 - 1 / f);
            e2 = 1 - b / a * b / a;
            eT2 = a / b * a / b - 1;

            double m0 = a * (1 - e2);
            double e4 = e2 * e2;
            double e6 = e4 * e2;
            double e8 = e6 * e2;

            A0 = (1 + 0.75 * e2 + 45.0 / 64.0 * e4
                + 175.0 / 256.0 * e6 + 11025.0 / 16384.0 * e8) * m0;
            A2 = -0.5 * (0.75 * e2 + 15.0 / 16.0 * e4
                + 525.0 / 512.0 * e6 + 2205.0 / 2048.0 * e8) * m0;
            A4 = 0.25 * (15.0 / 64.0 * e4 + 105.0 / 256.0 * e6
                + 2205.0 / 4096.0 * e8) * m0; ;
            A6 = -(35.0 / 512.0 * e6 + 315.0 / 2048.0 * e8) * m0 / 6.0;
            A8 = 315.0 / 16384.0 * e8 * m0 / 8.0;
        }

        /// <summary>
        /// 构造参考椭球
        /// </summary>
        /// <param name="semimajor_axis">长半轴</param>
        /// <param name="inverse_flattening">扁率的分母</param>
        private Spheroid(double semimajor_axis, double inverse_flattening)
        {
            _a = semimajor_axis;
            _f = inverse_flattening;
            InitSpheroid(_a, _f);
        }

        public double funM(double sinB2)
        {
            return a * (1 - e2) / Math.Pow(1 - e2 * sinB2, 1.5);
        }
        public double funN(double sinB2)
        {
            return a / Math.Sqrt(1 - e2 * sinB2);
        }

        public double funR(double sinB2)
        {
            return Math.Sqrt(funM(sinB2) * funN(sinB2));
        }

        public double funG2(double cosB2)
        {
            return eT2 * cosB2;
        }

        public double funX(double B)
        {
            return A0 * B
                + A2 * Math.Sin(2 * B)
                + A4 * Math.Sin(4 * B)
                + A6 * Math.Sin(6 * B)
                + A8 * Math.Sin(8 * B);
        }


        public double funBf(double x)
        {
            double B0 = x / A0, Bi = 0;

            int i = 0;
            while (i < 1000)
            {
                i++;
                Bi = (x - (
                   +A2 * Math.Sin(2 * B0)
                   + A4 * Math.Sin(4 * B0)
                   + A6 * Math.Sin(6 * B0)
                   + A8 * Math.Sin(8 * B0))) / A0;

                if (Math.Abs(Bi - B0) < 1e-10) break;
                else
                    B0 = Bi;
            };

            return Bi;
        }

        

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
