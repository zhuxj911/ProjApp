using System;
using System.Collections.Generic;
using System.Text;

namespace ZXY;

public class ProjType
{
    public static List<IProj> ProjList { get; } = [];
    public static Dictionary<string, IProj> ProjTypes { get; } = [];

    static ProjType()
    {
        ProjList.Add(new GaussProj(EllipsoidType.EllipsoidList[0]));
        ProjList.Add(new UtmProj(EllipsoidType.EllipsoidList[0]));

        foreach (var it in ProjList)
        {
            ProjTypes.Add(it.Id, it);
        }
    }
}
