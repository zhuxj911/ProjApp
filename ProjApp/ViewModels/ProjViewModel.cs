using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using Proj.Models;
using ProjApp.Models;
using ZXY;

namespace ProjApp.ViewModels;

public partial class ProjViewModel : ViewModelBase
{
    public List<XEllipsoid> EllipsoidList => EllipsoidFactory.EllipsoidList;
    public List<IProj> ProjList => ProjType.ProjList;

    private XEllipsoid currentEllipsoid = EllipsoidFactory.EllipsoidList[0];
    public XEllipsoid CurrentEllipsoid
    {
        get => currentEllipsoid;
        set
        {
            SetProperty(ref currentEllipsoid, value);
            Proj.ResetProj(currentEllipsoid);
        }
    }


    private IProj _proj = ProjType.ProjList[0];
    public IProj Proj
    {
        get => _proj;
        set
        {
            if (value != null)
            {
                _proj = value;
                _proj.ResetProj(CurrentEllipsoid);
                OnPropertyChanged(nameof(Proj));
            }
        }
    }


    /// <summary>
    /// 中央子午线的经度，单位:度分秒
    /// </summary>
    private double _dsmL0;

    /// <summary>
    /// 中央子午线的经度，单位:度分秒
    /// </summary>
    public double dmsL0
    {
        get => _dsmL0;
        set => SetProperty(ref _dsmL0, value);
    }

    /// <summary>
    /// 中央子午线的经度，单位:弧度
    /// </summary>
    public double L0
    {
        get => ZXY.SurMath.DmsToRadians(dmsL0);
        set => dmsL0 = ZXY.SurMath.RadiansToDms(value);
    }

    private int _N = 0;

    public int N
    {
        get => _N;
        set => SetProperty(ref _N, value);
    }

    private double _falseEast = 0;

    public double FalseEast
    {
        get => _falseEast;
        set => SetProperty(ref _falseEast, value);
    }

    private double _falseNorth = 0;
    public double FalseNorth
    {
        get => _falseNorth;
        set => SetProperty(ref _falseNorth, value);
    }


