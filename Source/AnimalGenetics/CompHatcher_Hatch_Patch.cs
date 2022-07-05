using HarmonyLib;
using RimWorld;
using Verse;

namespace AnimalGenetics.Assembly;

[HarmonyPatch(typeof(CompHatcher), nameof(CompHatcher.Hatch))]
public static class CompHatcher_Hatch_Patch
{
    public static void Prefix(ThingWithComps ___parent)
    {
        var comp = ___parent.TryGetComp<EggGeneticInformation>();
        ParentReferences.Push(new ParentReferences.Record { This = comp.GeneticInformation });
    }

    public static void Postfix()
    {
        ParentReferences.Pop();
    }
}