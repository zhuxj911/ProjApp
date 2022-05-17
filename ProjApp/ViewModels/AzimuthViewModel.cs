using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using ProjApp.Model;
using System.Windows.Input;

namespace ProjApp.ViewModels;

public class AzimuthViewModel : ObservableObject
{
    public AzimuthViewModel()
    {
        CalculateCommand = new RelayCommand(this.CalculateAzimuth);
    }

    private GPoint _ptA = new GPoint();

    public GPoint PtA { get => _ptA; }

    private GPoint _ptB = new GPoint();

    public GPoint PtB { get => _ptB; }

    private string _azName = "坐标方位角";

    public string AzName
    {
        get => _azName;
        set => SetProperty(ref _azName, value);
    }

    private string _az;
    public string Az
    {
        get => _az;
        set => SetProperty(ref _az, value);
    }

    private double _dist;

    public double Dist
    {
        get => _dist;
        set => SetProperty(ref _dist, value);
    }

    public ICommand CalculateCommand { get; }
    public void CalculateAzimuth()
    {
        Az = ZXY.SurMath.RADtoDMSString(
            ZXY.SurMath.Azimuth(PtA.X, PtA.Y, PtB.X, PtB.Y));
        Dist = ZXY.SurMath.Distance(PtA.X, PtA.Y, PtB.X, PtB.Y);
        AzName = string.Format($"{PtA.Name}->{PtB.Name}坐标方位角");
    }
}

