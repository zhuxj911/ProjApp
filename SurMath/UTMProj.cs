namespace ZXY;

/// <summary>
/// UTM投影算法，基于高斯投影算法实现
/// </summary>
public class UtmProj : IProj
{
    private double k = 0.9996;
    private GaussProj proj;

    public UtmProj(Ellipsoid ellipsoid)
    {
        this.proj = new GaussProj(ellipsoid);
    }

    /// <summary>
    /// UTM投影正算，根据经纬度投影计算North-East坐标
    /// </summary>
    /// <param name="lat">纬度，单位：弧度</param>
    /// <param name="lon">经度，单位：弧度</param>
    /// <param name="lon0">中央子午线经度，单位：弧度</param>
    /// <param name="ykm">Y坐标加常数，单位：km，一般为500km</param>
    /// <param name="zone">带号</param>
    /// <returns>North, East, 子午线收敛角γ，单位：弧度，长度比m </returns>
    public (double n, double e, double gamma, double m) Forward(double lat, double lon, double lon0, double ekm = 500.0, double zone = 0.0)
    {
        var (n, e, gamma, m) = proj.Forward(lat, lon - lon0);
        return (n * k, e * k + ekm * 1000 + zone * 1e6, gamma, m * k);
    }


    /// <summary>
    /// UTM投影反算，根据North-East坐标计算经纬度
    /// </summary>
    /// <param name="n">North坐标，单位：m</param>
    /// <param name="e">East坐标，单位：m</param>
    /// <param name="lon0">中央子午线经度，单位：弧度</param>
    /// <param name="ekm">East坐标加常数，单位：km，一般为500km</param>
    /// <param name="zone">带号</param>
    /// <returns>纬度，单位：弧度；经度，单位：弧度；子午线收敛角γ，单位：弧度，长度比m</returns>
    public (double lat, double lon, double gamma, double m) Inverse(double n, double e, double lon0, double ekm = 500, double zone = 0)
    {
        double ee = (e - zone * 1e6 - ekm * 1e3) / k;
        var (lat, ll, gamma, m) = proj.Inverse(n / k, ee);
        return (lat, lon0 + ll, gamma, m);
    }
}
