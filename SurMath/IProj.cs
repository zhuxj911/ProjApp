namespace ZXY;

public interface IProj
{
   void Bltoxy(double B, double l,
      out double x, out double y,
      out double gamma, out double m);

   /// <summary>
   /// 根据经纬度计算XY坐标（Y带加常数）
   /// </summary>
   /// <param name="B">纬度，单位：弧度</param>
   /// <param name="L">经度，单位：弧度</param>
   /// <param name="L0">中央子午线经度，单位：弧度</param>
   /// <param name="YKM">Y坐标加常数，单位：km， 一般为500km</param>
   /// <param name="N0">Y坐标前的带号</param>
   /// <param name="X">North坐标，单位：m</param>
   /// <param name="Y">East坐标，单位：m</param>
   void BLtoXYKM(double B, double L, double L0,
      double YKM, double N0,
      out double X, out double Y,
      out double gamma, out double m);


   void BLtoXYKM(double B, double L, double L0,
      double YKM, double N0,
      out double X,
      out double Y);


   void xytoBl(double x, double y,
      out double B, out double l,
      out double gamma, out double m);

   void XYKMtoBL(double X, double Y, double L0,
      double YKM, double N0,
      out double B, out double L,
      out double gamma, out double m);

   void XYKMtoBL(double X, double Y, double L0,
      double YKM, double N0,
      out double B,
      out double L);
}