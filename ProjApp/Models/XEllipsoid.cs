using CommunityToolkit.Mvvm.ComponentModel;
using ZXY;

namespace ProjApp.Models;

/// <summary>
/// 用于界面处理
/// </summary>
public partial class XEllipsoid : ObservableObject, IEllipsoid
{
    private readonly Ellipsoid ellipsoid;

    public Ellipsoid CurrentEllipsoid => ellipsoid;

    //public XEllipsoid() => this.ellipsoid = EllipsoidType.Ellipsoids["CGCS2000"];

    public XEllipsoid(Ellipsoid ellipsoid) => this.ellipsoid = ellipsoid;

    public string Name
    {
        get => ellipsoid.Name;
        set => SetProperty(ellipsoid.Name, value, ellipsoid, (ell, name) => ell.Name = name);
    }

    public override string ToString() => ellipsoid.ToString();

    public double a
    {
        get => ellipsoid.a;
        set => SetProperty(ellipsoid.a, value, ellipsoid, (ell, a) => ell.a = a);
    }

    public double f
    {
        get => ellipsoid.f;
        set => SetProperty(ellipsoid.f, value, ellipsoid, (ell, f) => ell.f = f);
    }

    public double b  => ellipsoid.b;     //短半轴
    public double c => ellipsoid.c; //极点处的子午线曲率半径

    public double e2 => ellipsoid.e2;
    public double eT2 => ellipsoid.eT2;

    public string Id 
    { 
        get => ellipsoid.Id; 
        set => SetProperty(ellipsoid.Id, value, ellipsoid, (ell, id) => ell.Id = id);
    }

    public bool IsCustomEllipsoid => ellipsoid.IsCustomEllipsoid;

    public double funM(double sinB2) => ellipsoid.funM(sinB2);

    public double funN(double sinB2) => ellipsoid.funN(sinB2);

    public double funR(double sinB2) => ellipsoid.funR(sinB2);

    public double funG2(double cosB2) => ellipsoid.funG2(cosB2);

    public double funX(double B) => ellipsoid.funX(B); 

    public double funBf(double x) => ellipsoid.funBf(x);

    public (double X, double Y, double Z) BLHtoXYZ(double B, double L, double H) => ellipsoid.BLHtoXYZ(B, L, H);

    public (double B, double L, double H) XYZtoBLH(double X, double Y, double Z) => ellipsoid.XYZtoBLH(X, Y, Z);
}
