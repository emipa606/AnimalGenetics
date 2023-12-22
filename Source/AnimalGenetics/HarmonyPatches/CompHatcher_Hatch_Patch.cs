using HarmonyLib;
using RimWorld;
using Verse;

namespace AnimalGenetics.HarmonyPatches;

[HarmonyPatch(typeof(CompHatcher), nameof(CompHatcher.Hatch))]
public static class CompHatcher_Hatch_Patch
{
    public static void Prefix(ThingWithComps ___parent, out bool __state)
    {
        __state = false;
        if (___parent == null)
        {
            __state = true;
            return;
        }

        var comp = ___parent.TryGetComp<EggGeneticInformation>();
        if (comp == null)
        {
            __state = true;
            return;
        }

        ParentReferences.Push(new ParentReferences.Record { This = comp.GeneticInformation });
    }

    public static void Postfix(bool __state)
    {
        if (!__state)
        {
            ParentReferences.Pop();
        }
    }
}