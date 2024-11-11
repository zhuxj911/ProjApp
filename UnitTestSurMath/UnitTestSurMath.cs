using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ZXY;

namespace UnitTestSurMath
{
    [TestClass]
    public class UnitTestSurMath
    {
        [TestMethod]
        public void TestDMStoDMS()
        {
            var dms = SurMath.DmsToDms(123.4052);
            Assert.AreEqual<int>(123, dms.d);
            Assert.AreEqual<int>(40, dms.m);
            Assert.AreEqual(52, dms.s, 1e-8);

            dms = SurMath.DmsToDms(-123.4052);
            Assert.AreEqual<int>(-123, dms.d);
            Assert.AreEqual<int>(-40, dms.m);
            Assert.AreEqual(-52, dms.s, 1e-8);

            dms = SurMath.DmsToDms(1.4000);
            Assert.AreEqual<int>(1, dms.d);
            Assert.AreEqual<int>(40, dms.m);
            Assert.AreEqual(0, dms.s, 1e-8);

            dms = SurMath.DmsToDms(-1.4000);
            Assert.AreEqual<int>(-1, dms.d);
            Assert.AreEqual<int>(-40, dms.m);
            Assert.AreEqual(0, dms.s, 1e-8);
        }

        [TestMethod]
        public void TestRAD2DMS()
        {
            var dms =SurMath.Radian2Dms(0.410855952558766);
            Assert.AreEqual<int>(23, dms.d);
            Assert.AreEqual<int>(32, dms.m);
            Assert.AreEqual(25.12345, dms.s, 1e-8);
            

            dms = SurMath.Radian2Dms(-0.410855952558766);
            Assert.AreEqual<int>(-23, dms.d);
            Assert.AreEqual<int>(-32, dms.m);
            Assert.AreEqual(-25.12345, dms.s, 1e-8);
            //Assert.AreEqual("-23°32′25.12345″", aa);
        }

        [TestMethod]
        public void TestAzimuth()
        {
            var ad = SurMath.Azimuth(12.234,23.1234,  13.2323, 24.232332);
            Assert.AreEqual(0.837851107, ad.a, 1e-9);

            ad = SurMath.Azimuth(12.234, 23.1234, 3.2323, 24.232332);
            Assert.AreEqual(3.019018832, ad.a, 1e-9);

            ad = SurMath.Azimuth(12.234, 23.1234, 3.2323, 4.232332);
            Assert.AreEqual(4.267712913, ad.a, 1e-9);

            ad = SurMath.Azimuth(12.234, 23.1234, 13.2323, 4.232332);
            Assert.AreEqual(4.765184951, ad.a, 1e-9);
        }
    }
}
