using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZXY
{
    public static class SpheroidFactory
    {
        private static List<Spheroid> spheroidList = new List<Spheroid>();
        public static List<Spheroid> GetSpheroidList() => spheroidList;


        private static Dictionary<string, Spheroid> spheroids = new Dictionary<string, Spheroid>();
        public static Dictionary<string, Spheroid> GetSpheroids() => spheroids;

        static SpheroidFactory()
        {
            spheroidList.Add( Spheroid.CreateCGCS2000());
            spheroidList.Add( Spheroid.CreateBeiJing1954());
            spheroidList.Add( Spheroid.CreateXiAn1980());
            spheroidList.Add( Spheroid.CreateWGS84());
            spheroidList.Add(Spheroid.CreateCustomSpheroid(6378137, 298.257222101));

            foreach (var it in spheroidList)
            {
                spheroids.Add(it.Id, it);
            }
        }
    }
}
