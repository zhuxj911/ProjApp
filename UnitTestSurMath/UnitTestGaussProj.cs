using ZXY;

namespace UnitTestGaussProj;

public class UnitTestGaussProj
{
    [Fact]
    public void Test_CGCS2000_GaussProj_Forward()
    {
        /*
         * Geographic Coordinate Systems (GCS) in China:
         * EPSG:4214   GCS_Beijing_1954
         * EPSG:4326   GCS_WGS_1984
         * EPSG:4490   GCS_China_Geodetic_Coordinate_System_2000
         * EPSG:4555   GCS_New_Beijing
         * EPSG:4610   GCS_Xian_1980
         * 
         * Projected Coordinate Systems (PCS) in China:
         * ESPG:4513 - ESPG:4533   CGCS2000 / 3-degree Gauss-Kruger zone 25~45             75°-135° 加带号
         * ESPG:4534 - ESPG:4554   CGCS2000 / 3-degree Gauss-Kruger CM 75E~135E            75°-135° 不加带号
         * 
         * ESPG:2401 - ESPG:2421   Beijing_1954_3_Degree_GK_Zone_25~75          Gauss-Kruger 75°-135° 加带号
         * ESPG:2422 - ESPG:2442   Beijing_1954_3_Degree_GK_CM_75E~135E         Gauss-Kruger 25-45 75°-135° 不加带号
         * 
         * ESPG:2349 - ESPG:2369   Xian_1980_3_Degree_GK_Zone_25~45  Gauss-Kruger 25-45 75°-135° 加带号
         * ESPG:2370 - ESPG:2490   Xian_1980_3_Degree_GK_CM_75E~135E  Gauss-Kruger 25-45 75°-135° 不加带号
         */
        //https://epsg.io/transform#s_srs=4490&t_srs=4527&x=116.3972000&y=39.9075000
        //Input coordinate system     EPSG:4490 China Geodetic Coordinate System 2000
        //Input coordinates  Longitude/Latitude  116°23'49.92" E 39°54'27" N
        //Output coordinate system    EPSG:4527 CGCS2000 / 3-degree Gauss-Kruger zone 39  
        //Unit: metre
        //Area of use: China - onshore between 115°30'E and 118°30'E.
        //E 39448455.02271326  N 4419432.367677833

        //【测试1】北京天安门 39.9075°, 116.3972°
        var lat = SurMath.DmsToRadians(39.5427);
        var lon = SurMath.DmsToRadians(116.234992);
        var lon0 = SurMath.DmsToRadians(117);

        var ellipsoid = EllipsoidType.Ellipsoids["CGCS2000"];
        IProj proj = new GaussProj(ellipsoid);

        var (N, E, gamma, m) = proj.Forward(lat, lon, lon0, 500, 39);
        Assert.Equal(4419432.367677833, N, 1e-3);
        Assert.Equal(39448455.02271326, E, 1e-3);
        Assert.Equal("-0°23'12.245514\"", SurMath.RadiansToDmsString(gamma));
        Assert.Equal(0.99963268166217156, m, 1e-3);
    }

    [Fact]
    public void Test_CGCS2000_GaussProj_Inverse()
    {
        //https://epsg.io/transform#s_srs=4490&t_srs=4527&x=116.3972000&y=39.9075000
        //Input coordinate system     EPSG:4490 China Geodetic Coordinate System 2000
        //Input coordinates  Longitude/Latitude  116°23'49.92" E 39°54'27" N
        //Output coordinate system    EPSG:4527 CGCS2000 / 3-degree Gauss-Kruger zone 39  
        //Unit: metre
        //Area of use: China - onshore between 115°30'E and 118°30'E.
        //E 39448455.02271326  N 4419432.367677833

        //var lat = SurMath.DmsToRadian(39.5427);
        //var lon = SurMath.DmsToRadian(116.234992);
        var N = 4419432.367677833;
        var E = 39448455.02271326;
        var lon0 = SurMath.DmsToRadians(117);

        var ellipsoid = EllipsoidType.Ellipsoids["CGCS2000"];
        IProj proj = new GaussProj(ellipsoid);

        var (lat, lon, gamma, m) = proj.Inverse(N, E, lon0, 500, 39, 0);
        var tlat = SurMath.RadiansToDms(lat);
        var tlon = SurMath.RadiansToDms(lon);
        Assert.Equal(39.5427, tlat, 1e-4);
        Assert.Equal(116.234992, tlon, 1e-6);
        Assert.Equal("-0°23'12.245513\"", SurMath.RadiansToDmsString(gamma));
        Assert.Equal(0.99963268166217156, m, 1e-3);
    }


