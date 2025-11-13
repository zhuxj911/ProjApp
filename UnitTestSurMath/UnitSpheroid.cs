using ZXY;

namespace UnitTestSurMath
{
    public class UnitSpheroid
    {
        [Fact]
        public void TestBltoxy()
        {
            {
                //Spheroid BJ = new Spheroid(6378245, 298.3);

                IProj proj = new GaussProj(EllipsoidFactory.Ellipsoids["BJ54"]);

                double B = SurMath.DmsToRadian(21.58470845);
                double l = SurMath.DmsToRadian(2.25314880);
                proj.Bltoxy(B, l, out double x, out double y, out _, out _);
                //B = 21 ◦ 58 ′ 47.0845 ′′ ,L = 113 ◦ 25 ′ 31.4880 ′′ ，
                //x = 2433586.692,y = 250547.403
                Assert.Equal(2433586.692, x, 1e-3);
                Assert.Equal(250547.403, y, 1e-3);
            }

            {
                //B = 21 ◦ 58 ′ 47.0845 ′′ ,L = 113 ◦ 25 ′ 31.4880 ′′ ，
                //x = 2433586.692,y = 250547.403            
                IProj proj = new GaussProj(EllipsoidFactory.Ellipsoids["BJ54"]);

                double B = SurMath.DmsToRadian(21.58470845);
                double L = SurMath.DmsToRadian(113.25314880);
                double L0 = SurMath.DmsToRadian(111);
                proj.BLtoXYKM(B, L, L0, 0, 0, out double x, out double y);

                Assert.Equal(2433586.692, x, 1e-3);
                Assert.Equal(250547.403, y, 1e-3);
            }
        }

        [Fact]
        public void TestxytoBl()
        {
            {
                //B = 21 ◦ 58 ′ 47.0845 ′′ ,L = 113 ◦ 25 ′ 31.4880 ′′ ，

                IProj proj = new GaussProj(EllipsoidFactory.Ellipsoids["BJ54"]);

                double x = 2433586.692, y = 250547.403;
                proj.xytoBl(x, y, out double B, out double l, out _, out _);

                B = SurMath.RadianToDms(B);
                l = SurMath.RadianToDms(l);

                Assert.Equal(21.58470845, B, 1e-8);
                Assert.Equal(2.25314880, l, 1e-8);
            }

        }
    }
}
