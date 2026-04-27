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

public partial class GeoPoint : ViewModelBase, IPoint
{
    [ObservableProperty]
    private string name;
    
    /// <summary>
    /// 投影坐标系中的北坐标
    /// </summary>   
    [ObservableProperty]
    private double _N;

    /// <summary>
    /// 投影坐标系中的东坐标
    /// </summary>   
    [ObservableProperty]
    private double _E;

    /// <summary>
    /// 纬度，单位为度分秒
    /// </summary>
    [ObservableProperty]
    private double dmsB;


    /// <summary>
    /// 纬度，单位为弧度
    /// </summary>
    public double B
    {
        get => ZXY.SurMath.DmsToRadians(DmsB);
        set => DmsB = ZXY.SurMath.RadiansToDms(value);
    }

    
    /// <summary>
    /// 经度，单位为度分秒
    /// </summary>
    [ObservableProperty]
    private double dmsL;

    /// <summary>
    /// 经度，单位为弧度
    /// </summary>
    public double L
    {
        get => ZXY.SurMath.DmsToRadians(DmsL);
        set => DmsL = ZXY.SurMath.RadiansToDms(value);
    }

    /// <summary>
    /// 大地高
    /// </summary>
    [ObservableProperty]
    private double _H = 0.0;

    /// <summary>
    /// 子午线收敛角，单位：弧度
    /// </summary>
    [ObservableProperty]
    private double gamma;

    public string GammaDmsString => ZXY.SurMath.RadiansToDmsString(Gamma);

    public double GammaDms => ZXY.SurMath.RadiansToDms(Gamma);

    [ObservableProperty]
    private double m;

    /// <summary>
    /// 空间直角坐标 X
    /// </summary>
    [ObservableProperty]
    private double _X;

    /// <summary>
    /// 空间直角坐标 Y
    /// </summary>
    [ObservableProperty]
    private double _Y;

    /// <summary>
    /// 空间直角坐标 Z
    /// </summary>
    [ObservableProperty]
    private double _Z;

    public override string ToString()
    {
        return $"{Name}, {N}, {E}, {DmsB}, {DmsL}, {H}, {GammaDms}, {M}, {X}, {Y}, {Z}";
    }
}