using HarmonyLib;
using RimWorld;
using Verse;

namespace AnimalGenetics.Assembly;

[HarmonyPatch(typeof(CompShearable), "get_ResourceAmount")]
public static class PatchCompShearable_ResourceAmount
{
    public static void Postfix(ref int __result, CompShearable __instance)
    {
        if (!Genes.EffectsThing(__instance.parent))
        {
            return;
        }

        __result = (int)(__result * Genes.GetGene((Pawn)__instance.parent, AnimalGenetics.GatherYield));
    }
}