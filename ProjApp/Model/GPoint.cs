using Microsoft.Toolkit.Mvvm.ComponentModel;
/**
* Model命名空间中的内容不应包括界面中的内容
*/
namespace ProjApp.Model;

public class GPoint : ObservableObject
{
    private ZXY.Point point = new ZXY.Point();

    public string Name
    {
        get => point.Name;
        set => SetProperty(point.Name, value, point, (p, n) => p.Name = n);
    }

    public double X
    {
        get => point.X;
        set => SetProperty(point.X, value, point, (p, x) =>  p.X = x);
    }

    public double Y
    {
        get => point.Y;
        set => SetProperty(point.Y, value, point, (p, y) => p.Y = y);
    }


    private double _dmsB;
    /// <summary>
    /// 纬度，单位为度分秒
    /// </summary>
    public double dmsB
    {
        get => _dmsB;
        set => SetProperty(ref _dmsB, value);
    }

    /// <summary>
    /// 纬度，单位为弧度
    /// </summary>
    public double B
    {
        get => ZXY.SurMath.DMStoRAD(dmsB);
        set => dmsB = ZXY.SurMath.RADtoDMS(value);
    }  

    private double _dmsL;

    /// <summary>
    /// 经度，单位为度分秒
    /// </summary>
    public double dmsL
    {
        get => _dmsL;
        set => SetProperty(ref _dmsL, value);
    }

    /// <summary>
    /// 经度，单位为弧度
    /// </summary>
    public double L
    {
        get => ZXY.SurMath.DMStoRAD(dmsL);
        set => dmsL = ZXY.SurMath.RADtoDMS(value);
    }

    private string _Gamma;
    /// <summary>
    /// 度分秒的字符串
    /// </summary>
    public string Gamma
    {
        get => _Gamma;
        set => SetProperty(ref _Gamma, value);
    }


    private double _m;
    public double m
    {
        get => _m;
        set => SetProperty(ref _m, value);
    }

    public override string ToString()
    {
        return $"{Name}, {X}, {Y}, {dmsB}, {dmsL}, {Gamma}, {m}";
    }

}
