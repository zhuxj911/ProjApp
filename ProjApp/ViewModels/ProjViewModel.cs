using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using ProjApp.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using ZXY;

namespace ProjApp.ViewModels;

public partial class ProjViewModel : ViewModelBase
{
    public List<Ellipsoid> EllipsoidList => EllipsoidFactory.EllipsoidList;
    public List<IProj> ProjList => EllipsoidFactory.ProjList;

    private Ellipsoid currentEllipsoid = EllipsoidFactory.EllipsoidList[0];
    public Ellipsoid CurrentEllipsoid
    {
        get => currentEllipsoid;
        set
        {
            SetProperty(ref currentEllipsoid, value);
            Proj.ResetProj(currentEllipsoid);
        }
    }

    private IProj _proj = EllipsoidFactory.ProjList[0];
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
        CurrentEllipsoid = EllipsoidFactory.Ellipsoids[EllipsoidType.CGCS2000];
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

                    string itemName = items[0].Trim();
                    switch (itemName)
                    {
                        case "CS":
                            string item2 = items[1].Trim();
                            if ((item2 == "CS00"))
                            {
                                string[] its = item2.Split([':']);
                                if (its.Length == 3 && its[0] == "CS00")
                                {
                                    CurrentEllipsoid = EllipsoidFactory.Ellipsoids[EllipsoidType.CS00];
                                    CurrentEllipsoid.a = double.Parse(its[1]);
                                    CurrentEllipsoid.f = double.Parse(its[2]);
                                }
                            }
                            else //item2 == "BJ54" or item2 == "XA80" or item2 == "WGS84" or item2 == "CGCS2000"
                            {
                                CurrentEllipsoid = EllipsoidFactory.IdEllipsoids[item2];
                            }
                            break;

                        case "L0":
                            //默认为 D.MMSS
                            dmsL0 = double.Parse(items[1]);
                            break;

                        case "YKM":
                            FalseEast = double.Parse(items[1]);
                            break;

                        case "XKM":
                            FalseNorth = double.Parse(items[1]);
                            break;

                        case "N":
                            N = int.Parse(items[1]);
                            break;
                        case "PROJ":                            
                            Proj = EllipsoidFactory.IdProjs[items[1].Trim()]; //如果没有输错的话，此时items[1].Trim()的值为GaussProj or UTMProj
                            Proj.ResetProj(CurrentEllipsoid);
                            break;

                        default:
                            break;
                    }
                    continue; //处理完毕继续
                }

                items = buffer.Split([',']);
                if (items.Length < 3) continue; //少于三项数据，不是点的坐标数据，忽略
                GeoPoint pnt = new GeoPoint();
                pnt.Name = items[0].Trim();
                pnt.N = double.Parse(items[1]);
                pnt.E = double.Parse(items[2]);

                if (items.Length >= 5)
                {
                    //默认为 D.MMSS
                    pnt.DmsB = double.Parse(items[3]);
                    pnt.DmsL = double.Parse(items[4]);
                }
                this.PointList.Add(pnt);
            }
        }
    }

    [RelayCommand]
    public void SaveFile()
    {
        if (FileName == "untitle")
            SaveAsFile();
        else
            WriteFile();
    }

    [RelayCommand]
    public void SaveAsFile()
    {
        SaveFileDialog dlg = new SaveFileDialog();
        dlg.DefaultExt = ".txt";
        dlg.Filter = "高斯投影坐标数据|*.txt|All File(*.*)|*.*";
        if (dlg.ShowDialog() != true) return;
        FileName = dlg.FileName;
        WriteFile();
    }

    private void WriteFile()
    {
        using (FileStream fs = new FileStream(FileName, FileMode.Create))
        {
            StreamWriter sr = new StreamWriter(fs);

            sr.WriteLine("#数据文件中的 # : , 均应为英文字符");
            sr.WriteLine("#可以忽略0个空格的行");
            sr.WriteLine("#可以忽略有多个空格的行");
            sr.WriteLine("#CS 指定坐标系 Beijing1954 Xian1980 CGCS2000 WGS1984 GRS80 CS00");
            sr.WriteLine("#CS: Beijing1954");
            sr.WriteLine("#CS: Xian1980");
            sr.WriteLine("#CS: WGS1984");
            sr.WriteLine("#CS: CGCS2000");
            sr.WriteLine("#CS: GRS80");
            sr.WriteLine("#CS: CS00, 6378137, 298.257222101");
            if (CurrentEllipsoid.Id.ToString() == "CS00")
            {
                sr.WriteLine($"CS: {CurrentEllipsoid.Id.ToString()}, {CurrentEllipsoid.a}, {CurrentEllipsoid.f}");
            }
            else
            {
                sr.WriteLine($"CS: {CurrentEllipsoid.Id.ToString()}");
            }
            sr.WriteLine("#PROJ 指定投影类型: 高斯投影 -> GaussProj   UTM投影 -> UTMProj");
            sr.WriteLine($"PROJ: {Proj.Id}");

            sr.WriteLine("#角度数据格式为D.MMSS");
            sr.WriteLine($"L0: {dmsL0}");
            sr.WriteLine($"YKM: {FalseEast}");
            sr.WriteLine($"XKM: {FalseNorth}");
            sr.WriteLine($"N: {N}");
            sr.WriteLine("#角度的单位，默认为 D.MMSS");
            sr.WriteLine("#ANGLE : DEGREE D.MMSSS RADIAN");
            sr.WriteLine("ANGLE: D.MMSSS");

            sr.WriteLine("#点名, N(m), E(m), B(D.MMSS), L(D.MMSS), H(m), M, γ(D.MMSS), X(m), Y(m), Z(m)");
            foreach (var pnt in PointList)
            {
                sr.WriteLine(pnt);
            }
            sr.Close();
        }
    }
}