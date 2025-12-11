using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace AnimalGenetics;

public class PawnTable_Genetics(PawnTableDef def, Func<IEnumerable<Pawn>> pawnsGetter, int uiWidth, int uiHeight)
    : PawnTable(def, pawnsGetter, uiWidth, uiHeight)
{
    public override IEnumerable<Pawn> PrimarySortFunction(IEnumerable<Pawn> input)
    {
        switch (CoreMod.Instance.Settings.sortMode)
        {
            case 1:
                return from p in input
                    orderby p.def.label
                    select p;
            case 2:
                return from p in input
                    orderby p.def.label descending
                    select p;
        }

        return input;
    }
}