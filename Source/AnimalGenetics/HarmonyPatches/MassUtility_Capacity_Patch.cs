using HarmonyLib;
using RimWorld;
using Verse;

namespace AnimalGenetics.HarmonyPatches;

[HarmonyPatch(typeof(MassUtility), nameof(MassUtility.Capacity))]
public static class MassUtility_Capacity_Patch
{
    public static void Postfix(ref float __result, Pawn __0)
    {
        if (!Genes.EffectsThing(__0))
        {
            return;
        }

        __result *= Genes.GetGene(__0, StatDefOf.CarryingCapacity);
    }
}