using ProjApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ZXY;

namespace ProjApp
{
    class SinglePointProjWinVM : ProjWinVM
    { 
        private SPoint sPoint = new SPoint
        {
            X = 2433586.692,
            Y = 250547.403,
            dmsB = 21.58470845,
            dmsL = 113.25314880
        };
        public SPoint SPnt
        {
            get => sPoint;
        } 

        public override void BLtoXY()
        {
            //IProj proj = new GaussProj(CurrentSpheroid);

            IProj proj = new UTMProj(CurrentSpheroid);
            proj.BLtoXYKM(SPnt.B, SPnt.L, L0, YKM, N,
                out double x, out double y);
            SPnt.X = x; SPnt.Y = y;
        }

        public override void XYtoBL()
        {
            IProj proj = new GaussProj(CurrentSpheroid);
            proj.XYKMtoBL(SPnt.X, SPnt.Y, L0, YKM, N,
                out double B, out double L);
            SPnt.B = B;
            SPnt.L = L;
        }
    }
}
