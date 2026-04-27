using ZXY;

namespace UnitTestXYZtoBLH;

public class UnitTestXYZtoBLH
{
    [Fact]
    public void Test_CGCS2000_BLHtoXYZ()
    {
        var ellipsoid = EllipsoidFactory.Ellipsoids[EllipsoidType.CGCS2000];
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
    public void Test_CGCS2000_XYZtoBLH()
    {
        var ellipsoid = EllipsoidFactory.Ellipsoids[EllipsoidType.CGCS2000];

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