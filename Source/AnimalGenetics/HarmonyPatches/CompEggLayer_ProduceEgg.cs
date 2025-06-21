using System;
using HarmonyLib;
using RimWorld;
using Verse;

namespace AnimalGenetics.HarmonyPatches;

[HarmonyPatch(typeof(CompEggLayer), nameof(CompEggLayer.ProduceEgg))]
public static class CompEggLayer_ProduceEgg
{
    public static void Postfix(ref Thing __result, CompEggLayer __instance)
    {
        if (!Genes.EffectsThing(__instance.parent))
        {
            return;
        }

        __result.stackCount =
            (int)Math.Max(__result.stackCount * Genes.GetGene((Pawn)__instance.parent, AnimalGenetics.GatherYield), 1f);
    }
}