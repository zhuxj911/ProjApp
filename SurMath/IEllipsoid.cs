using System;
using System.IO;

namespace ZXY;


/// <summary>
/// 参考椭球接口
/// </summary>
public interface IEllipsoid
{
    string Id { get; set; }

    bool IsCustomEllipsoid { get; }

    string Name { get; set; }

 
    double a{ get; } 
	
    /// <summary>
    /// 扁率的分母 α = (a-b)/a = 1/f
    /// </summary>       
    double f{get; }

    double b { get;} //短半轴
    double c { get; } //极点处的子午线曲率半径

    double e2 { get;}
    double eT2 { get;}

    double funM(double sinB2);

    double funN(double sinB2);

    double funR(double sinB2);

    double funG2(double cosB2);

    double funX(double B);

    double funBf(double x);

    (double X, double Y, double Z) BLHtoXYZ(double B, double L, double H);

    (double B, double L, double H) XYZtoBLH(double X, double Y, double Z);
}