using ZXY;

/// <summary>
/// 针对Ellipsoid进行函数功能扩展
/// </summary>
public static class EllipsoidExtensions
{
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
    public static Ellipsoid ExpandEllipsoidByGeodeticDiff(this IEllipsoid ellipsoid, double Bm, double Hm)
    {
        var sinBm = Math.Sin(Bm);
        var da = Hm / Math.Sqrt(1 - ellipsoid.e2 * sinBm * sinBm); //广义微分公式
        return new Ellipsoid(ellipsoid.a + da, ellipsoid.f)
        {
            Id = "CS00",
            Name = $"{ellipsoid.Name}+{Hm})"
        };
    }

    /// <summary>
    /// 平均曲率半径法创建新的椭球体
    /// </summary>
    /// <param name="ellipsoid">原椭球</param>
    /// <param name="Bm">平均纬度，单位弧度<</param>
    /// <param name="Hm">平均大地高，单位m</param>
    /// <returns></returns>
    public static Ellipsoid ExpandEllipsoidByAverageR(this IEllipsoid ellipsoid, double Bm, double Hm)
    {
        var sinBm = Math.Sin(Bm);
        var da = Hm * (1 - ellipsoid.e2 * sinBm * sinBm)/ Math.Sqrt(1 - ellipsoid.e2); //平均曲率半径法
        return new Ellipsoid(ellipsoid.a + da, ellipsoid.f)
        {
            Id = "CS00",
            Name = $"{ellipsoid.Name}+{Hm})"
        };
    }

    /// <summary>
    /// 卯酉曲率半径法创建新的椭球体
    /// </summary>
    /// <param name="ellipsoid">原椭球</param>
    /// <param name="Bm">平均纬度，单位弧度</param>
    /// <param name="Hm">平均大地高，单位m</param>
    /// <returns></returns>
    public static Ellipsoid ExpandEllipsoidByPrimeVerticalCurvatureRadius(this IEllipsoid ellipsoid, double Bm, double Hm)
    {
        var sinBm = Math.Sin(Bm);
        var da = Hm *  Math.Sqrt(1 - ellipsoid.e2 * sinBm * sinBm); //卯酉曲率半径法
        return new Ellipsoid(ellipsoid.a + da, ellipsoid.f)
        {
            Id = "CS00",
            Name = $"{ellipsoid.Name}+{Hm})"
        };
    }

    /// <summary>
    /// 平面解析法创建新的椭球体
    /// </summary>
    /// <param name="ellipsoid">原椭球</param>
    /// <param name="Bm">平均纬度，单位弧度</param>
    /// <param name="Hm">平均大地高，单位m</param>
    /// <returns></returns>
    public static Ellipsoid ExpandEllipsoidByPlaneAnalysis(this IEllipsoid ellipsoid, double Bm, double Hm)
    {
        var sinB1 = Math.Sin(Bm);
        var N1 = ellipsoid.funN(sinB1 * sinB1);
        var B2 = Math.Atan(Math.Tan(Bm) * (1 + Hm * ellipsoid.e2/((N1+Hm)* (1-ellipsoid.e2)))); //平面解析法
        var N2 = (N1 + Hm)* Math.Cos(Bm)/ Math.Cos(B2);

        var sinB2 = Math.Sin(B2);
        var a2 = N2 *  Math.Sqrt(1 - ellipsoid.e2 * sinB2 * sinB2); 
        return new Ellipsoid(a2, ellipsoid.f)
        {
            Id = "CS00",
            Name = $"{ellipsoid.Name}+{Hm})"
        };
    }
    
    /// <summary>
    /// 直接法创建新的椭球体
    /// </summary>
    /// <param name="ellipsoid">原椭球</param>
    /// <param name="Bm">平均纬度，单位弧度</param>
    /// <param name="Hm">平均大地高，单位m</param>
    /// <returns></returns>
    public static Ellipsoid ExpandEllipsoidByDeltaB0(this IEllipsoid ellipsoid, double Bm, double Hm)
    {
        return new Ellipsoid(ellipsoid.a + Hm, ellipsoid.f)
        {
            Id = "CS00",
            Name = $"{ellipsoid.Name}+{Hm})"
        };
    }
}