using ZXY;

namespace UnitTestXYZtoBLH;

public class UnitTestXYZtoBLH
{
    [Fact]
    public void Test_CGCS2000_BLHtoXYZ()
    {
        var ellipsoid = EllipsoidType.Ellipsoids["CGCS2000"];
        {
            //【示例1】北京天安门附近的大地坐标
            var B = SurMath.DegreeToRadians(39.907500);
            var L = SurMath.DegreeToRadians(116.39720);
            var H = 45.0;

            var (X, Y, Z) = ellipsoid.BLHtoXYZ(B, L, H);

            Assert.Equal(-2178203.388578, X, 1e-6);
            Assert.Equal(4388500.751834, Y, 1e-6);
            Assert.Equal(4070141.363799, Z, 1e-6);
        }

        {
            //【示例2】赤道 + 赤道上空点
            var B = SurMath.DegreeToRadians(0);
            var L = SurMath.DegreeToRadians(100);
            var H = 10000;

            var (X, Y, Z) = ellipsoid.BLHtoXYZ(B, L, H);

            Assert.Equal(-1109288.3487366911, X, 1e-6);
            Assert.Equal(6291086.8449041471, Y, 1e-6);
            Assert.Equal(0, Z, 1e-6);
        }

        {
            //【示例3】北极点附近(B = 89°, L = 120°, H = 2000m)
            var B = SurMath.DegreeToRadians(89);
            var L = SurMath.DegreeToRadians(120);
            var H = 2000;

            var (X, Y, Z) = ellipsoid.BLHtoXYZ(B, L, H);

            Assert.Equal(-55861.549585225606, X, 1e-6);
            Assert.Equal(96755.042071138945, Y, 1e-6);
            Assert.Equal(6357777.3219249602, Z, 1e-6);
        }
    }

    [Fact]
    public void Test_Beijing1954_BLHtoXYZ()
    {
        var ellipsoid = EllipsoidType.Ellipsoids["Beijing1954"];
        {
            var B = SurMath.DmsToRadians(34.222004343);
            var L = SurMath.DmsToRadians(109.133400626);
            var H = 0.0;

            var (X, Y, Z) = ellipsoid.BLHtoXYZ(B, L, H);

            //验证数据由笑脸坐标转换4.2版提供
            Assert.Equal(-1735443.505443, X, 1e-5); //-1735443.5054467432
            Assert.Equal(4976211.355080, Y, 1e-5);  //4976211.3550843112
            Assert.Equal(3580666.123764, Z, 1e-4);  //3580666.123755361

            var (tB, tL, tH) = ellipsoid.XYZtoBLH(X, Y, Z);
            Assert.Equal(34.222004343, SurMath.RadiansToDms(tB), 1e-9); 
            Assert.Equal(109.133400626, SurMath.RadiansToDms(tL), 1e-9);
            Assert.Equal(0.0, tH, 1e-7); //-2.60770320892334E-08
        }
    }


    [Fact]
    public void Test_CGCS2000_XYZtoBLH()
    {
        var ellipsoid = EllipsoidType.Ellipsoids["CGCS2000"];

        {
            //【示例1】北京天安门附近的大地坐标
            var X = -2178203.388578;
            var Y = 4388500.751834;
            var Z = 4070141.363799;

            var (B, L, H) = ellipsoid.XYZtoBLH(X, Y, Z);

            Assert.Equal(39.907500, SurMath.RadiansToDegree(B), 1e-6);
            Assert.Equal(116.39720, SurMath.RadiansToDegree(L), 1e-6);
            Assert.Equal(45.0, H, 1e-6);
        }

        {
            //【示例2】赤道 + 赤道上空点
            var X = -1109288.3487366911;
            var Y = 6291086.8449041471;
            var Z = 0;

            var (B, L, H) = ellipsoid.XYZtoBLH(X, Y, Z);

            Assert.Equal(0, SurMath.RadiansToDegree(B), 1e-6);
            Assert.Equal(100, SurMath.RadiansToDegree(L), 1e-6);
            Assert.Equal(10000, H, 1e-6);
        }

        {
            //【示例3】北极点附近(B = 89°, L = 120°, H = 2000m)
            var X = -55861.549585225606;
            var Y = 96755.042071138945;
            var Z = 6357777.3219249602;

            var (B, L, H) = ellipsoid.XYZtoBLH(X, Y, Z);

            Assert.Equal(89, SurMath.RadiansToDegree(B), 1e-6);
            Assert.Equal(120, SurMath.RadiansToDegree(L), 1e-6);
            Assert.Equal(2000, H, 1e-6);
        }
    }
}