namespace ZXY;

/// <summary>
/// 高斯/UTM投影接口
/// </summary>
public interface IProj
{
    /// <summary>
    /// 投影正算，根据经纬度投影计算North-East坐标
    /// </summary>
    /// <param name="lat">纬度，单位：弧度</param>
    /// <param name="lon">经度，单位：弧度</param>
    /// <param name="lon0">中央子午线经度，单位：弧度</param>
    /// <param name="ykm">Y坐标加常数，单位：km，一般为500km</param>
    /// <param name="zone">带号</param>
    /// <returns>North, East, 子午线收敛角γ，单位：弧度，长度比m </returns>
    (double n, double e, double gamma, double m) Forward(double lat, double lon, double lon0, double ekm = 500.0, double zone = 0.0);

    /// <summary>
    /// 投影反算，根据North-East坐标计算经纬度
    /// </summary>
    /// <param name="n">North坐标，单位：m</param>
    /// <param name="e">East坐标，单位：m</param>
    /// <param name="lon0">中央子午线经度，单位：弧度</param>
    /// <param name="ekm">East坐标加常数，单位：km，一般为500km</param>
    /// <param name="zone">带号</param>
    /// <returns>纬度，单位：弧度；经度，单位：弧度；子午线收敛角γ，单位：弧度，长度比m</returns>
    (double lat, double lon, double gamma, double m) Inverse(double n, double e, double lon0, double ekm = 500.0, double zone = 0.0);
}