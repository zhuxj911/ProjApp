using ZXY;

namespace UnitTestUtmProj;

public class UnitTestUtmProj
{
    [Fact]
    public void Test_WGS84_UtmProj_Forward()
    {
        //lat = 39°54'27"  39.9075° , lon = 116°23'49.92" 116.3972° ，L0 = 117°  测试点：北京天安门
        //E=448475.64070452086 N=4417664.594844512
        //验证数据来自 https://epsg.io/transform#s_srs=4326&t_srs=32650&x=116.3972000&y=39.9075000
        //Input coordinate system   EPSG:4326 WGS 84 Longitude/Latitude  116°23'49.92" E 39°54'27" N
        //Output coordinate system  EPSG:32650 WGS 84 / UTM zone 50N   448475.64070452086 E 4417664.594844512 N
        //Unit: metre
        //Area of use: Between 114°E and 120°E, northern

        //测试点1: 北京天安门 (纬度: 39.9075°N, 经度: 116.3972°E)
        //var lat = SurMath.DmsToRadian(39.5427);
        //var lon = SurMath.DmsToRadian(116.234992);
        //var lon0 = SurMath.DmsToRadian(117);
        var lat = SurMath.DegreeToRadians(39.9075);
        var lon = SurMath.DegreeToRadians(116.3972);
        var lon0 = SurMath.DegreeToRadians(117);

        var ellipsoid = EllipsoidType.Ellipsoids["WGS1984"];
        var proj = new UtmProj(ellipsoid);
        var (n, e, gamma, m) = proj.Forward(lat, lon, lon0, 500);
        Assert.Equal(4417664.594844512, n, 1e-3);
        Assert.Equal(448475.64070452086, e, 1e-3);
        Assert.Equal("-0°23'12.245514\"", SurMath.RadiansToDmsString(gamma));
        Assert.Equal(0.99963268166217156, m, 1e-15);

        //测试点2: 上海东方明珠 (纬度: 31.2397°N, 经度: 121.5017°E) 
        //zone = [(λ + 180)/6] + 1   -> zone = [(121.5017 + 180)/6] + 1 = 51
        //lon0 = 6*zone - 183 -> lon0 = 6*51 - 183 = 123
        //EPSG:4326 WGS 84      EPSG:32651 WGS 84 / UTM zone 51N    Area of use: Between 120°E and 126°E, northern
        //Input coordinate system   EPSG:4326 WGS 84 Longitude/Latitude  121°30'6.12" E 31°14'22.92" N
        //Output coordinate system  EPSG:32651 WGS 84 / UTM zone 51N   357314.66519028944 E 3457134.8978081113 N

        lat = SurMath.DegreeToRadians(31.2397);  //31°14'22.92"
        lon = SurMath.DegreeToRadians(121.5017); //121°30'6.12"
        lon0 = SurMath.DegreeToRadians(123);

        (n, e, gamma, m) = proj.Forward(lat, lon, lon0, 500);
        Assert.Equal(3457134.8978081113, n, 1e-3);
        Assert.Equal(357314.66519028944, e, 1e-3);
        Assert.Equal("-0°46'37.84482\"", SurMath.RadiansToDmsString(gamma));
        Assert.Equal(0.99985112151247002, m, 1e-15);


        //测试点3: 南半球 (澳大利亚悉尼: 33.8688°S, 151.2093°E)
        //zone = [(λ + 180)/6] + 1   -> zone = [(151.2093 + 180)/6] + 1 = 56
        //lon0 = 6*zone - 183 -> lon0 = 6*56 - 183 = 153
        //EPSG:4326 WGS 84      EPSG:32656 WGS 84 / UTM zone 56N    Area of use: Between 150°E and 156°E, northern
        //Input coordinate system   EPSG:4326 WGS 84 Longitude/Latitude  151°12'33.48" E 33°52'7.68" S
        //Output coordinate system  EPSG:32656 WGS 84 / UTM zone 56N   334368.633648097 E -3749051.6546149906 N

        lat = SurMath.DegreeToRadians(-33.8688);  //-33°52'7.68" 南半球纬度为负
        lon = SurMath.DegreeToRadians(151.2093); //151°12'33.48"
        lon0 = SurMath.DegreeToRadians(153);
        //EPSG:4326 WGS 84      EPSG:32656 WGS 84 / UTM zone 56N    Area of use: Between 150°E and 156°E, northern
        //Input coordinate system   EPSG:4326 WGS 84 Longitude/Latitude  151°12'33.48" E 33°52'7.68" S
        //Output coordinate system  EPSG:32656 WGS 84 / UTM zone 56N   334368.633648097 E -3749051.6546149906 N

        (n, e, gamma, m) = proj.Forward(lat, lon, lon0, 500, 0, 10000);
        Assert.Equal(-3749051.6546149906 + 10000*1e3, n, 1e-3);
        Assert.Equal(334368.633648097, e, 1e-3);
        Assert.Equal("0°59'53.418677\"", SurMath.RadiansToDmsString(gamma));
        Assert.Equal(0.99993820041542703, m, 1e-15);

        //测试点4: 赤道附近 (印度尼西亚: 0.0°S, 108.0°E) 
        //zone = [(λ + 180)/6] + 1   -> zone = [(108.0 + 180)/6] + 1 = 49
        //lon0 = 6*zone - 183 -> lon0 = 6*49 - 183 = 111
        //EPSG:4326 WGS 84      EPSG:32649 WGS 84 / UTM zone 49N    Area of use: Between 108°E and 114°E, northern
        //Input coordinate system   EPSG:4326 WGS 84 Longitude/Latitude  108°0'0" E 0°0'0" N
        //Output coordinate system  EPSG:32649 WGS 84 / UTM zone 49N   166021.44308054337 E 0 N
        lat = SurMath.DegreeToRadians(0.0);
        lon = SurMath.DegreeToRadians(108.0);
        lon0 = SurMath.DegreeToRadians(111);

        (n, e, gamma, m) = proj.Forward(lat, lon, lon0, 500);
        Assert.Equal(0, n, 1e-3);
        Assert.Equal(166021.44308054337, e, 1e-3);
        Assert.Equal("0°00'00\"", SurMath.RadiansToDmsString(gamma));
        Assert.Equal(1.0009810299736448, m, 1e-15);


        //测试点5: 香港 (纬度: 22.5431°N, 经度: 114.0579°E) 
        //zone = [(λ + 180)/6] + 1   -> zone = [(114.0579 + 180)/6] + 1 = 50
        //lon0 = 6*zone - 183 -> lon0 = 6*50 - 183 = 117
        //EPSG:4326 WGS 84      EPSG:32650 WGS 84 / UTM zone 50N    Area of use: Between 114°E and 120°E, northern
        //Input coordinate system   EPSG:4326 WGS 84 Longitude/Latitude  121°30'6.12" E 31°14'22.92" N
        //Output coordinate system  EPSG:32650 WGS 84 / UTM zone 50N    197389.33118408715 E 2495922.941117888 N

        lat = SurMath.DegreeToRadians(22.5431); //22°32'35.16"
        lon = SurMath.DegreeToRadians(114.0579);//114°3'28.44"
        lon0 = SurMath.DegreeToRadians(117);

        (n, e, gamma, m) = proj.Forward(lat, lon, lon0, 500);
        Assert.Equal(2495922.941117888, n, 1e-3);
        Assert.Equal(197389.33118408715, e, 1e-3);
        Assert.Equal("-1°07'43.673837\"", SurMath.RadiansToDmsString(gamma));
        Assert.Equal(1.0007315244664023, m, 1e-15);

        //测试点6: 西安 (纬度: 34.3416°N, 经度: 108.9398°E) 
        //zone = [(λ + 180)/6] + 1   -> zone = [(108.9398 + 180)/6] + 1 = 49
        //lon0 = 6*zone - 183 -> lon0 = 6*49 - 183 = 111
        //EPSG:4326 WGS 84      EPSG:32649 WGS 84 / UTM zone 49N     Area of use: Between 108°E and 114°E, northern
        //Input coordinate system   EPSG:4326 WGS 84 Longitude/Latitude  108°56'23.28" E 34°20'29.76" N
        //Output coordinate system  EPSG:32649 WGS 84 / UTM zone 49N    310494.9844291871 E 3801955.457332173 N

        lat = SurMath.DegreeToRadians(34.3416); //34°20'29.76"
        lon = SurMath.DegreeToRadians(108.9398); //108°56'23.28"
        lon0 = SurMath.DegreeToRadians(111);

        (n, e, gamma, m) = proj.Forward(lat, lon, lon0, 500);
        Assert.Equal(3801955.457332173, n, 1e-3);
        Assert.Equal(310494.9844291871, e, 1e-3);
        Assert.Equal("-1°09'45.208995\"", SurMath.RadiansToDmsString(gamma));
        Assert.Equal(1.0000426832017848, m, 1e-15);
    }

