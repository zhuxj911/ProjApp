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

        var ellipsoid = EllipsoidFactory.Ellipsoids[EllipsoidType.CGCS2000];
        IProj proj = new GaussProj(ellipsoid);

        var (N, E, gamma, m) = proj.Forward(lat, lon, lon0, 0, 500, 39);
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

        var ellipsoid = EllipsoidFactory.Ellipsoids[EllipsoidType.CGCS2000];
        IProj proj = new GaussProj(ellipsoid);

        var (lat, lon, gamma, m) = proj.Inverse(N, E, lon0, 0, 500, 39);
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
        //B = 21 ◦ 58 ′ 47.0845 ′′ , L = 113 ◦ 25 ′ 31.4880 ′′ ，L0 = 111 ◦  
        //x = 2433586.692,y = 250547.403
        var B = SurMath.DmsToRadians(21.58470845);
        var l = SurMath.DmsToRadians(2.25314880);

        var ellipsoid = EllipsoidFactory.Ellipsoids[EllipsoidType.Beijing1954];
        IProj proj = new GaussProj(ellipsoid);
        
        var (x, y, _, _) = proj.Forward(B, l, 0, 0, 0, 0);
        Assert.Equal(2433586.692, x, 1e-3);
        Assert.Equal(250547.403, y, 1e-3);
    }

    [Fact]
    public void Test_Beijing1954_GaussProj_Forward1()
    {
        //B = 21 ◦ 58 ′ 47.0845 ′′ , L = 113 ◦ 25 ′ 31.4880 ′′ ，L0 = 111 ◦ 
        //x = 2433586.692,y = 37750547.403
        var B = SurMath.DmsToRadians(21.58470845);
        var L = SurMath.DmsToRadians(113.25314880);
        var L0 = SurMath.DmsToRadians(111);

        var ellipsoid = EllipsoidFactory.Ellipsoids[EllipsoidType.Beijing1954];
        GaussProj proj = new GaussProj(ellipsoid);
       
        var (x, y, _, _) = proj.Forward(B, L, L0, 0, 500, 37);
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
        var ellipsoid = EllipsoidFactory.Ellipsoids[EllipsoidType.Beijing1954];
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
        var ellipsoid = EllipsoidFactory.Ellipsoids[EllipsoidType.Beijing1954];
        GaussProj proj = new GaussProj(ellipsoid);
        //UtmProj proj = new UtmProj(ellipsoid);

        var (B, L, _, _) = proj.Inverse(X, Y, L0, 0, 500.0, 37);

        Assert.Equal(21.58470845, SurMath.RadiansToDms(B), 1e-8);
        Assert.Equal(113.25314880, SurMath.RadiansToDms(L), 1e-8);
    }
}