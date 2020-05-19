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
            int d, m;
            double s;

            SurMath.DMStoDMS(123.4052,
                out d, out m, out s);
            Assert.AreEqual<int>(123, d);
            Assert.AreEqual<int>(40, m);
            Assert.AreEqual(52, s, 1e-8);

            SurMath.DMStoDMS(-123.4052,
                out d, out m, out s);
            Assert.AreEqual<int>(-123, d);
            Assert.AreEqual<int>(-40, m);
            Assert.AreEqual(-52, s, 1e-8);

            SurMath.DMStoDMS(1.4000,
                out d, out m, out s);
            Assert.AreEqual<int>(1, d);
            Assert.AreEqual<int>(40, m);
            Assert.AreEqual(0, s, 1e-8);

            SurMath.DMStoDMS(-1.4000,
                out d, out m, out s);
            Assert.AreEqual<int>(-1, d);
            Assert.AreEqual<int>(-40, m);
            Assert.AreEqual(0, s, 1e-8);
        }

        [TestMethod]
        public void TestRAD2DMS()
        {
            int d, m;
            double s;

            SurMath.RADtoDMS(0.410855952558766,
                out d, out m, out s);
            Assert.AreEqual<int>(23, d);
            Assert.AreEqual<int>(32, m);
            Assert.AreEqual(25.12345, s, 1e-8);
            

            SurMath.RADtoDMS(-0.410855952558766,
                out d, out m, out s);
            Assert.AreEqual<int>(-23, d);
            Assert.AreEqual<int>(-32, m);
            Assert.AreEqual(-25.12345, s, 1e-8);
            //Assert.AreEqual("-23°32′25.12345″", aa);
        }

        [TestMethod]
        public void TestAzimuth()
        {
            double a = SurMath.Azimuth(12.234,23.1234, 
                13.2323, 24.232332);
            Assert.AreEqual(0.837851107, a, 1e-9);

            a = SurMath.Azimuth(12.234, 23.1234,
                3.2323, 24.232332);
            Assert.AreEqual(3.019018832, a, 1e-9);

            a = SurMath.Azimuth(12.234, 23.1234,
                3.2323, 4.232332);
            Assert.AreEqual(4.267712913, a, 1e-9);

            a = SurMath.Azimuth(12.234, 23.1234,
                13.2323, 4.232332);
            Assert.AreEqual(4.765184951, a, 1e-9);
        }
    }
}
