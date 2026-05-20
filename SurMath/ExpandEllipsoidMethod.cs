namespace ZXY;

// public enum ExpandEllipsoidMethod
// {
//     DeltaB0, //直接法
//     AverageR, //平均屈率半径法
//     PrimeVerticalCurvatureRadius, //卯酉圈半径法 radius of curvature in prime vertical
//     PlaneAnalysis, //平面解析法
//     GeodeticDiff, //大地微分法  Geodetic Differential
// }

public class ExpandEllipsoidMethod
{
    public string Id { get; set; }
    public string Name { get; set; }

    private ExpandEllipsoidMethod(string id, string name)
    {
        Id = id;
        Name = name;
    }

    public static List<ExpandEllipsoidMethod> ExpandMethods { get; } = [];

    static ExpandEllipsoidMethod()
    {
        ExpandMethods.Add(new("DeltaB0", "直接法"));
        ExpandMethods.Add(new("AverageR", "平均屈率半径法"));
        ExpandMethods.Add(new("PrimeVerticalCurvatureRadius", "卯酉圈半径法 "));
        ExpandMethods.Add(new("PlaneAnalysis", "平面解析法"));
        ExpandMethods.Add(new("GeodeticDiff", "广义微分法"));
    }

    public override string ToString() => Name;
}