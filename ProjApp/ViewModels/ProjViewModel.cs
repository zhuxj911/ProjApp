using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Win32;
using ProjApp.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using ZXY;

namespace ProjApp.ViewModels;

public class ProjViewModel : ObservableObject
{   
    public ProjViewModel()
    {
        NewFileCommand = new RelayCommand(this.NewFile);
        OpenFileCommand = new RelayCommand(this.OpenFile);
        SaveFileCommand = new RelayCommand(this.SaveFile);
        SaveAsFileCommand = new RelayCommand(this.SaveAsFile);
        ClearBLCommand = new RelayCommand(this.ClearBL);
        ClearXYCommand= new RelayCommand(this.ClearXY);
        BLtoXYCommand = new RelayCommand(this.BLtoXY);
        XYtoBLCommand = new RelayCommand(this.XYtoBL);
        ShowAzimuthWindowCommand = new RelayCommand(this.ShowAzimuthWindow);
    }

    public List<Spheroid> SpheroidList
    {
        get => SpheroidFactory.SpheroidList;
    }

    private Spheroid currentSpheroid = SpheroidFactory.SpheroidList[0];
    public Spheroid CurrentSpheroid
    {
        get => currentSpheroid;
        set => SetProperty(ref currentSpheroid, value);
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
        get => ZXY.SurMath.DMStoRAD(dmsL0);
        set => dmsL0 = ZXY.SurMath.RADtoDMS(value);
    }

    private int _N = 0;
    public int N
    {
        get => _N;
        set => SetProperty(ref _N, value);
    }

    private double _ykm = 0;
    public double YKM
    {
        get => _ykm;
        set => SetProperty(ref _ykm, value);
    }

    private string _angleFormat = "D.MMSS";
    public string AngleFormat
    {
        get => _angleFormat;
        set => SetProperty(ref _angleFormat, value);
    }

    private ObservableCollection<GPoint> pointList = new ObservableCollection<GPoint>();
    public ObservableCollection<GPoint> PointList => pointList;
    public Dictionary<string, Spheroid> Spheroids  => SpheroidFactory.Spheroids; 
    public bool IsValidated() => PointList.Count > 0;

    private string fileName = "untitle";
    public string FileName
    {
        get => fileName;
        set => SetProperty(ref fileName, value);
    }

    public string Title  => $"测量螺丝刀(Ver2020)-{FileName}"; 

    public ICommand BLtoXYCommand { get; }
    public void BLtoXY()
    {
        IProj proj = new GaussProj(CurrentSpheroid);
        foreach (var pnt in this.PointList)
        {
            proj.BLtoXYKM(pnt.B, pnt.L, L0, YKM, N,
                out double x, out double y,
                out double gamma, out double m);
            pnt.X = x;
            pnt.Y = y;
            pnt.Gamma = ZXY.SurMath.RADtoDMSString(gamma);
            pnt.m = m;
        }
    }

    public ICommand XYtoBLCommand { get; }
    public void XYtoBL()
    {
        IProj proj = new GaussProj(CurrentSpheroid);
        foreach (var pnt in this.PointList)
        {
            proj.XYKMtoBL(pnt.X, pnt.Y, L0, YKM, N,
                out double B, out double L, out double gamma, out double m);
            pnt.B = B; 
            pnt.L = L;
            pnt.Gamma = ZXY.SurMath.RADtoDMSString(gamma);
            pnt.m = m;
        }
    }

    public ICommand ClearBLCommand { get; }
    public void ClearBL()
    {
        foreach (var pnt in this.PointList)
        {
            pnt.B = pnt.L = 0;
        }
    }

    public ICommand ClearXYCommand { get; }
    public void ClearXY()
    {
        foreach (var pnt in this.PointList)
        {
            pnt.X = pnt.Y = 0;
        }
    }


    public ICommand NewFileCommand { get; }
    public void NewFile()
    {
        CurrentSpheroid = Spheroids["CGCS2000"];
        dmsL0 = 0;
        YKM = 0;
        N = 0;
        PointList.Clear();
    }

