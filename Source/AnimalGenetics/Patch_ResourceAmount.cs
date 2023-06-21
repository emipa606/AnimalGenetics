using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;

namespace AnimalGenetics.Assembly;

[HarmonyPatch]
public static class Patch_ResourceAmount
{
    private static IEnumerable<MethodBase> TargetMethods()
    {
        yield return AccessTools.Method(typeof(CompMilkable), "get_ResourceAmount");
        yield return AccessTools.Method(typeof(CompShearable), "get_ResourceAmount");

        if (ModLister.GetActiveModWithIdentifier("OskarPotocki.VanillaFactionsExpanded.Core") == null)
        {
            yield break;
        }

        Log.Message("[AnimalGenetics]: Added patch for CompAnimalProduct in Vanilla Expanded Framework");
        yield return AccessTools.Method("AnimalBehaviours.CompAnimalProduct:get_ResourceAmount");
    }

    public static void Postfix(ref int __result, CompMilkable __instance)
    {
        if (!Genes.EffectsThing(__instance.parent))
        {
            return;
        }

        __result = (int)(__result * Genes.GetGene((Pawn)__instance.parent, AnimalGenetics.GatherYield));
    }
}