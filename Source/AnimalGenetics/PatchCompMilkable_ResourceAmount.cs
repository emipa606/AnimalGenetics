using HarmonyLib;
using RimWorld;
using Verse;

namespace AnimalGenetics.Assembly;

[HarmonyPatch(typeof(CompMilkable), "get_ResourceAmount")]
public static class PatchCompMilkable_ResourceAmount
{
    public static void Postfix(ref int __result, CompMilkable __instance)
    {
        if (!Genes.EffectsThing(__instance.parent))
        {
            return;
        }

        __result = (int)(__result * Genes.GetGene((Pawn)__instance.parent, AnimalGenetics.GatherYield));
    }
}