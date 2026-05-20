using ZXY;

namespace UnitTestExpandEllipsoid;

public class UnitTestExpandEllipsoid
{
    [Fact]
    public void Test_CGCS2000_ExpandEllipsoid()
    {

        var ellipsoid = EllipsoidType.Ellipsoids["CGCS2000"];

        //【示例1】北京天安门附近的大地坐标
        var B = SurMath.DegreeToRadians(39.907500); // 039:54:27
        var L = SurMath.DegreeToRadians(116.39720); // 116:23:49.92
        var H = 45.0;

        //验证思想： 点的XYZ坐标不变，在扩展后的椭球面上计算点新的BLH坐标， 新H应该为0
        var (X, Y, Z) = ellipsoid.BLHtoXYZ(B, L, H);

        var newEllipsoid = ellipsoid.ExpandEllipsoidByGeodeticDiff(B, H);
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
        var ellipsoid = EllipsoidType.Ellipsoids["Beijing1954"];

        //【示例1】北京天安门附近的大地坐标
        var B = SurMath.DegreeToRadians(39.907500); // 039:54:27
        var L = SurMath.DegreeToRadians(116.39720); // 116:23:49.92
        var H = 45.0;

        //验证思想： 点的XYZ坐标不变，在扩展后的椭球面上计算点新的BLH坐标， 新H应该为0
        var (X, Y, Z) = ellipsoid.BLHtoXYZ(B, L, H);

        //var newEllipsoid = ellipsoid.ExpandEllipsoid(B, H);//广义微分公式
        var newEllipsoid = ellipsoid.ExpandEllipsoidByAverageR(B, H);//平均曲率半径法
        //var newEllipsoid = ellipsoid.ExpandEllipsoid3(B, H);//卯酉曲率半径法
        var (newB, newL, newH) = newEllipsoid.XYZtoBLH(X, Y, Z);

        IProj proj = new GaussProj(newEllipsoid);
        var L0 = SurMath.DegreeToRadians(117.0);
        //var (newN, newE, _, _) = proj.Forward(newB, newL, L0, 500.0);
        var (newN, newE, _, _) = proj.Forward(B, newL, L0, 500.0);

        //TODO：怎么验证 笑脸的这个值是否正确呢？
        //笑脸坐标转换程序使用的 平均曲率半径法 膨胀椭球， 椭球膨胀后的 纬度B 值 用的原有的值，没加改变量 ΔB
        //newN = 4419541.7298019473  newE = 448453.79621923371
        Assert.Equal(4419541.729802, newN, 1e-3); //Actual:4419541.9027905371 ,笑脸 4419541.729802  笑脸转换用的平均曲率半径法
        Assert.Equal(448453.796176, newE, 1e-3);  //Actual:448453.79693855019 ,笑脸 448453.796176
    }

    ////该处的测试是针对一篇论文测试计算的，论文中的示例数据不知道怎么出来的
    //[Fact]
    //public void Test_Beijing1954_ExpandEllipsoid_delta_a()
    //{
    //    var ellipsoid = EllipsoidFactory.Ellipsoids[EllipsoidType.Beijing1954];

    //    var B = SurMath.DegreeToRadians(45.0);

    //    var newEllipsoid = ellipsoid.ExpandEllipsoid(B, 1000.0);
    //    var da1 = newEllipsoid.a - ellipsoid.a;

    //    newEllipsoid = ellipsoid.ExpandEllipsoid(B, 2000.0);
    //    var da2 = newEllipsoid.a - ellipsoid.a;

    //    newEllipsoid = ellipsoid.ExpandEllipsoid(B, 3000.0);
    //    var da3 = newEllipsoid.a - ellipsoid.a;

    //    newEllipsoid = ellipsoid.ExpandEllipsoid(B, 4000.0);
    //    var da4 = newEllipsoid.a - ellipsoid.a;

    //    newEllipsoid = ellipsoid.ExpandEllipsoid(B, 5000.0);
    //    var da5 = newEllipsoid.a - ellipsoid.a;
    //}

    [Fact]
    public void Test_CGCS2000_ExpandEllipsoid_delta_a()
    {
        var ellipsoid = EllipsoidType.Ellipsoids["CGCS2000"];

        var B = SurMath.DegreeToRadians(32.0);

        var  newEllipsoid = ellipsoid.ExpandEllipsoidByAverageR(B, 3850.0); //平均曲率半径法
        var da2 = newEllipsoid.a - ellipsoid.a;

        newEllipsoid = ellipsoid.ExpandEllipsoidByPrimeVerticalCurvatureRadius(B, 3850.0); //卯酉曲率半径法
        var da3 = newEllipsoid.a - ellipsoid.a;

        newEllipsoid = ellipsoid.ExpandEllipsoidByPlaneAnalysis(B, 3850.0); //平面解析法
        var da4 = newEllipsoid.a - ellipsoid.a;

       newEllipsoid = ellipsoid.ExpandEllipsoidByGeodeticDiff(B, 3850.0); //广义微分公式
        var da1 = newEllipsoid.a - ellipsoid.a;
    }
}