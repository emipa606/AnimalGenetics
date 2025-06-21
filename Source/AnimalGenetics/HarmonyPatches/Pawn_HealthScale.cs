using HarmonyLib;
using Verse;

namespace AnimalGenetics.HarmonyPatches;

[HarmonyPatch(typeof(Pawn), nameof(Pawn.HealthScale), MethodType.Getter)]
public static class Pawn_HealthScale
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