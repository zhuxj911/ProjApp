using System;
using ZXY;

/// <summary>
/// 针对Ellipsoid进行函数功能扩展
/// </summary>
public static class EllipsoidExtensions
{
    public static double funM(this Ellipsoid ellipsoid, double sinB2) => ellipsoid.a * (1 - ellipsoid.e2) / Math.Pow(1 - ellipsoid.e2 * sinB2, 1.5);

    public static double funN(this Ellipsoid ellipsoid, double sinB2) => ellipsoid.a / Math.Sqrt(1 - ellipsoid.e2 * sinB2);

    public static double funR(this Ellipsoid ellipsoid, double sinB2) => Math.Sqrt(ellipsoid.funM(sinB2) * ellipsoid.funN(sinB2));

    public static double funG2(this Ellipsoid ellipsoid, double cosB2) => ellipsoid.eT2 * cosB2;

    public static double funX(this Ellipsoid ellipsoid, double B) => ellipsoid.A0 * B
               + ellipsoid.A2 * Math.Sin(2 * B)
               + ellipsoid.A4 * Math.Sin(4 * B)
               + ellipsoid.A6 * Math.Sin(6 * B)
               + ellipsoid.A8 * Math.Sin(8 * B);

    public static double funBf(this Ellipsoid ellipsoid, double x)
    {
        double B0 = x / ellipsoid.A0, Bi = 0;

        int i = 0;
        while (i < 1000)
        {
            i++;
            Bi = (x - (
                ellipsoid.A2 * Math.Sin(2 * B0)
                + ellipsoid.A4 * Math.Sin(4 * B0)
                + ellipsoid.A6 * Math.Sin(6 * B0)
                + ellipsoid.A8 * Math.Sin(8 * B0))) / ellipsoid.A0;

            if (Math.Abs(Bi - B0) < 1e-10) break;
            else
                B0 = Bi;
        }

        return Bi;
    }

    public static (double X, double Y, double Z) BLHtoXYZ(this Ellipsoid ellipsoid, double B, double L, double H)
    {
        var sinB = Math.Sin(B);
        var cosB = Math.Cos(B);
        var N = ellipsoid.funN(sinB * sinB);

        var X = (N + H) * cosB * Math.Cos(L);
        var Y = (N + H) * cosB * Math.Sin(L);
        var Z = (N * (1 - ellipsoid.e2) + H) * sinB;
        return (X, Y, Z);
    }

    public static (double B, double L, double H) XYZtoBLH(this Ellipsoid ellipsoid, double X, double Y, double Z)
    {
        var L = Math.Atan2(Y, X);

        var pp = Math.Sqrt(X * X + Y * Y);
        var p = ellipsoid.c * ellipsoid.e2 / pp;
        var k = 1 + ellipsoid.eT2;

        var t0 = Z / pp;
        var ti = 0.0;
        while (true)
        {
            ti = Z / pp + p * t0 / Math.Sqrt(k + t0 * t0);
            if (Math.Abs(t0 - ti) < 1e-10) break;
            t0 = ti;
        }
        var B = Math.Atan(ti);
        var N = ellipsoid.funN(Math.Sin(B) * Math.Sin(B));
        var H = pp / Math.Cos(B) - N;
        return (B, L, H);
    }

    /// <summary>
    /// 椭球膨胀法创建新的椭球体
    /// 如果扁率不变， 根据 ff = 1/f    e2 = 2 * ff - ff * ff， 则第一、二偏心率 平方 e^2 均不变，
    /// 由卯酉圈半径 N = a / sqrt(1 - e^2 * sin(B)^2) 可知，
    /// 仅长半轴 a 发生变化， 变化量 da = Hm * sqrt(1 - e^2 * sin(Bm)^2)， 其中 Bm 为平均纬度， Hm 为平均高程
    /// 上面是用卯酉圈曲率半径公式推导的，似乎不正确
    /// 
    /// 根据大地微分公式，略去平移、旋转、尺度变化参数的影响， dH = da *[- N/a  * (1 - e^2 * sin(B)^2)]， 则 da = -dH / sqrt(1 - e^2 * sin(Bm)^2)
    /// 也就是源椭球的大地高为50m， 现变为0m， 变化量为 (H' - H0) = 0 -50 = -50m，
    /// 新的椭球的长半轴a 需变长为 da = -(-50) / sqrt(1 - e^2 * sin(Bm)^2) = 50 / sqrt(1 - e^2 * sin(Bm)^2)， 以满足新的椭球在平均纬度Bm处的高程为0m
    /// 如何验证呢？ 可以通过BLHtoXYZ方法， 将平均纬度Bm、平均经度Lm（任意值）和平均高程Hm（0m）转换为XYZ坐标， 
    /// 再将XYZ坐标转换回BLH坐标， 看看得到的B'、L'、H'是否满足 L' = Lm， H' = 0m
    /// 以下的计算采用大地微分公式
    /// 在新的椭球中，点的纬度会发生变化，如何规避这个问题呢？
    /// 由于新旧椭球的位置都没有变化，也就是其空间直角坐标值是不变的，因此进行如下操作：
    /// 在源椭球中: (B, L, H) -> (X, Y, Z)
    /// 在新的椭球中: (X, Y, Z) -> (B', L', H') - GaussKruger -> (N,E)
    /// </summary>
    /// <param name="ellipsoid">源椭球</param>
    /// <param name="Bm">平均纬度，单位：弧度</param>
    /// <param name="Hm">平均高程，单位：米</param>
    /// <returns>新的椭球体</returns>
    public static Ellipsoid ExpandEllipsoid(this Ellipsoid ellipsoid, double Bm, double Hm)
    {
        var sinBm = Math.Sin(Bm);
        var da = Hm / Math.Sqrt(1- ellipsoid.e2 *sinBm * sinBm);
        return new Ellipsoid(ellipsoid.a + da, ellipsoid.f) { 
                Id = EllipsoidType.CS00, 
                Name = $"{ellipsoid.Name}+{Hm})"
        };
    }
}