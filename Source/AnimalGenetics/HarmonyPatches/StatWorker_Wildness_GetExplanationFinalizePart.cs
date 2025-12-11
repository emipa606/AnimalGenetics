using HarmonyLib;
using RimWorld;
using Verse;

namespace AnimalGenetics.HarmonyPatches;

[HarmonyPatch(typeof(StatWorker_Wildness), nameof(StatWorker_Wildness.GetExplanationFinalizePart))]
public static class StatWorker_Wildness_GetExplanationFinalizePart
{
    public static void Postfix(ref string __result, StatRequest req)
    {
        if (!CoreMod.Instance.Settings.wildnessMode)
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

        var geneValue = Genes.GetGene(pawn, StatDefOf.Wildness);
        __result += "\n" + "AnimalGenetics.GeneValue".Translate(geneValue.ToStringPercent());
    }
}