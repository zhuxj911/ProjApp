namespace ZXY;

/// <summary>
/// UTM投影算法，基于高斯投影算法实现
/// </summary>
public class UtmProj : IProj
{
    public string Id { get; } = "UTMProj";
    public string Name { get; } = "UTM 投影";

    override public string ToString() => Name;

    private double k0 = 0.9996;
    private GaussProj proj;

    public UtmProj(Ellipsoid ellipsoid)
    {
        ResetProj(ellipsoid);
        //proj = new GaussProj(ellipsoid);
    }

    public void ResetProj(Ellipsoid ellipsoid)
    {
        proj = new GaussProj(ellipsoid);
    }


    /// <summary>
    /// UTM投影正算，根据经纬度投影计算North-East坐标
    /// </summary>
    /// <param name="latitude">纬度，单位：弧度</param>
    /// <param name="longitude">经度，单位：弧度</param>
    /// <param name="centralMeridian">中央子午线经度，单位：弧度</param>
    /// <param name="falseEasting">East坐标加常数，  单位：km，一般为500km</param>
    /// <param name="zone">带号</param>
    /// <param name="falseNorthing">North坐标加常数，单位：km，北半球为0km， 南半球一般为10000km</param>
    /// <returns>North, East, 子午线收敛角γ，单位：弧度，长度比m </returns>
    public (double north, double east, double gamma, double m) Forward(double latitude, double longitude, double centralMeridian,
        double falseEasting = 0.0, double zone = 0.0, double falseNorthing = 0.0)
    {
        var (north, east, gamma, m) = proj.Forward(latitude, longitude - centralMeridian);
        north = north * k0 + (latitude < 0.0 ? falseNorthing * 1e3 : 0.0); //如果纬度为南半球，则North坐标加上10000km的偏移
        return (north, east * k0 + falseEasting * 1e3 + zone * 1e6, gamma, m * k0);
    }


    /// <summary>
    /// UTM投影反算，根据North-East坐标计算经纬度
    /// </summary>
    /// <param name="north">North坐标，单位：m</param>
    /// <param name="east">East坐标，单位：m</param>
    /// <param name="centralMeridian">中央子午线经度，单位：弧度</param>
    /// <param name="falseNorthing">North坐标加常数，单位：km，北半球为0km， 南半球一般为10000km</param>
    /// <param name="falseEasting">East坐标加常数，单位：km，一般为500km</param>
    /// <param name="zone">带号</param>
    /// <returns>纬度，单位：弧度；经度，单位：弧度；子午线收敛角γ，单位：弧度，长度比m</returns>
    public (double latitude, double longitude, double gamma, double m) Inverse(double north, double east, double centralMeridian,
        double falseEasting = 0.0, double zone = 0.0, double falseNorthing = 0.0)
    {
        north -= falseNorthing * 1e3; //North坐标减去10000km的偏移
        east -= falseEasting * 1e3 - zone * 1e6; //East坐标减去500km的偏移与减去带号对应的偏移
        var (latitude, ll, gamma, m) = proj.Inverse(north/k0, east/k0);
        return (latitude, centralMeridian + ll, gamma, m * k0); //经度加上中央子午线经度，此处的长度比是按高斯投影算出的，需要的是UTM投影，所以需乘以k
    }


    /// <summary>
    /// 根据经度计算UTM带号
    /// </summary>
    /// <param name="lon">经度 (度)</param>
    /// <param name="lat">纬度 (度)</param>
    /// <returns>UTM带号 (1-60)</returns>
    public int GetZoneNumber(double lon, double lat)
    {
        //计算带号
        var zone = (int)((lon + 180) / 6) + 1;

        //特殊地区处理
        if (lat >= 72.0 && lat < 84.0)
        {
            if (lon >= 0.0 && lon < 9.0)
                zone = 31;
            else if (lon >= 9.0 && lon < 21.0)
                zone = 33;
            else if (lon >= 21.0 && lon < 33.0)
                zone = 35;
            else if (lon >= 33.0 && lon < 42.0)
                zone = 37;
        }
        else if (lat >= 56.0 && lat < 64.0)
        {
            if (lon >= 3.0 && lon < 12.0)
                zone = 32;
        }

        return zone;
    }

    /// <summary>
    /// 根据带号计算中央子午线经度
    /// </summary>
    /// <param name="zone">UTM带号 (1-60)</param>
    /// <returns>中央子午线经度 (度)</returns>
    public double GetCentralMeridian(int zone) => (zone - 1) * 6 - 180 + 3; //3度带中央经线
}
