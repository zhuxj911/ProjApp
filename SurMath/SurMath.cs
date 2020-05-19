using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZXY
{
    public static class SurMath
    {
        public static void DMStoDMS(double dmsAngle,
            out int d, out int m, out double s)
        {           
            dmsAngle = dmsAngle * 10000;
            int angle = (int)Math.Round(dmsAngle);
            d = angle / 10000;
            m = (angle - d * 10000) / 100;
            s = dmsAngle - d*10000 - m*100;
        }

        public static string DMStoDMSString(double dmsAngle)
        {
            int f = dmsAngle >= 0 ? 1 : -1;
            char ff = f == 1 ? ' ' : '-';

            DMStoDMS(dmsAngle, out int d, out int m, out double s);

            return $"{ff}{f * d}°{f * m:00}′{f * s:00.#####}″";
        }

        public static double DMStoRAD(double dmsAngle)
        {
            DMStoDMS(dmsAngle, out int d, out int m, out double s);
            return (d + m / 60.0 + s / 3600.0) / 180.0 * Math.PI;
        }

        public static double DegreetoRAD(double degreeAngle)
        {
            return degreeAngle / 180.0 * Math.PI;
        }

        public static double RADtoDegree(double radAngle)
        {
            return radAngle / Math.PI * 180.0;
        }


        //1 30 35  => 1.3035
        public static void RADtoDMS(double radAngle, out int d,
           out int m, out double s)
        {
            radAngle = radAngle * 180.0 / Math.PI * 3600;
            int dAngle = (int)Math.Round(radAngle);
            d = dAngle / 3600;
            dAngle = dAngle - d * 3600;
            m = dAngle/60;
            s = radAngle - d*3600 - m*60;
        }

        public static double RADtoDMS(double radAngle)
        {         
            RADtoDMS(radAngle, out int d, out int m, out double s);
            return d + m / 100.0 + s / 10000.0;
        }


        public static string RADtoDMSString(double radAngle)
        {
            int f = radAngle >= 0 ? 1 : -1;
            char ff = f == 1 ? ' ' : '-';
           
            RADtoDMS(radAngle, out int d, out int m, out double s);

            return $"{ff}{f * d}°{f * m:00}′{f * s:00.#####}″";
        }


        public static double Azimuth(double xA, double yA,
            double xB, double yB, out double distance)
        {
            double dx = xB - xA;
            double dy = yB - yA;

            distance = Math.Sqrt(dx * dx + dy * dy);

            return Math.Atan2(dy, dx) + (dy < 0 ? 1 : 0) * 2 * Math.PI;
        }


        public static double Azimuth(double xA, double yA,
            double xB, double yB)
        {
            return Azimuth(xA, yA, xB, yB, out _);
        }

        public static double Distance(double xA, double yA,
            double xB, double yB)
        {
            Azimuth(xA, yA, xB, yB, out double d);
            return d;
        }
    }
}