    [Fact]
    public void Test_WGS84_UtmProj_Inverse()
    {
        var ellipsoid = EllipsoidType.Ellipsoids["WGS1984"];
        var proj = new UtmProj(ellipsoid);

        //测试点1: 北京天安门 (纬度: 39.9075°N, 经度: 116.3972°E)
        //lat = 39°54'27"  39.9075° , lon = 116°23'49.92" 116.3972° ，L0 = 117°  测试点：北京天安门
        //E=448475.64070452086 N=4417664.594844512

        //var lat = SurMath.DmsToRadian(39.5427);
        //var lon = SurMath.DmsToRadian(116.234992);

        var n = 4417664.594844512;
        var e = 448475.64070452086;
        var lon0 = SurMath.DegreeToRadians(117);

        var (lat, lon, gamma, m) = proj.Inverse(n, e, lon0, 500, 0, 0);
        var tlat = SurMath.RadiansToDms(lat);
        var tlon = SurMath.RadiansToDms(lon);
        Assert.Equal(39.5427, tlat, 1e-4);
        Assert.Equal(116.234992, tlon, 1e-6);
        Assert.Equal("-0°23'12.245513\"", SurMath.RadiansToDmsString(gamma));
        Assert.Equal(0.99963268166217156, m, 1e-8); //Actual:   0.99963268166721742

        //测试点2: 上海东方明珠 (纬度: 31.2397°N, 经度: 121.5017°E) 
        //zone = [(λ + 180)/6] + 1   -> zone = [(121.5017 + 180)/6] + 1 = 51
        //lon0 = 6*zone - 183 -> lon0 = 6*51 - 183 = 123
        //EPSG:4326 WGS 84      EPSG:32651 WGS 84 / UTM zone 51N    Area of use: Between 120°E and 126°E, northern
        //Input coordinate system   EPSG:4326 WGS 84 Longitude/Latitude  121°30'6.12" E 31°14'22.92" N
        //Output coordinate system  EPSG:32651 WGS 84 / UTM zone 51N   357314.66519028944 E 3457134.8978081113 N

        //lat = SurMath.DegreeToRadian(31.2397);  //31°14.382'
        //lon = SurMath.DegreeToRadian(121.5017); //121°30.102'
        //lon0 = SurMath.DegreeToRadian(123);

        n = 3457134.8978081113;
        e = 357314.66519028944;
        lon0 = SurMath.DegreeToRadians(123);

        (lat, lon, gamma, m) = proj.Inverse(n, e, lon0, 500, 0, 0);
        tlat = SurMath.RadiansToDegree(lat);
        tlon = SurMath.RadiansToDegree(lon);
        Assert.Equal(31.2397, tlat, 1e-4);
        Assert.Equal(121.5017, tlon, 1e-4);
        Assert.Equal("-0°46'37.8448\"", SurMath.RadiansToDmsString(gamma));
        Assert.Equal(0.99985112151247002, m, 1e-8); //Actual:   0.99985112195241965

        //测试点3: 南半球 (澳大利亚悉尼: 33.8688°S, 151.2093°E)
        //zone = [(λ + 180)/6] + 1   -> zone = [(151.2093 + 180)/6] + 1 = 56
        //lon0 = 6*zone - 183 -> lon0 = 6*56 - 183 = 153
        //EPSG:4326 WGS 84      EPSG:32656 WGS 84 / UTM zone 56N    Area of use: Between 150°E and 156°E, northern
        //Input coordinate system   EPSG:4326 WGS 84 Longitude/Latitude  151°12'33.48" E 33°52'7.68" S
        //Output coordinate system  EPSG:32656 WGS 84 / UTM zone 56N   334368.633648097 E -3749051.6546149906 N
        n = -3749051.6546149906 + 10000*1e3;
        e = 334368.633648097;
        lon0 = SurMath.DegreeToRadians(153);

        (lat, lon, gamma, m) = proj.Inverse(n, e, lon0, 500, 0, 10000);
        tlat = SurMath.RadiansToDegree(lat);
        tlon = SurMath.RadiansToDegree(lon);
        Assert.Equal(-33.8688, tlat, 1e-4);
        Assert.Equal(151.2093, tlon, 1e-4);
        Assert.Equal("0°59'53.418645\"", SurMath.RadiansToDmsString(gamma));
        Assert.Equal(0.99993820041542703, m, 1e-8);// Actual:   0.99993820113541232

        //测试点4: 赤道附近 (印度尼西亚: 0.0°S, 108.0°E) 
        //zone = [(λ + 180)/6] + 1   -> zone = [(108.0 + 180)/6] + 1 = 49
        //lon0 = 6*zone - 183 -> lon0 = 6*49 - 183 = 111
        //EPSG:4326 WGS 84      EPSG:32649 WGS 84 / UTM zone 49N    Area of use: Between 108°E and 114°E, northern
        //Input coordinate system   EPSG:4326 WGS 84 Longitude/Latitude  108°0'0" E 0°0'0" N
        //Output coordinate system  EPSG:32649 WGS 84 / UTM zone 49N   166021.44308054337 E 0 N
        n = 0;
        e = 166021.44308054337;
        lon0 = SurMath.DegreeToRadians(111);

        (lat, lon, gamma, m) = proj.Inverse(n, e, lon0, 500, 0, 0);
        tlat = SurMath.RadiansToDegree(lat);
        tlon = SurMath.RadiansToDegree(lon);
        Assert.Equal(0.0, tlat, 1e-4);
        Assert.Equal(108.0, tlon, 1e-4);
        Assert.Equal("0°00'00\"", SurMath.RadiansToDmsString(gamma));
        Assert.Equal(1.0009810299736448, m, 1e-7); //Actual:   1.0009810529005898

        //测试点5: 香港 (纬度: 22.5431°N, 经度: 114.0579°E) 
        //zone = [(λ + 180)/6] + 1   -> zone = [(114.0579 + 180)/6] + 1 = 50
        //lon0 = 6*zone - 183 -> lon0 = 6*50 - 183 = 117
        //EPSG:4326 WGS 84      EPSG:32650 WGS 84 / UTM zone 50N    Area of use: Between 114°E and 120°E, northern
        //Input coordinate system   EPSG:4326 WGS 84 Longitude/Latitude  121°30'6.12" E 31°14'22.92" N
        //Output coordinate system  EPSG:32650 WGS 84 / UTM zone 50N    197389.33118408715 E 2495922.941117888 N
        n = 2495922.941117888;
        e = 197389.33118408715;
        lon0 = SurMath.DegreeToRadians(117);

        (lat, lon, gamma, m) = proj.Inverse(n, e, lon0, 500, 0, 0);
        tlat = SurMath.RadiansToDegree(lat);
        tlon = SurMath.RadiansToDegree(lon);
        Assert.Equal(22.5431, tlat, 1e-4);
        Assert.Equal(114.0579, tlon, 1e-4);
        Assert.Equal("-1°07'43.673724\"", SurMath.RadiansToDmsString(gamma));
        Assert.Equal(1.0007315244664023, m, 1e-7); //Actual:   1.0007315363425171

        //测试点6: 西安 (纬度: 34.3416°N, 经度: 108.9398°E) 
        //zone = [(λ + 180)/6] + 1   -> zone = [(108.9398 + 180)/6] + 1 = 49
        //lon0 = 6*zone - 183 -> lon0 = 6*49 - 183 = 111
        //EPSG:4326 WGS 84      EPSG:32649 WGS 84 / UTM zone 49N     Area of use: Between 108°E and 114°E, northern
        //Input coordinate system   EPSG:4326 WGS 84 Longitude/Latitude  108°56'23.28" E 34°20'29.76" N
        //Output coordinate system  EPSG:32649 WGS 84 / UTM zone 49N    310494.9844291871 E 3801955.457332173 N
        n = 3801955.457332173;
        e = 310494.9844291871;
        lon0 = SurMath.DegreeToRadians(111);

        (lat, lon, gamma, m) = proj.Inverse(n, e, lon0, 500, 0, 0);
        tlat = SurMath.RadiansToDegree(lat);
        tlon = SurMath.RadiansToDegree(lon);
        Assert.Equal(34.3416, tlat, 1e-4);
        Assert.Equal(108.9398, tlon, 1e-4);
        Assert.Equal("-1°09'45.20895\"", SurMath.RadiansToDmsString(gamma));
        Assert.Equal(1.0000426832017848, m, 1e-8); //Actual:   1.0000426844089625
    }

    [Fact]
    public void Test_WGS84_Utm_GetZoneNumber()
    {
        var ellipsoid = EllipsoidType.Ellipsoids["WGS1984"];
        var proj = new UtmProj(ellipsoid);

        var lat = 39.9075; // 39°54'27"
        var lon = 116.3972; // 116°23'49.92"

        var zone = proj.GetZoneNumber(lon, lat);
        Assert.Equal(50, zone);
    }


    [Fact]
    public void Test_WGS84_Utm_GetCentralMeridian()
    {
        var ellipsoid = EllipsoidType.Ellipsoids["WGS1984"];
        var proj = new UtmProj(ellipsoid);

        var lon0 = proj.GetCentralMeridian(50);
        Assert.Equal(117, lon0);
    }
}
