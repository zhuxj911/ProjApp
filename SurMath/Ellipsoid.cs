using System;
using System.IO;

namespace ZXY;

/// <summary>
/// 参考椭球类型
/// </summary>
public enum EllipsoidType
{
    CGCS2000 = 0,
    Xian1980 = 1,
    Beijing1954 = 2,
    WGS1984 = 3,
    GRS80 = 4,
    CS00 = 5
}


/// <summary>
/// 参考椭球
/// </summary>
public class Ellipsoid
{
    public EllipsoidType Id { get; set; } //约定CS00代表自定义参考椭球
    public bool IsCustomEllipsoid //用于控制界面，如果为自定义椭球，则可以改变 a f 文本输入框中的值
    {
        get => Id == EllipsoidType.CS00;
    }

    public string Name { get; set; }

    private double _a;
    public double a
    {
        get => _a;
        set
        {
            if (value > 6371000)
            {
                _a = value;
                InitEllipsoid();
            }
        }
    }
    /// <summary>
    /// 扁率的分母 α = (a-b)/a = 1/f
    /// </summary>       
    private double _f;
    public double f
    {
        get => _f;
        set
        {
            if (value > 298 && value < 299)
            {
                _f = value;
                InitEllipsoid();
            }
        }
    }

    public double b { get; private set; } //短半轴
	public double c => a * a / b; //极点处的子午线曲率半径
	
    public double e2 { get; private set; }
    public double eT2 { get; private set; }
    public double A0 { get; private set; }
    public double A2 { get; private set; }
    public double A4 { get; private set; }
    public double A6 { get; private set; }
    public double A8 { get; private set; }


    private void InitEllipsoid()
    {
        //防御性处理，防止界面上给a与f输入值0导致程序崩溃
        if (a <= 0 || f <= 0) return;

        b = a * (1 - 1 / f);
        e2 = 1 - b / a * b / a;
        eT2 = a / b * a / b - 1;

        //var ff = 1 / f;  //换个计算式 ff = 1/f
        //b = a * (1 - ff); //短半径 (m)
        //e2 = 2 * ff - ff * ff; //第一偏心率平方 e^2   ff = 1/f    e2 = 2 * ff - ff * ff;
        //eT2 = e2 / (1 - e2);   //第二偏心率平方 e'^2

        double m0 = a * (1 - e2);
        double e4 = e2 * e2;
        double e6 = e4 * e2;
        double e8 = e6 * e2;

        A0 = (1 + 0.75 * e2 + 45.0 / 64.0 * e4
              + 175.0 / 256.0 * e6 + 11025.0 / 16384.0 * e8) * m0;
        A2 = -0.5 * (0.75 * e2 + 15.0 / 16.0 * e4
                               + 525.0 / 512.0 * e6 + 2205.0 / 2048.0 * e8) * m0;
        A4 = 0.25 * (15.0 / 64.0 * e4 + 105.0 / 256.0 * e6
                                      + 2205.0 / 4096.0 * e8) * m0;
        A6 = -(35.0 / 512.0 * e6 + 315.0 / 2048.0 * e8) * m0 / 6.0;
        A8 = 315.0 / 16384.0 * e8 * m0 / 8.0;
    }

    /// <summary>
    /// 构造参考椭球
    /// </summary>
    /// <param name="semimajor_axis">长半轴</param>
    /// <param name="inverse_flattening">扁率的分母</param>
    public Ellipsoid(double semimajor_axis, double inverse_flattening)
    {
        //此处使用属性a, f接收参数，因此不需要调用函数 InitSpheroid
        this.a = semimajor_axis;
        this.f = inverse_flattening;
    }

    public override string ToString() => $"{Name}"; 
}