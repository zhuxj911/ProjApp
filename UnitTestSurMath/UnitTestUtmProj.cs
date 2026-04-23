using ZXY;

namespace UnitTestSurMath;

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
       
        var lat = SurMath.DmsToRadian(39.5427);
        var lon = SurMath.DmsToRadian(116.234992);
        var lon0 = SurMath.DmsToRadian(117);

        var ellipsoid = EllipsoidFactory.Ellipsoids[EllipsoidType.WGS1984];
        var proj = new UtmProj(ellipsoid);

        var (x, y, gamma, m) = proj.Forward(lat, lon, lon0, 500);
        Assert.Equal(4417664.594844512, x, 1e-3);
        Assert.Equal(448475.64070452086, y, 1e-3);
        Assert.Equal("-0°23′12.245514″", SurMath.RadianToDmsString(gamma));
        Assert.Equal(0.99963268166217156, m, 1e-3);

       
    }

    [Fact]
    public void Test_WGS84_UtmProj_Inverse()
    {
        //lat = 39°54'27"  39.9075° , lon = 116°23'49.92" 116.3972° ，L0 = 117°  测试点：北京天安门
        //E=448475.64070452086 N=4417664.594844512

        //var lat = SurMath.DmsToRadian(39.5427);
        //var lon = SurMath.DmsToRadian(116.234992);

        var N = 4417664.594844512;
        var E = 448475.64070452086;
        var lon0 = SurMath.DmsToRadian(117);

        var ellipsoid = EllipsoidFactory.Ellipsoids[ EllipsoidType.WGS1984];
        var proj = new UtmProj(ellipsoid);

        var (lat, lon, gamma, m) = proj.Inverse(N, E, lon0, 500);
        var tlat = SurMath.RadianToDms(lat);
        var tlon = SurMath.RadianToDms(lon);
        Assert.Equal(39.5427, tlat, 1e-4);
        Assert.Equal(116.234992,tlon, 1e-6);
        Assert.Equal("-0°23′12.245513″", SurMath.RadianToDmsString(gamma));
        Assert.Equal(0.99963268166217156, m, 1e-3);
    }
}
