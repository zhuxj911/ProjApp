using System.Collections.Generic;

namespace ZXY;

public static class EllipsoidType
{
    public static List<Ellipsoid> EllipsoidList { get; } = [];
    
    //用于数据文件的读写，根据参考椭球带号查找相应椭球
    public static Dictionary<string, Ellipsoid> Ellipsoids { get; } = [];

    static EllipsoidType()
    {
        //ToDO 此处的椭球参数可以从配置文件中读取，或者从数据库中读取，或者从网络上读取，或者从用户输入中读取，等等
        EllipsoidList.Add(
            new Ellipsoid(6378137, 298.257222101) { Id = "CGCS2000", Name = "CGCS2000大地坐标系" });
        EllipsoidList.Add(
            new Ellipsoid(6378245, 298.3) { Id = "Beijing1954", Name = "北京1954坐标系" });
        EllipsoidList.Add(
            new Ellipsoid(6378140, 298.257) { Id = "Xian1980", Name = "西安1980坐标系" });
        EllipsoidList.Add(
            new Ellipsoid(6378137, 298.257223563) { Id = "WGS1984", Name = "WGS1984大地坐标系" });
        //EllipsoidList.Add(
        //    new Ellipsoid(6378137.0, 298.257222101) { Id = "GRS80", Name = "GRS80大地坐标系" });
        EllipsoidList.Add(
            new Ellipsoid(6378137, 298.257222101) { Id = "CS00", Name = "自定义坐标系" });

        //根据参考椭球类型查找相应椭球
        foreach (var it in EllipsoidList)
        {
            Ellipsoids.Add(it.Id, it);
        }
    }
}