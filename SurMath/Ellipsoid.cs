using System;
using System.IO;
using System.Xml.Linq;

namespace ZXY;

///// <summary>
///// 参考椭球类型
///// </summary>
//public enum EllipsoidType
//{
//    CGCS2000 = 0,
//    Xian1980 = 1,
//    Beijing1954 = 2,
//    WGS1984 = 3,
//    GRS80 = 4,
//    CS00 = 5
//}


/// <summary>
/// 参考椭球,主要用于内部计算
/// </summary>
public class Ellipsoid : IEllipsoid
{
    public string Id { get; set; } //约定CS00代表自定义参考椭球 Id用enum，感觉扩展性不够好，改为string更好一些
    public bool IsCustomEllipsoid => Id == "CS00"; //用于控制界面，如果为自定义椭球，则可以改变 a f 文本输入框中的值
     
    public string Name { get; set; }

    public override string ToString() => $"{Id}-{Name}";


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

   


    public  double funM(double sinB2) => a * (1 - e2) / Math.Pow(1 - e2 * sinB2, 1.5);

    public  double funN(double sinB2) => a / Math.Sqrt(1 - e2 * sinB2);

    public  double funR(double sinB2) => Math.Sqrt(funM(sinB2) * funN(sinB2));

    public  double funG2(double cosB2) => eT2 * cosB2;

    public  double funX( double B) => A0 * B
               + A2 * Math.Sin(2 * B)
               + A4 * Math.Sin(4 * B)
               + A6 * Math.Sin(6 * B)
               + A8 * Math.Sin(8 * B);

    public double funBf(double x)
    {
        double B0 = x / A0, Bi = 0;

        int i = 0;
        while (i < 1000)
        {
            i++;
            Bi = (x - (
                A2 * Math.Sin(2 * B0)
                + A4 * Math.Sin(4 * B0)
                + A6 * Math.Sin(6 * B0)
                + A8 * Math.Sin(8 * B0))) / A0;

            if (Math.Abs(Bi - B0) < 1e-10) break;
            else
                B0 = Bi;
        }

        return Bi;
    }

    public (double X, double Y, double Z) BLHtoXYZ(double B, double L, double H)
    {
        var sinB = Math.Sin(B);
        var cosB = Math.Cos(B);
        var N = funN(sinB * sinB);

        var X = (N + H) * cosB * Math.Cos(L);
        var Y = (N + H) * cosB * Math.Sin(L);
        var Z = (N * (1 - e2) + H) * sinB;
        return (X, Y, Z);
    }

    public (double B, double L, double H) XYZtoBLH(double X, double Y, double Z)
    {
        var L = Math.Atan2(Y, X);

        var pp = Math.Sqrt(X * X + Y * Y);
        var p = c * e2 / pp;
        var k = 1 + eT2;

        var t0 = Z / pp;
        var ti = 0.0;
        while (true)
        {
            ti = Z / pp + p * t0 / Math.Sqrt(k + t0 * t0);
            if (Math.Abs(t0 - ti) < 1e-10) break;
            t0 = ti;
        }
        var B = Math.Atan(ti);
        var N = funN(Math.Sin(B) * Math.Sin(B));
        var H = pp / Math.Cos(B) - N;
        return (B, L, H);
    }
}