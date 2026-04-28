namespace ZXY;

/// <summary>
/// 高斯/UTM投影接口
/// </summary>
public interface IProj
{
    string Id { get; }
    string Name { get; }
    void ResetProj(Ellipsoid ellipsoid);

    /// <summary>
    /// 投影正算，根据经纬度投影计算North-East坐标
    /// </summary>
    /// <param name="latitude">纬度，单位：弧度</param>
    /// <param name="longitude">经度，单位：弧度</param>
    /// <param name="centralMeridian">中央子午线经度，单位：弧度</param>
    /// <param name="falseNorthing">North坐标加常数，单位：km，北半球为0km， 南半球一般为10000km</param>
    /// <param name="zone">带号</param>
    /// <param name="falseEasting">East坐标加常数，  单位：km，一般为500km</param>
    /// <returns>North, East, 子午线收敛角γ，单位：弧度，长度比m </returns>
    (double north, double east, double gamma, double m) Forward(double latitude, double longitude, double centralMeridian,
        double falseEasting = 0.0, double zone = 0.0, double falseNorthing = 0.0);

    /// <summary>
    /// 投影反算，根据North-East坐标计算经纬度
    /// </summary>
    /// <param name="north">North坐标，单位：m</param>
    /// <param name="east">East坐标，单位：m</param>
    /// <param name="centralMeridianLongitude">中央子午线经度，单位：弧度</param>
    /// <param name="falseEast">East坐标加常数，单位：km，一般为500km</param>
    /// <param name="zone">带号</param> 
    /// <param name="falseNorth">North坐标加常数，单位：km，北半球为0km， 南半球一般为10000km</param>
    /// <returns>纬度，单位：弧度；经度，单位：弧度；子午线收敛角γ，单位：弧度，长度比m</returns>
    (double latitude, double longitude, double gamma, double m) Inverse(double north, double east, double centralMeridianLongitude,
       double falseEast = 0.0, double zone = 0.0, double falseNorth = 0.0);
}