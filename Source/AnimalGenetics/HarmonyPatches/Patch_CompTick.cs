using HarmonyLib;
using RimWorld;
using Verse;

namespace AnimalGenetics.HarmonyPatches;

[HarmonyPatch(typeof(CompEggLayer), nameof(CompEggLayer.CompTick))]
public static class Patch_CompTick
{
    public static void Prefix(out float __state, float ___eggProgress)
    {
        __state = ___eggProgress;
    }

    public static void Postfix(float __state, ref float ___eggProgress, CompEggLayer __instance)
    {
        var change = ___eggProgress - __state;
        if (change <= 0)
        {
            return;
        }

        change *= Genes.GetGene((Pawn)__instance.parent, AnimalGenetics.GatherYield);
        ___eggProgress = __state + change;
    }
}