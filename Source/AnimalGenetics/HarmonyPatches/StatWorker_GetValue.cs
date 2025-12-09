using HarmonyLib;
using RimWorld;
using Verse;

namespace AnimalGenetics.HarmonyPatches;

[HarmonyPatch(typeof(RimWorld.StatWorker), nameof(RimWorld.StatWorker.GetValue), typeof(StatRequest), typeof(bool))]
public static class StatWorker_GetValue
{
    public static void Postfix(ref float __result, StatRequest req, RimWorld.StatWorker __instance)
    {
        if (__instance is not StatWorker_Wildness)
        {
            return;
        }

        if (!req.HasThing)
        {
            return;
        }

        if (req.Thing is not Pawn pawn)
        {
            return;
        }


        if (!Genes.EffectsThing(pawn))
        {
            return;
        }

        __result *= Genes.GetGene(pawn, StatDefOf.Wildness);
    }
}