    public ICommand OpenFileCommand { get; }
    public void OpenFile()
    {
        OpenFileDialog dlg = new OpenFileDialog();
        dlg.DefaultExt = ".txt";
        dlg.Filter = "高斯投影坐标数据|*.txt|All File(*.*)|*.*";
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
                    items = buffer.Split(new char[1] { ':' });

                    string itemName = items[0].Trim();
                    switch (itemName)
                    {
                        case "CS":
                            {
                                string item2 = items[1].Trim();
                                if (item2 == "BJ54")
                                    CurrentSpheroid = Spheroids["BJ54"];
                                else if (item2 == "XA80")
                                    CurrentSpheroid = Spheroids["XA80"];
                                else if (item2 == "WGS84")
                                    CurrentSpheroid = Spheroids["WGS84"];
                                else if (item2 == "CGCS2000")
                                    CurrentSpheroid = Spheroids["CGCS2000"];
                                else
                                {
                                    string[] its = item2.Split(new char[1] { ',' });
                                    if (its.Length == 3 && its[0] == "CS00")
                                    {
                                        CurrentSpheroid = Spheroids["CS00"];
                                        CurrentSpheroid.a = double.Parse(its[1]);
                                        CurrentSpheroid.f = double.Parse(its[2]);
                                    }
                                }
                            }
                            break;
                        case "L0":
                            if (AngleFormat == "DEGREE") //单位为度
                                L0 = SurMath.DegreetoRAD(double.Parse(items[1]));
                            else if (AngleFormat == "RADIAN")//单位为弧度
                                L0 = double.Parse(items[1]);
                            else //默认为 D.MMSS
                                dmsL0 = double.Parse(items[1]);
                            break;
                        case "YKM":
                            YKM = double.Parse(items[1]);
                            break;
                        case "N":
                            N = int.Parse(items[1]);
                            break;
                        case "ANGLE":
                            AngleFormat = items[1].Trim();
                            break;
                        default:
                            break;
                    }
                    continue; //处理完毕继续
                }

                items = buffer.Split(new char[1] { ',' });
                if (items.Length < 3) continue; //少于三项数据，不是点的坐标数据，忽略
                GPoint pnt = new GPoint();
                pnt.Name = items[0].Trim();
                pnt.X = double.Parse(items[1]);
                pnt.Y = double.Parse(items[2]);

                if (items.Length >= 5)
                {
                    if (AngleFormat == "DEGREE") //单位为度
                    {
                        pnt.B = SurMath.DegreetoRAD(double.Parse(items[3]));
                        pnt.L = SurMath.DegreetoRAD(double.Parse(items[4]));
                    }
                    else if (AngleFormat == "RADIAN")//单位为弧度
                    {
                        pnt.B = double.Parse(items[3]);
                        pnt.L = double.Parse(items[4]);
                    }
                    else //默认为 D.MMSS
                    {
                        pnt.dmsB = double.Parse(items[3]);
                        pnt.dmsL = double.Parse(items[4]);
                    }
                }
                this.PointList.Add(pnt);
            }
        }
    }

    public ICommand SaveFileCommand { get; }
    public void SaveFile()
    {
        if (FileName == "untitle")
            SaveAsFile();
        else
            WriteFile();
    }

    public ICommand SaveAsFileCommand { get; }
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
            sr.WriteLine("#CS 指定坐标系 BJ54 XA80 CGCS2000 WGS84 CS00");
            sr.WriteLine("#CS: BJ54");
            sr.WriteLine("#CS: XA80");
            sr.WriteLine("#CS: WGS84");
            sr.WriteLine("#CS: CGCS2000");
            sr.WriteLine("#CS: CS00, 6378137, 298.257222101");
            if (CurrentSpheroid.Id == "CS00")
            {
                sr.WriteLine($"CS: {CurrentSpheroid.Id}, {CurrentSpheroid.a}, {CurrentSpheroid.f}");
            }
            else
            {
                sr.WriteLine($"CS: {CurrentSpheroid.Id}");
            }
            sr.WriteLine("#角度数据格式为D.MMSS");
            sr.WriteLine($"L0: {dmsL0}");
            sr.WriteLine($"YKM: {YKM}");
            sr.WriteLine($"N: {N}");
            sr.WriteLine("#角度的单位，默认为 D.MMSS");
            sr.WriteLine("#ANGLE : DEGREE D.MMSSS RADIAN");
            sr.WriteLine("ANGLE: D.MMSSS");


            sr.WriteLine("#点名, B, L, X, Y, 子午线收敛角(γ),长度比(m)");
            foreach (var pnt in PointList)
            {
                sr.WriteLine(pnt);
            }
            sr.Close();
        }
    }

    public ICommand ShowAzimuthWindowCommand { get; }
    /// <summary>
    /// 显示坐标方位角计算窗体
    /// </summary>
    public void ShowAzimuthWindow()
    {
        AzimuthWin dlg = new AzimuthWin();
        dlg.ShowDialog();
    }
}