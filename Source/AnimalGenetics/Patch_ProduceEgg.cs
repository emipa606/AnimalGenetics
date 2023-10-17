using HarmonyLib;
using RimWorld;
using Verse;

namespace AnimalGenetics.Assembly;

[HarmonyPatch(typeof(CompEggLayer), nameof(CompEggLayer.ProduceEgg))]
public static class Patch_ProduceEgg
{
    public static void Postfix(ref Thing __result, CompEggLayer __instance)
    {
        if (!Genes.EffectsThing(__instance.parent))
        {
            return;
        }

        __result.stackCount =
            (int)(__result.stackCount * Genes.GetGene((Pawn)__instance.parent, AnimalGenetics.GatherYield));
    }
}