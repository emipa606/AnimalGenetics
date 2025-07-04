using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;

namespace AnimalGenetics.HarmonyPatches;

[HarmonyPatch]
public static class Comp_ResourceAmount
{
    private static IEnumerable<MethodBase> TargetMethods()
    {
        yield return AccessTools.Method(typeof(CompMilkable), "get_ResourceAmount");
        yield return AccessTools.Method(typeof(CompShearable), "get_ResourceAmount");

        if (ModLister.GetActiveModWithIdentifier("OskarPotocki.VanillaFactionsExpanded.Core", true) == null)
        {
            yield break;
        }

        Log.Message("[AnimalGenetics]: Added patch for CompAnimalProduct in Vanilla Expanded Framework");
        yield return AccessTools.Method("VEF.AnimalBehaviours.CompAnimalProduct:get_ResourceAmount");
    }

    public static void Postfix(ref int __result, CompHasGatherableBodyResource __instance)
    {
        if (!Genes.EffectsThing(__instance.parent))
        {
            return;
        }

        __result = (int)Math.Max(__result * Genes.GetGene((Pawn)__instance.parent, AnimalGenetics.GatherYield), 1f);
    }
}