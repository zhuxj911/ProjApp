using System.Collections.Generic;
using ProjApp.Models;


namespace Proj.Models;

public static class EllipsoidFactory
{
    public static List<XEllipsoid> EllipsoidList { get; } = [];
    
    //用于数据文件的读写，根据参考椭球带号查找相应椭球
    public static Dictionary<string, XEllipsoid> EllipsoidTypes { get; } = [];
    

    static EllipsoidFactory()
    { 
        //根据参考椭球类型查找相应椭球
        foreach (var it in ZXY.EllipsoidType.EllipsoidList)
        {
            var xe = new XEllipsoid(it);
            EllipsoidList.Add(xe);
            EllipsoidTypes.Add(xe.Id, xe); //用于数据文件的读写，根据参考椭球代号查找相应椭球
        }
    }
}