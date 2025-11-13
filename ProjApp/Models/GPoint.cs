using CommunityToolkit.Mvvm.ComponentModel;
using ProjApp.ViewModels;
using ZXY;

/**
* Model命名空间中的内容不应包括界面中的内容
*
* 试验源生成器
* 将基于字段名称创建生成的属性的名称。
* 生成器假定字段命名采用 lowerCamel、_lowerCamel 或者 m_lowerCamel，
* 并将之转换为 UpperCamel，以遵循正确的 .NET 命名约定。
* 生成的属性将始终具有公共访问器，但在声明该字段时可以使用任何可见性（建议使用 private）。
*/

namespace ProjApp.Models;

public partial class GPoint : ViewModelBase, IPoint
{
    private ZXY.Point point = new();

    public string Name
    {
        get => point.Name;
        set => SetProperty(point.Name, value, point, (pt, n) => pt.Name = n);
    }

    public double X
    {
        get => point.X;
        set => SetProperty(point.X, value, point, (pt, x) => pt.X = x);
    }

    public double Y
    {
        get => point.Y;
        set => SetProperty(point.Y, value, point, (pt, y) => pt.Y = y);
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
        get => ZXY.SurMath.DmsToRadian(dmsB);
        set => dmsB = ZXY.SurMath.RadianToDms(value);
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
        get => ZXY.SurMath.DmsToRadian(dmsL);
        set => dmsL = ZXY.SurMath.RadianToDms(value);
    }

    /// <summary>
    /// 度分秒的字符串
    /// </summary>
    [ObservableProperty] //使用源生成器生成 Gamma 属性
    private string gamma;


    [ObservableProperty] //使用源生成器生成 M 属性
    private double m;


    public override string ToString()
    {
        return $"{Name}, {X}, {Y}, {dmsB}, {dmsL}, {Gamma}, {M}";
    }
}