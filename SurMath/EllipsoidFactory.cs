using System.Collections.Generic;

namespace ZXY;

public static class EllipsoidFactory
{
    public static List<Ellipsoid> EllipsoidList { get; } = [];

    //用于数据文件的读写，根据参考椭球带号查找相应椭球
    public static Dictionary<EllipsoidType, Ellipsoid> Ellipsoids { get; } = [];

    public static Dictionary<string, Ellipsoid> IdEllipsoids { get; } = [];
    static EllipsoidFactory()
    {
        EllipsoidList.Add(
            new Ellipsoid(6378137, 298.257222101) { Id = EllipsoidType.CGCS2000, Name = "CGCS2000大地坐标系" });
        EllipsoidList.Add(
            new Ellipsoid(6378245, 298.3) { Id = EllipsoidType.Beijing1954, Name = "北京1954坐标系" });
        EllipsoidList.Add(
            new Ellipsoid(6378140, 298.257) { Id = EllipsoidType.Xian1980, Name = "西安1980坐标系" });
        EllipsoidList.Add(
            new Ellipsoid(6378137, 298.257223560) { Id = EllipsoidType.WGS1984, Name = "WGS1984大地坐标系" });
        EllipsoidList.Add(
            new Ellipsoid(6378137, 298.257222101) { Id = EllipsoidType.CS00, Name = "自定义坐标系" });

        //根据参考椭球类型查找相应椭球
        foreach (var it in EllipsoidList)
        {
            Ellipsoids.Add(it.Id, it);
        }

        //用于数据文件的读写，根据参考椭球代号查找相应椭球
        foreach (var it in EllipsoidList)
        {
            IdEllipsoids.Add(it.Id.ToString(), it);
        }
    }
}