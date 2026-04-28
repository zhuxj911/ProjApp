using ZXY;

namespace UnitTestExpandEllipsoid;

public class UnitTestExpandEllipsoid
{
    [Fact]
    public void Test_CGCS2000_ExpandEllipsoid()
    {

        var ellipsoid = EllipsoidFactory.Ellipsoids[EllipsoidType.CGCS2000];

        //【示例1】北京天安门附近的大地坐标
        var B = SurMath.DegreeToRadians(39.907500); // 039:54:27
        var L = SurMath.DegreeToRadians(116.39720); // 116:23:49.92
        var H = 45.0;

        //验证思想： 点的XYZ坐标不变，在扩展后的椭球面上计算点新的BLH坐标， 新H应该为0
        var (X, Y, Z) = ellipsoid.BLHtoXYZ(B, L, H);

        var newEllipsoid = ellipsoid.ExpandEllipsoid(B, H);
        var (newB, newL, newH) = newEllipsoid.XYZtoBLH(X, Y, Z);

        var dB = SurMath.RadiansToDegree(newB - B); //newB 0.696517294607325     B 0.696517271239637
        var dL = SurMath.RadiansToDegree(newL - L); //newL 2.0315143801023439    L 2.0315143801023439   

        Assert.Equal(0, newH, 1e-4); //-1.5832483768463135E-08
        Assert.Equal(1.3388699061300527E-06, dB, 1e-15); // 1.3388698997689443E-06
        Assert.Equal(0, dL, 1e-15);
        Assert.Equal(6378182.0621230202, newEllipsoid.a, 1e-15);
        Assert.Equal(45.06212302017957, newEllipsoid.a - ellipsoid.a, 1e-15);
    }

    [Fact]
    public void Test_Beijing1954_ExpandEllipsoid()
    {
        var ellipsoid = EllipsoidFactory.Ellipsoids[EllipsoidType.Beijing1954];

        //【示例1】北京天安门附近的大地坐标
        var B = SurMath.DegreeToRadians(39.907500); // 039:54:27
        var L = SurMath.DegreeToRadians(116.39720); // 116:23:49.92
        var H = 45.0;

        //验证思想： 点的XYZ坐标不变，在扩展后的椭球面上计算点新的BLH坐标， 新H应该为0
        var (X, Y, Z) = ellipsoid.BLHtoXYZ(B, L, H);

        var newEllipsoid = ellipsoid.ExpandEllipsoid(B, H);
        var (newB, newL, newH) = newEllipsoid.XYZtoBLH(X, Y, Z);

        IProj proj = new GaussProj(newEllipsoid);
        var L0 = SurMath.DegreeToRadians(117.0);
        var (newN, newE, _, _) = proj.Forward(newB, newL, L0, 500.0);

        //TODO：怎么验证 笑脸的这个值是否正确呢？
        Assert.Equal(4419541.729802, newN, 1e-3); //Actual:4419541.9027905371 ,笑脸 4419541.729802  
        Assert.Equal(448453.796176, newE, 1e-3);  //Actual:448453.79693855019 ,笑脸 448453.796176
    }
}