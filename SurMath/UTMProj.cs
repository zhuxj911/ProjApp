using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZXY
{
    public class UTMProj : IProj
    {
        private GaussProj gaussProj;
        private double k = 0.9996;

        private Ellipsoid spheroid;
        public UTMProj(Ellipsoid spheroid)
        {
            this.spheroid = spheroid;
            gaussProj = new GaussProj(spheroid);
        }


        public void Bltoxy(double B, double l, out double x, out double y, out double gamma, out double m)
        {
            gaussProj.Bltoxy(B, l, out x, out y, out gamma, out m);
            x = k * x;
            y = k * y;
            gamma = k * gamma;
            m = k * m;
        }

        public void BLtoXYKM(double B, double L, double L0, double YKM, double N0, out double X, out double Y, out double gamma, out double m)
        {
            double l = L - L0;
            this.Bltoxy(B, l, out X, out double y, out gamma, out m);
            Y = y + YKM * 1000 + N0 * 1e6;
        }


        public void BLtoXYKM(double B, double L, double L0, double YKM, double N0, out double X, out double Y)
        {
            this.BLtoXYKM(B, L, L0, YKM, N0, out X, out Y, out _, out _);
        }

        public void XYKMtoBL(double X, double Y, double L0, double YKM, double N0, out double B, out double L, out double gamma, out double m)
        {
            double y = Y - N0 * 1e6 - YKM * 1000;
            this.xytoBl(X, y, out B, out double l, out gamma, out m);
            L = l + L0;
        }


        public void XYKMtoBL(double X, double Y, double L0, double YKM, double N0, out double B, out double L)
        {
            throw new NotImplementedException();
        }

        public void xytoBl(double x, double y, out double B, out double l, out double gamma, out double m)
        {
            x = x / k;
            y = y / k;

            gaussProj.xytoBl(x, y, out B, out l, out gamma, out m);

            gamma = k * gamma;
            m = k * m;
        }
    }
}
