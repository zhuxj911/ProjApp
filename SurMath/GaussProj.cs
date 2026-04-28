using System;

namespace ZXY;

/// <summary>
/// 椭球Ellipsoid 不依赖于 投影算法
/// 说明椭球Ellipsoid不应该包含投影算法
/// 投影算法依赖于椭球
/// Gauss-Kruger
/// </summary>
public class GaussProj : IProj
{
    public string Id { get; } = "GaussProj";
    public string Name { get; } = "高斯-克吕格投影";

    override public string ToString() => Name;

    private Ellipsoid ellipsoid;

    public GaussProj(Ellipsoid ellipsoid)
    {
        //this.ellipsoid = ellipsoid;
        ResetProj(ellipsoid);
    }

    public void ResetProj(Ellipsoid ellipsoid)
    {
        this.ellipsoid = ellipsoid;
    }

    internal (double n, double e, double gamma, double m) Forward(double lat, double ll)
    {
        double sinB = Math.Sin(lat);
        double cosB = Math.Cos(lat);
        double cosB2 = cosB * cosB;
        double cosB4 = cosB2 * cosB2;

        double t = Math.Tan(lat);
        double t2 = t * t;
        double t4 = t2 * t2;

        double g2 = ellipsoid.funG2(cosB2);
        double g4 = g2 * g2;

        double l2 = ll * ll;
        double l4 = l2 * l2;

        double X = ellipsoid.funX(lat);
        double N = ellipsoid.funN(sinB * sinB);

        double n = X + N * sinB * cosB * l2 * (
            0.5
            + cosB2 / 24.0 * (5 - t2 + 9 * g2 + 4 * g4) * l2
            + cosB4 / 720.0 * (61 - 58 * t2 + t4) * l4
        );
        double e = N * cosB * ll * (
            1 + cosB2 / 6.0 * (1 - t2 + g2) * l2
              + cosB4 / 120.0 * (5 - 18 * t2 + t4 + 14 * g2 - 58 * g2 * t2) * l4
        );

        double gamma = sinB * ll * (
            1
            + (1 + 3 * g2 + 2 * g4) / 3.0 * cosB2 * l2
            + (2 - t2) / 15.0 * cosB4 * l4
        );
        double m = 1 + 0.5 * l2 * cosB2 * (1 + g2) + l4 * cosB4 * (5 - 4 * t2) / 24.0;

        return (n, e, gamma, m);
    }

    /// <summary>
    ///  高斯投影正算，根据经纬度投影计算North-East坐标
    /// </summary>
    /// <param name="latitude">纬度，单位：弧度</param>
    /// <param name="longitude">经度，单位：弧度</param>
    /// <param name="centralMeridian">中央子午线经度，单位：弧度</param>
    /// <param name="falseEasting">East坐标加常数，  单位：km，一般为500km</param>
    /// <param name="zone">带号</param>
    /// <param name="falseNorthing">North坐标加常数，单位：km，高斯投影一般为0km</param>
    /// <returns>North, East, 子午线收敛角γ，单位：弧度，长度比m </returns>
    public (double north, double east, double gamma, double m) Forward(double latitude, double longitude, double centralMeridian, 
        double falseEasting = 0.0, double zone = 0.0, double falseNorthing = 0.0)
    {
        double dl = longitude - centralMeridian;
        var (north, east, gamma, m) = Forward(latitude, dl);
        return (north + falseNorthing * 1e3, east + falseEasting * 1e3 + zone * 1e6, gamma, m);
    }

    internal (double lat, double dl, double gamma, double m) Inverse(double n, double e)
    {
        double Bf = ellipsoid.funBf(n);
        double tf = Math.Tan(Bf);
        double tf2 = tf * tf;
        double tf4 = tf2 * tf2;

        double sinBf = Math.Sin(Bf);
        double sinBf2 = sinBf * sinBf;

        double Mf = ellipsoid.funM(sinBf2);
        double Nf = ellipsoid.funN(sinBf2);
        double Nf2 = Nf * Nf;
        double Nf4 = Nf2 * Nf2;

        double cosBf = Math.Cos(Bf);
        double gf2 = ellipsoid.funG2(cosBf * cosBf);

        double y2 = e * e;
        double y4 = y2 * y2;

        double lat = Bf + tf / Mf / Nf * y2 * (
            -0.5
            + y2 / 24.0 / Nf2 * (5 + 3 * tf2 + gf2 - 9 * gf2 * tf2)
            - y4 / 720.0 / Nf4 * (61 + 90 * tf2 + 45 * tf4)
        );
        double dl = e / Nf / cosBf * (
            1
            - y2 / 6.0 / Nf2 * (1 + 2 * tf2 + gf2)
            + y4 / 120.0 / Nf4 * (5 + 28 * tf2 + 24 * tf4 + 6 * gf2 + 8 * gf2 * tf2)
        );
        double gamma = tf / Nf * e * (
            1
            - (1 + tf2 - gf2) / 3.0 / Nf2 * y2
            + (2 + 5 * tf2 + 3 * tf4) / 15.0 / Nf4 * y4
        );

        double sinB = Math.Sin(lat);
        double sinB2 = sinB * sinB;
        double R = ellipsoid.funR(sinB2);
        double R2 = R * R;
        double R4 = R2 * R2;
        double m = 1 + y2 / 2.0 / R2 + y4 / 24.0 / R4;

        return (lat, dl, gamma, m);
    }


    /// <summary>
    /// 高斯投影反算，根据North-East坐标计算经纬度
    /// </summary>
    /// <param name="north">North坐标，单位：m</param>
    /// <param name="east">East坐标，单位：m</param>
    /// <param name="centralMeridian">中央子午线经度，单位：弧度</param>
    /// <param name="falseEasting">East坐标加常数，单位：km，一般为500km</param>
    /// <param name="zone">带号</param>
    /// <param name="falseNorthing">North坐标加常数，单位：km，高斯投影一般为0km</param> 
    /// <returns>纬度，单位：弧度；经度，单位：弧度；子午线收敛角γ，单位：弧度，长度比m</returns>
    public (double latitude, double longitude, double gamma, double m) Inverse(double north, double east, double centralMeridian, 
        double falseEasting = 0.0, double zone = 0.0, double falseNorthing = 0.0)
    {
        double ee = east - zone * 1e6 - falseEasting * 1e3;
        var (latitude, dl, gamma, m) = Inverse(north, ee);
        return (latitude, centralMeridian + dl, gamma, m);
    }
}