    [Fact]
    public void Test_Beijing1954_GaussProj_Forward()
    {
        var ellipsoid = EllipsoidType.Ellipsoids["Beijing1954"];
        IProj proj = new GaussProj(ellipsoid);


        //B = 21 ◦ 58 ′ 47.0845 ′′ , L = 113 ◦ 25 ′ 31.4880 ′′ ，L0 = 111 ◦  
        //x = 2433586.692,y = 250547.403
        var B = SurMath.DmsToRadians(21.58470845);
        var l = SurMath.DmsToRadians(2.25314880);

        var (x, y, _, _) = proj.Forward(B, l, 0, 0, 0, 0);
        Assert.Equal(2433586.692, x, 1e-3);
        Assert.Equal(250547.403, y, 1e-3);

        // 经笑脸坐标转换程序验证，B=034:22:20.04343 L=109:13:34.00626
        // 经https://epsg.io/transform#s_srs=4214&t_srs=2412&x=109.2261129&y=34.3722343 验证
        // epsg验证时，由于秒值只保留到0.001秒，可能精度不够， 在输入时可输入 109°13'34.00626"   34°22'20.04343"
        //                                                            109.22611285°  34.3722342861111°
        //                           epsg显示为：109.2261129°  34.3722343° 或 109°13'34.006" 34°22'20.043"
        //Input coordinate system：EPSG:4214 Beijing 1954
        //Output coordinate system：EPSG:2412 Beijing 1954 / 3-degree Gauss-Kruger zone 36  Area of use: China - onshore between 106°30'E and 109°30'E.
        /* 
         * pyproj 验证：
          
         from pyproj import Transformer
         lat, lon = 34+22.0/60.0 + 20.04343/3600.0, 109+13.0/60.0 + 34.00626/3600.0 # 34°22'20.04343"   109°13'34.00626"  => 448475.64070452086  4417664.594844512
         # Input coordinate system：EPSG:4214 Beijing 1954
         # Output coordinate system：EPSG:2412 Beijing 1954 / 3-degree Gauss-Kruger zone 36
         # Area of use: China - onshore between 106°30'E and 109°30'E.
         trans = Transformer.from_crs("EPSG:4214", "EPSG:2412", always_xy=True)
         e, n = trans.transform(lon, lat)

         print(f"EPSG:2412 Beijing 1954 / 3-degree Gauss-Kruger zone 36:  E={e:.10f}m, N={n:.10f}m") 
         
        输出结果为：
        EPSG:2412 Beijing 1954 / 3-degree Gauss-Kruger zone 36:  E=36612782.7330614328m, N=3805700.6570984675m
         */


        B = SurMath.DmsToRadians(34.222004343);
        var L = SurMath.DmsToRadians(109.133400626);
        var L0 = SurMath.DmsToRadians(108);
        (x, y, _, _) = proj.Forward(B, L, L0, 500, 36, 0);
        Assert.Equal(3805700.6570984675, x, 1e-6); //3805700.6570983259
        Assert.Equal(36612782.73306143, y, 1e-6);  //36612782.733061552
    }

    [Fact]
    public void Test_Beijing1954_GaussProj_Forward1()
    {
        //B = 21 ◦ 58 ′ 47.0845 ′′ , L = 113 ◦ 25 ′ 31.4880 ′′ ，L0 = 111 ◦ 
        //x = 2433586.692,y = 37750547.403
        var B = SurMath.DmsToRadians(21.58470845);
        var L = SurMath.DmsToRadians(113.25314880);
        var L0 = SurMath.DmsToRadians(111);

        var ellipsoid = EllipsoidType.Ellipsoids["Beijing1954"];
        GaussProj proj = new GaussProj(ellipsoid);
       
        var (x, y, _, _) = proj.Forward(B, L, L0, 500, 37);
        Assert.Equal(2433586.692, x, 1e-3);
        Assert.Equal(37750547.403, y, 1e-3);
    }


    [Fact]
    public void Test_Beijing1954_GaussProj_Inverse1()
    {
        //B = 21 ◦ 58 ′ 47.0845 ′′ , L = 113 ◦ 25 ′ 31.4880 ′′ ，L0 = 111 ◦ 
        //x = 2433586.692,y = 250547.403
        double x = 2433586.692, y = 250547.403;

        //var ellipsoid = new Ellipsoid(6378245, 298.3);
        var ellipsoid = EllipsoidType.Ellipsoids["Beijing1954"];
        GaussProj proj = new GaussProj(ellipsoid);
        //UtmProj proj = new UtmProj(ellipsoid);

        var (B, l, _, _) = proj.Inverse(x, y, 0, 0, 0, 0);

        Assert.Equal(21.58470845, SurMath.RadiansToDms(B), 1e-8);
        Assert.Equal(2.25314880, SurMath.RadiansToDms(l), 1e-8);
    }

    [Fact]
    public void Test_Beijing1954_GaussProj_Inverse2()
    {
        //B = 21 ◦ 58 ′ 47.0845 ′′ , L = 113 ◦ 25 ′ 31.4880 ′′ ，L0 = 111 ◦ 
        //x = 2433586.692,y = 250547.403
        double X = 2433586.692, Y = 37750547.403;
        var L0 = SurMath.DmsToRadians(111);

        //var ellipsoid = new Ellipsoid(6378245, 298.3);
        var ellipsoid = EllipsoidType.Ellipsoids["Beijing1954"];
        GaussProj proj = new GaussProj(ellipsoid);
        //UtmProj proj = new UtmProj(ellipsoid);

        var (B, L, _, _) = proj.Inverse(X, Y, L0, 500.0, 37, 0);

        Assert.Equal(21.58470845, SurMath.RadiansToDms(B), 1e-8);
        Assert.Equal(113.25314880, SurMath.RadiansToDms(L), 1e-8);
    }
}