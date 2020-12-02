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
    public class Spheroid
    {
        public string Id { get; set; } //约定CS00代表自定义参考椭球
        public bool IsCustomSpheroid //用于控制界面，如果为自定义椭球，则可以改变 a f 文本输入框中的值
        {
            get => Id == "CS00";
        }

        public string Name { get; set; }

        private double _a;
        public double a
        {
            get => _a;
            set
            {
                if (value > 6371000)
                {
                    _a = value;                   
                    InitSpheroid();
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
                    InitSpheroid();
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
        
       
        private void InitSpheroid()
        {
            //防御性处理，防止界面上给a与f输入值0导致程序崩溃
            if (a <= 0 || f <= 0) return; 

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
        public Spheroid(double semimajor_axis, double inverse_flattening)
        {
            //此处使用属性a, f接收参数，因此不需要调用函数 InitSpheroid
            this.a = semimajor_axis;
            this.f = inverse_flattening;
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
