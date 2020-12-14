using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ProjApp.Model;

namespace ProjApp.ViewModel
{
  public class ProjViewModel : ViewModelBase
  {
    public ProjViewModel()
    {
      GaussProj = new GaussProjModel();
    }

    private GaussProjModel gaussProj;
    public GaussProjModel GaussProj
    {
      get => gaussProj;
      set
      {
        gaussProj = value;
        RaisePropertyChanged(() => GaussProj);
      }
    }

    #region Commands
    private RelayCommand newFileCommand;
    public RelayCommand NewFileCommand
    {
      get
      {
        if (newFileCommand == null)
          return new RelayCommand(() => GaussProj.New());
        return newFileCommand;
      }
      set { newFileCommand = value; }
    }

    private RelayCommand openFileCommand;
    public RelayCommand OpenFileCommand
    {
      get
      {
        if (openFileCommand == null)
          return new RelayCommand(() => GaussProj.Open());
        return openFileCommand;
      }
      set { openFileCommand = value; }
    }

    private RelayCommand saveFileCommand;
    public RelayCommand SaveFileCommand
    {
      get
      {
        if (saveFileCommand == null)
          return new RelayCommand(() => GaussProj.Save());
        return saveFileCommand;
      }
      set { saveFileCommand = value; }
    }
    private RelayCommand saveAsFileCommand;
    public RelayCommand SaveAsFileCommand
    {
      get
      {
        if (saveFileCommand == null)
          return new RelayCommand(() => GaussProj.SaveAs());
        return saveFileCommand;
      }
      set { saveFileCommand = value; }
    }

    private RelayCommand clearBLCommand;
    public RelayCommand ClearBLCommand
    {
      get
      {
        if (clearBLCommand == null)
          return new RelayCommand(() => GaussProj.ClearBL());
        return clearBLCommand;
      }
      set { clearBLCommand = value; }
    }

    private RelayCommand clearXYCommand;
    public RelayCommand ClearXYCommand
    {
      get
      {
        if (clearXYCommand == null)
          return new RelayCommand(() => GaussProj.ClearXY());
        return clearXYCommand;
      }
      set { clearXYCommand = value; }
    }

    private RelayCommand onBLtoXYCommand;
    public RelayCommand OnBLtoXYCommand
    {
      get
      {
        if (onBLtoXYCommand == null)
          return new RelayCommand(() => GaussProj.BLtoXY());
        return onBLtoXYCommand;
      }
      set { onBLtoXYCommand = value; }
    }


    private RelayCommand onXYtoBLCommand;
    public RelayCommand OnXYtoBLCommand
    {
      get
      {
        if (onXYtoBLCommand == null)
          return new RelayCommand(() => GaussProj.XYtoBL());
        return onXYtoBLCommand;
      }
      set { onXYtoBLCommand = value; }
    }


    private RelayCommand showAzimuthWindowCommand;
    /// <summary>
    /// 显示坐标方位角计算窗体
    /// </summary>
    public RelayCommand ShowAzimuthWindowCommand
    {
      get
      {
        if (showAzimuthWindowCommand == null)
          showAzimuthWindowCommand = new RelayCommand(() => ShowAzimuthWindow());
        return showAzimuthWindowCommand;
      }
      set { showAzimuthWindowCommand = value; }
    }

    private void ShowAzimuthWindow()
    {
      AzimuthWin dlg = new AzimuthWin();
      dlg.ShowDialog();
    }
    #endregion
  }
}
