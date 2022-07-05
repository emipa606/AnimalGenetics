using HarmonyLib;
using Verse;

namespace AnimalGenetics.Assembly;

[HarmonyPatch(typeof(Pawn), "get_HealthScale")]
public static class Pawn_HealthScale_Patch
{
    public static void Postfix(ref float __result, ref Pawn __instance)
    {
        if (!Genes.EffectsThing(__instance))
        {
            return;
        }

        __result *= Genes.GetGene(__instance, AnimalGenetics.Health);
    }
}