    private ObservableCollection<GeoPoint> pointList = new ObservableCollection<GeoPoint>();
    public ObservableCollection<GeoPoint> PointList => pointList;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Title))]
    private string fileName = "untitle";

    public string Title => $"测量螺丝刀(Ver2026)-{FileName}";

    [RelayCommand]
    public void BLtoNE()
    {
        foreach (var pnt in this.PointList)
        {
            var (n, e, gamma, m) = Proj.Forward(pnt.B, pnt.L, L0, FalseEast, N, FalseNorth);
            pnt.N = n;
            pnt.E = e;
            pnt.Gamma =  gamma;
            //pnt.GammaDmsString = ZXY.SurMath.RadiansToDmsString(gamma);
            pnt.M = m;
        }
    }

    [RelayCommand]
    public void NEtoBL()
    {
        foreach (var pnt in this.PointList)
        {
            var (lat, lon, gamma, m) = Proj.Inverse(pnt.N, pnt.E, L0, FalseEast, N, FalseNorth);
            pnt.B = lat;
            pnt.L = lon;
            pnt.Gamma = gamma;
            //pnt.GammaDmsString = ZXY.SurMath.RadiansToDmsString(gamma);
            pnt.M = m;
        }
    }


    [RelayCommand]
    public void BLHtoXYZ()
    {
        foreach (var pnt in this.PointList)
        {
            var (X, Y, Z) = CurrentEllipsoid.BLHtoXYZ(pnt.B, pnt.L, pnt.H);
            pnt.X = X;
            pnt.Y = Y;
            pnt.Z = Z;
        }
    }

    [RelayCommand]
    public void XYZtoBLH()
    {
        foreach (var pnt in this.PointList)
        {
            var (B, L, H) = CurrentEllipsoid.XYZtoBLH(pnt.X, pnt.Y, pnt.Z);
            pnt.B = B;
            pnt.L = L;
            pnt.H = H;
        }
    }


    [RelayCommand]
    public void ClearBL()
    {
        foreach (var pnt in this.PointList)
        {
            pnt.B = pnt.L = 0;
        }
    }

    [RelayCommand]
    public void ClearNE()
    {
        foreach (var pnt in this.PointList)
        {
            pnt.N = pnt.E = 0;
        }
    }

    [RelayCommand]
    public void ClearXYZ()
    {
        foreach (var pnt in this.PointList)
        {
            pnt.X = pnt.Y = pnt.Z = 0.0;
        }
    }

    [RelayCommand]
    public void NewFile()
    {
        FileName = "untitle";
        CurrentEllipsoid = EllipsoidFactory.EllipsoidTypes["CGCS2000"];
        dmsL0 = 0;
        FalseEast = 0;
        N = 0;
        PointList.Clear();
    }

    [RelayCommand]
    public void OpenFile()
    {
        OpenFileDialog dlg = new OpenFileDialog
        {
            DefaultExt = ".txt",
            Filter = "高斯投影坐标数据|*.txt|All File(*.*)|*.*"
        };
        if (dlg.ShowDialog() != true) return;
        FileName = dlg.FileName;

        using (StreamReader sr = new StreamReader(FileName))
        {
            string buffer;
            string[] items = null;
            //读入点的坐标数据
            this.PointList.Clear();
            while (true)
            {
                buffer = sr.ReadLine();
                if (null == buffer) break; //文件末尾

                if ("" == buffer.Trim()) continue; //空行略过
                if (buffer.Trim()[0] == '#') continue;//注释行略过

                //处理含 : 的项
                if (buffer.Contains<char>(':'))
                {
                    items = buffer.Split([':']);
                    string v = items[0].Trim();
                    var cap = v;
                    switch (cap)
                    {
                        case "CS":
                            string item2 = items[1].Trim();
                            var its = item2.Split([',']);
                            if ((item2 == "CS00"))
                            {
                                if (its is ["CS00", _, _])  //if (its.Length == 3 && its[0] == "CS00")
                                {
                                    CurrentEllipsoid = EllipsoidFactory.EllipsoidTypes["CS00"];
                                    CurrentEllipsoid.a = double.TryParse(its[1], out var va) ? va : 0.0;
                                    CurrentEllipsoid.f = double.TryParse(its[2], out var vf) ? vf : 1.0;
                                }
                            }
                            else //item2 == "BJ54" or item2 == "XA80" or item2 == "WGS84" or item2 == "CGCS2000"
                            {
                                CurrentEllipsoid = EllipsoidFactory.EllipsoidTypes[its[0]];
                            }
                            break;

                        case "L0":
                            //默认为 D.MMSS
                            dmsL0 = double.TryParse(items[1], out var vL0) ? vL0 : 0.0;
                            break;

                        case "YKM":
                            FalseEast = double.TryParse(items[1], out var vYKM) ? vYKM : 0.0;
                            break;

                        case "XKM":
                            FalseNorth = double.TryParse(items[1], out var vXKM) ? vXKM : 0.0;
                            break;

                        case "N":
                            N = int.TryParse(items[1], out var vN) ? vN : 0;
                            break;
                        case "PROJ":
                            Proj = ProjType.ProjTypes[items[1].Trim()]; //如果没有输错的话，此时items[1].Trim()的值为 GaussKruger or UTM
                            Proj.ResetProj(CurrentEllipsoid);
                            break;

                        default:
                            break;
                    }
                    continue; //处理完毕继续
                }

                items = buffer.Split([',']);
                var pnt = new GeoPoint();

                if (items.Length < 3)
                    continue; //少于三项数据，不是点的坐标数据，忽略
                else if (items.Length == 3)
                {
                    pnt.Name = items[0].Trim();
                    pnt.N = double.TryParse(items[1], out var vN) ? vN : 0.0;
                    pnt.E = double.TryParse(items[2], out var vE) ? vE : 0.0;
                }
                else if (items.Length == 5)
                {
                    pnt.Name = items[0].Trim();
                    pnt.N = double.TryParse(items[1], out var vN) ? vN : 0.0;
                    pnt.E = double.TryParse(items[2], out var vE) ? vE : 0.0;
                    //默认为 D.MMSS
                    pnt.DmsB = double.TryParse(items[3], out var vB) ? vB : 0.0;
                    pnt.DmsL = double.TryParse(items[4], out var vL) ? vL : 0.0;
                }
                else if (items.Length == 6)
                {
                    pnt.Name = items[0].Trim();
                    pnt.N = double.TryParse(items[1], out var vN) ? vN : 0.0;
                    pnt.E = double.TryParse(items[2], out var vE) ? vE : 0.0;
                    //默认为 D.MMSS
                    pnt.DmsB = double.TryParse(items[3], out var vB) ? vB : 0.0;
                    pnt.DmsL = double.TryParse(items[4], out var vL) ? vL : 0.0;
                    pnt.H = double.TryParse(items[5], out var vH) ? vH : 0.0;
                }
                else if (items.Length == 8)
                {
                    pnt.Name = items[0].Trim();
                    pnt.N = double.TryParse(items[1], out var vN) ? vN : 0.0;
                    pnt.E = double.TryParse(items[2], out var vE) ? vE : 0.0;
                    //默认为 D.MMSS
                    pnt.DmsB = double.TryParse(items[3], out var vB) ? vB : 0.0;
                    pnt.DmsL = double.TryParse(items[4], out var vL) ? vL : 0.0;
                    pnt.H = double.TryParse(items[5], out var vH) ? vH : 0.0;
                    pnt.M = double.TryParse(items[6], out var vM) ? vM : 0.0;
                    pnt.Gamma = double.TryParse(items[7], out var vGamma) ? vGamma : 0.0;
                }
                else if (items.Length == 11)
                {
                    pnt.Name = items[0].Trim();
                    pnt.N = double.TryParse(items[1], out var vN) ? vN : 0.0;
                    pnt.E = double.TryParse(items[2], out var vE) ? vE : 0.0;
                    //默认为 D.MMSS
                    pnt.DmsB = double.TryParse(items[3], out var vB) ? vB : 0.0;
                    pnt.DmsL = double.TryParse(items[4], out var vL) ? vL : 0.0;
                    pnt.H = double.TryParse(items[5], out var vH) ? vH : 0.0;
                    pnt.M = double.TryParse(items[6], out var vM) ? vM : 0.0;
                    pnt.Gamma = double.TryParse(items[7], out var vGamma) ? vGamma : 0.0;
                    pnt.X = double.TryParse(items[8], out var vX) ? vX : 0.0;
                    pnt.Y = double.TryParse(items[9], out var vY) ? vY : 0.0;
                    pnt.Z = double.TryParse(items[10], out var vZ) ? vZ : 0.0;
                }
                else if (items.Length == 13)
                {
                    pnt.Name = items[0].Trim();
                    pnt.N = double.TryParse(items[1], out var vN) ? vN : 0.0;
                    pnt.E = double.TryParse(items[2], out var vE) ? vE : 0.0;
                    //默认为 D.MMSS
                    pnt.DmsB = double.TryParse(items[3], out var vB) ? vB : 0.0;
                    pnt.DmsL = double.TryParse(items[4], out var vL) ? vL : 0.0;
                    pnt.H = double.TryParse(items[5], out var vH) ? vH : 0.0;
                    pnt.M = double.TryParse(items[6], out var vM) ? vM : 0.0;
                    pnt.Gamma = double.TryParse(items[7], out var vGamma) ? vGamma : 0.0;
                    pnt.X = double.TryParse(items[8], out var vX) ? vX : 0.0;
                    pnt.Y = double.TryParse(items[9], out var vY) ? vY : 0.0;
                    pnt.Z = double.TryParse(items[10], out var vZ) ? vZ : 0.0;
                    pnt.DmsDB = double.TryParse(items[11], out var vDB) ? vDB : 0.0;
                    pnt.DH = double.TryParse(items[12], out var vDH) ? vDH : 0.0;
                }
                this.PointList.Add(pnt);
            }
        }
    }

    [RelayCommand]
    private async Task SaveFileAsync()
    {
        if (FileName == "untitle")
            await SaveAsFileAsync();
        else
            await WriteFile();
    }

    [RelayCommand]
    private async Task SaveAsFileAsync()
    {
        SaveFileDialog dlg = new SaveFileDialog();
        dlg.DefaultExt = ".txt";
        dlg.Filter = "高斯投影坐标数据|*.txt|All File(*.*)|*.*";
        if (dlg.ShowDialog() != true) return;
        FileName = dlg.FileName;
        await  WriteFile();
    }

    private async Task WriteFile()
    {
        StringBuilder sb =new StringBuilder();
        sb.AppendLine("#数据文件中的 # : , 均应为英文字符");
        sb.AppendLine("#可以忽略0个空格的行");
        sb.AppendLine("#可以忽略有多个空格的行");
        sb.AppendLine("#CS 指定坐标系 Beijing1954 Xian1980 CGCS2000 WGS1984 GRS80 CS00");
        sb.AppendLine("#CS: Beijing1954");
        sb.AppendLine("#CS: Xian1980");
        sb.AppendLine("#CS: WGS1984");
        sb.AppendLine("#CS: CGCS2000");
        sb.AppendLine("#CS: GRS80");
        sb.AppendLine("#CS: CS00, 6378137, 298.257222101");
        if (CurrentEllipsoid.Id == "CS00")
        {
            sb.AppendLine($"CS: {CurrentEllipsoid.Id}, {CurrentEllipsoid.a}, {CurrentEllipsoid.f}");
        }
        else
        {
            sb.AppendLine($"CS: {CurrentEllipsoid.Id}");
        }
        sb.AppendLine("#PROJ 指定投影类型: 高斯-克吕格投影 -> GaussKruger   UTM投影 -> UTM");
        sb.AppendLine($"PROJ: {Proj.Id}");

        sb.AppendLine("#角度数据格式为D.MMSS");
        sb.AppendLine($"L0: {dmsL0}");
        sb.AppendLine($"YKM: {FalseEast}");
        sb.AppendLine($"XKM: {FalseNorth}");
        sb.AppendLine($"N: {N}");
        sb.AppendLine("#角度的单位，默认为 D.MMSS");
        sb.AppendLine("#ANGLE : DEGREE D.MMSSS RADIAN");
        sb.AppendLine("ANGLE: D.MMSSS");

        sb.AppendLine("#点名, N(m), E(m), B(D.MMSS), L(D.MMSS), H(m), M, γ(D.MMSS), X(m), Y(m), Z(m), ΔB, ΔH");
        foreach (var pnt in PointList)
        {
            sb.AppendLine(pnt.ToString());
        }

        using FileStream fs = new FileStream(FileName, FileMode.Create);
        await using var sr = new StreamWriter(fs, Encoding.UTF8);
        await sr.WriteAsync(sb.ToString());
    }
}