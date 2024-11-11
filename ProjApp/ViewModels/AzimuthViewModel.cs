using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProjApp.Models;
using System;

namespace ProjApp.ViewModels;

public partial class AzimuthViewModel : ObservableObject
{
    public AzimuthViewModel()
    {
        A = new();
        B = new();
#if DEBUG
        A = new()
        {
            Name = "GP11",
            X = 50342.464,
            Y = 3528.978
        };

        B = new()
        {
            Name = "GP12",
            X = 50289.874,
            Y = 3423.232
        };
#endif
        A.PropertyChanged += OnPointPropertyChanged;
        B.PropertyChanged += OnPointPropertyChanged;
    }

    private void OnPointPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        //OnPropertyChanged(nameof(CanCalculate));
        CalculateCommand.NotifyCanExecuteChanged();
    }

    /**
     * 修改 A B 两点的 X Y 值，通知只读属性 CanCalculate 控制计算按钮 CalculateCommand 是否可用 ：
     *   public bool CanCalculate => Math.Abs(A.X - B.X) >= 0.0001 || Math.Abs(A.Y - B.Y) >= 0.0001;
     * 解决方法有：
     *  方法1. 在 构造函数中加入 A B 属性发生变化的Event：
     *        A.PropertyChanged += OnPointPropertyChanged;
              B.PropertyChanged += OnPointPropertyChanged;
     *    
     *     在 OnPointPropertyChanged 中通知 CanCalculate 属性发生改变：
     *        OnPropertyChanged(nameof(CanCalculate));
     *   
     *     在 xaml 界面代码中，绑定 IsEnabled 属性
     *       <Button
     *           Grid.Column="3"
     *           Command="{Binding Path=CalculateCommand}"
     *           IsEnabled="{Binding CanCalculate}"
     *           Content="计算" />
     *   从而解决问题
     *   
     *   完整代码如下：
     *   <Button
                    Grid.Column="3"
                    Command="{Binding Path=CalculateCommand}"
                    IsEnabled="{Binding CanCalculate}"
                    Content="计算" />

                    A.PropertyChanged += OnPointPropertyChanged;
                    B.PropertyChanged += OnPointPropertyChanged;
     
                    private void OnPointPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
                    {
                        OnPropertyChanged(nameof(CanCalculate));
                    }

                    [RelayCommand]
                    private void Calculate()
                    {
                        var ad = ZXY.SurMath.Azimuth(A.X, A.Y, B.X, B.Y);
                        AzValue = ZXY.SurMath.RadianToDmsString(ad.a);
                        Dist = ad.d;
                        AzName = $"{A.Name} -> {B.Name} 坐标方位角";
                    }
     * 方法2. 在 构造函数中加入 A B 属性发生变化的Event：
     *        A.PropertyChanged += OnPointPropertyChanged;
     *                 B.PropertyChanged += OnPointPropertyChanged;
     *    
     *     在 OnPointPropertyChanged 中调用命令 NotifyCanExecuteChanged()， 计算 CanExecute ， 从而改变命令的状态：
     *        CalculateCommand.NotifyCanExecuteChanged();
     *        
     *     命令的定义为：
     *      [RelayCommand(CanExecute = nameof(CanCalculate))]
            private void Calculate()
            {
                var ad = ZXY.SurMath.Azimuth(A.X, A.Y, B.X, B.Y);
                AzValue = ZXY.SurMath.RadianToDmsString(ad.a);
                Dist = ad.d;
                AzName = $"{A.Name} -> {B.Name} 坐标方位角";
            }

          在 xaml中无需绑定属性 IsEnabled ： 
     *      <Button
     *           Grid.Column="3"
                 Command="{Binding Path=CalculateCommand}"                    
                 Content="计算" />
     *     
     */


    [ObservableProperty]
    private GPoint _A;


    [ObservableProperty]
    private GPoint _B;


    [ObservableProperty] //使用源生成器生成 AzName 属性
    private string azName;


    [ObservableProperty] //使用源生成器生成 AzValue 属性
    private string azValue;


    [ObservableProperty] //使用源生成器生成 Dist 属性
    private double dist;


    [RelayCommand]
    private void SwitchAB()
    {
        (B, A) = (A, B);
    }

    /// <summary>
    /// 控制计算按钮是否可用
    /// </summary>  
    public bool CanCalculate => Math.Abs(A.X - B.X) >= 0.0001 || Math.Abs(A.Y - B.Y) >= 0.0001;



    //[RelayCommand]
    //private void Calculate()
    //{
    //    var ad = ZXY.SurMath.Azimuth(A.X, A.Y, B.X, B.Y);
    //    AzValue = ZXY.SurMath.RadianToDmsString(ad.a);
    //    Dist = ad.d;
    //    AzName = $"{A.Name} -> {B.Name} 坐标方位角";
    //}


    [RelayCommand(CanExecute = nameof(CanCalculate))]
    private void Calculate()
    {
        var ad = ZXY.SurMath.Azimuth(A.X, A.Y, B.X, B.Y);
        AzValue = ZXY.SurMath.RadianToDmsString(ad.a);
        Dist = ad.d;
        AzName = $"{A.Name} -> {B.Name} 坐标方位角";
    }
}