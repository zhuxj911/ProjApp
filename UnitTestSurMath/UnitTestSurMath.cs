using Moq;
using ZXY;

namespace UnitTestSurMath;

public class UnitTestSurMath
{
    [Fact]
    public void TestDmsToDms()
    {
        var dms = SurMath.DmsToDms(123.4052);
        Assert.Equal<int>(123, dms.d);
        Assert.Equal<int>(40, dms.m);
        Assert.Equal(52, dms.s, 1e-8);

        dms = SurMath.DmsToDms(-123.4052);
        Assert.Equal<int>(-123, dms.d);
        Assert.Equal<int>(-40, dms.m);
        Assert.Equal(-52, dms.s, 1e-8);

        dms = SurMath.DmsToDms(101.02201);
        Assert.Equal(101, dms.d);
        Assert.Equal(2, dms.m);
        Assert.Equal(20.1, dms.s, 1e-6);

        dms = SurMath.DmsToDms(-101.02201);
        Assert.Equal(-101, dms.d);
        Assert.Equal(-2, dms.m);
        Assert.Equal(-20.1, dms.s, 1e-6);


        dms = SurMath.DmsToDms(1.4000);
        Assert.Equal<int>(1, dms.d);
        Assert.Equal<int>(40, dms.m);
        Assert.Equal(0, dms.s, 1e-8);

        dms = SurMath.DmsToDms(-1.4000);
        Assert.Equal<int>(-1, dms.d);
        Assert.Equal<int>(-40, dms.m);
        Assert.Equal(0, dms.s, 1e-8);

        dms = SurMath.DmsToDms(1.40001);
        Assert.Equal(1, dms.d);
        Assert.Equal(40, dms.m);
        Assert.Equal(0.1, dms.s, 1e-6);

        dms = SurMath.DmsToDms(-1.40001);
        Assert.Equal(-1, dms.d);
        Assert.Equal(-40, dms.m);
        Assert.Equal(-0.1, dms.s, 1e-6);
    }


    [Fact]
    public void TestDmsToRadian()
    {
        var r = SurMath.DmsToRadian(101.02201);
        Assert.Equal(1.76346176848151, r, 1e-14);

        r = SurMath.DmsToRadian(-101.02201);
        Assert.Equal(-1.76346176848151, r, 1e-14);

        r = SurMath.DmsToRadian(1.4000);
        Assert.Equal(0.0290888208665722, r, 1e-14);

        r = SurMath.DmsToRadian(-1.4000);
        Assert.Equal(-0.0290888208665722, r, 1e-14);

        r = SurMath.DmsToRadian(1.40001);
        Assert.Equal(0.02908930568025330, r, 1e-14);

        r = SurMath.DmsToRadian(-1.40001);
        Assert.Equal(-0.02908930568025330, r, 1e-14);
    }


    [Fact]
    public void TestDmsToDmsString()
    {
        var r = SurMath.DmsToDmsString(101.02201);
        Assert.Equal("101°02′20.1″", r);

        r = SurMath.DmsToDmsString(-101.02201);
        Assert.Equal("-101°02′20.1″", r);

        r = SurMath.DmsToDmsString(1.40001);
        Assert.Equal("1°40′00.1″", r);

        r = SurMath.DmsToDmsString(-1.40001);
        Assert.Equal("-1°40′00.1″", r);

        r = SurMath.DmsToDmsString(1.4000);
        Assert.Equal("1°40′00″", r);

        r = SurMath.DmsToDmsString(-1.4000);
        Assert.Equal("-1°40′00″", r);
    }


    [Fact]
    public void TestRadian2Dms()
    {
        var dms = SurMath.Radian2Dms(0.410855952558766);
        Assert.Equal<int>(23, dms.d);
        Assert.Equal<int>(32, dms.m);
        Assert.Equal(25.12345, dms.s, 1e-8);


        dms = SurMath.Radian2Dms(-0.410855952558766);
        Assert.Equal<int>(-23, dms.d);
        Assert.Equal<int>(-32, dms.m);
        Assert.Equal(-25.12345, dms.s, 1e-8);


        dms = SurMath.Radian2Dms(1.76346176848151);
        Assert.Equal(101, dms.d);
        Assert.Equal(2, dms.m);
        Assert.Equal(20.1, dms.s, 1e-6);

        dms = SurMath.Radian2Dms(-1.76346176848151);
        Assert.Equal(-101, dms.d);
        Assert.Equal(-2, dms.m);
        Assert.Equal(-20.1, dms.s, 1e-6);

        dms = SurMath.Radian2Dms(0.0290888208665722);
        Assert.Equal(1, dms.d);
        Assert.Equal(40, dms.m);
        Assert.Equal(0, dms.s, 1e-6);

        dms = SurMath.Radian2Dms(-0.0290888208665722);
        Assert.Equal(-1, dms.d);
        Assert.Equal(-40, dms.m);
        Assert.Equal(0, dms.s, 1e-6);

        dms = SurMath.Radian2Dms(0.0290893056802533);
        Assert.Equal(1, dms.d);
        Assert.Equal(40, dms.m);
        Assert.Equal(0.1, dms.s, 1e-6);

        dms = SurMath.Radian2Dms(-0.0290893056802533);
        Assert.Equal(-1, dms.d);
        Assert.Equal(-40, dms.m);
        Assert.Equal(-0.1, dms.s, 1e-6);
    }


    [Fact]
    public void TestRadianToDms()
    {
        var dms = SurMath.RadianToDms(1.76346176848151);
        Assert.Equal(101.02201, dms, 1e-10);

        dms = SurMath.RadianToDms(-1.76346176848151);
        Assert.Equal(-101.02201, dms, 1e-10);

        dms = SurMath.RadianToDms(0.0290888208665722);
        Assert.Equal(1.4000, dms, 1e-10);

        dms = SurMath.RadianToDms(-0.0290888208665722);
        Assert.Equal(-1.4000, dms, 1e-10);

        dms = SurMath.RadianToDms(0.0290893056802533);
        Assert.Equal(1.40001, dms, 1e-10);

        dms = SurMath.RadianToDms(-0.0290893056802533);
        Assert.Equal(-1.40001, dms, 1e-10);
    }


    [Fact]
    public void TestDmsToRadianToDms()
    {
        var r = SurMath.DmsToRadian(101.02201);
        var dms = SurMath.RadianToDms(r);
        Assert.Equal(101.02201, dms, 1e-14);

        r = SurMath.DmsToRadian(-101.02201);
        dms = SurMath.RadianToDms(r);
        Assert.Equal(-101.02201, dms, 1e-14);

        r = SurMath.DmsToRadian(1.4000);
        dms = SurMath.RadianToDms(r);
        Assert.Equal(1.4000, dms, 1e-14);

        r = SurMath.DmsToRadian(-1.4000);
        dms = SurMath.RadianToDms(r);
        Assert.Equal(-1.4000, dms, 1e-14);

        r = SurMath.DmsToRadian(1.40001);
        dms = SurMath.RadianToDms(r);
        Assert.Equal(1.40001, dms, 1e-14);

        r = SurMath.DmsToRadian(-1.40001);
        dms = SurMath.RadianToDms(r);
        Assert.Equal(-1.40001, dms, 1e-14);
    }


    [Fact]
    public void TestAzimuth()
    {
        var ad = SurMath.Azimuth(12.234, 23.1234, 13.2323, 24.232332);
        Assert.Equal(0.837851107, ad.a, 1e-9);

        ad = SurMath.Azimuth(12.234, 23.1234, 3.2323, 24.232332);
        Assert.Equal(3.019018832, ad.a, 1e-9);

        ad = SurMath.Azimuth(12.234, 23.1234, 3.2323, 4.232332);
        Assert.Equal(4.267712913, ad.a, 1e-9);

        ad = SurMath.Azimuth(12.234, 23.1234, 13.2323, 4.232332);
        Assert.Equal(4.765184951, ad.a, 1e-9);


        double xA = 50342.464;
        double yA = 3423.232;
        double xB = 50289.874;
        double yB = 3528.978;

        ad = SurMath.Azimuth(xA, yA, xB, yB);
        Assert.Equal(2.03230066539732, ad.a, 1e-10);
        Assert.Equal(118.101331982326, ad.d, 1e-10);

        ad = SurMath.Azimuth(xB, yB, xA, yA);
        Assert.Equal(2.03230066539732 + Math.PI, ad.a, 1e-10);
        Assert.Equal(118.101331982326, ad.d, 1e-10);
    }

    [Fact]
    public void TestTo0_2PI()
    {
        var r = SurMath.To0_2PI(SurMath.DmsToRadian(500.0));
        Assert.Equal(140.0, SurMath.RadianToDms(r), 1e-10);

        r = SurMath.To0_2PI(SurMath.DmsToRadian(-500.0));
        Assert.Equal(220.0, SurMath.RadianToDms(r), 1e-10);
    }

    [Fact]
    public void TestPointAzimuth()
    {
        //var A = new Point(50342.464, 3423.232);
        //var B = new Point(50289.874, 3528.978);
        //var ad = A.Azimuth(B);
        //Assert.Equal(2.03230066539732, ad.a, 1e-10);
        //Assert.Equal(118.101331982326, ad.d, 1e-10);

        //ad = B.Azimuth(A);
        //Assert.Equal(2.03230066539732 + Math.PI, ad.a, 1e-10);
        //Assert.Equal(118.101331982326, ad.d, 1e-10);

        //引入Moq针对接口IPoint模拟测试
        //dotnet add package Moq --version 4.20.72
        // https://github.com/devlooped/moq
        /*
            public interface IUserInfo
            {
                string UserName { get; set; }
                int Age { get; set; }

                string GetUserData();
            }

            public static void UserInfoTest()
            {
                // 创建 IUserInfo 的模拟对象
                var mockUserInfo = new Mock<IUserInfo>();

                // 设置模拟对象的属性值
                mockUserInfo.SetupProperty(u => u.UserName, "大姚");
                mockUserInfo.SetupProperty(u => u.Age, 27);

                // 设置 GetUserData 方法的返回值
                mockUserInfo.Setup(u => u.GetUserData()).Returns("UserName: 大姚, Age: 25");

                // 获取模拟对象的实例
                var userInfo = mockUserInfo.Object;

                // 调用方法并输出结果
                Console.WriteLine(userInfo.GetUserData());
                Console.WriteLine("UserName: {0}, Age: {1}", userInfo.UserName, userInfo.Age);
            }
        */
        var A = new Mock<IPoint>();
        A.SetupProperty(p => p.X, 50342.464);
        A.SetupProperty(p => p.Y, 3423.232);
        var B = new Mock<IPoint>();
        B.SetupProperty(p => p.X, 50289.874);
        B.SetupProperty(p => p.Y, 3528.978);

        //获取模拟对象的实例 A.Object B.Object
        var ad = A.Object.Azimuth(B.Object);
        Assert.Equal(2.03230066539732, ad.a, 1e-10);
        Assert.Equal(118.101331982326, ad.d, 1e-10);

        ad = B.Object.Azimuth(A.Object);
        Assert.Equal(2.03230066539732 + Math.PI, ad.a, 1e-10);
        Assert.Equal(118.101331982326, ad.d, 1e-10);
    }
}