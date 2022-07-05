using HarmonyLib;
using Verse;

namespace AnimalGenetics.Assembly;

[HarmonyPatch(typeof(VerbProperties), nameof(VerbProperties.GetDamageFactorFor), typeof(Tool), typeof(Pawn),
    typeof(HediffComp_VerbGiver))]
public static class VerbProperties_GetDanageFactorFor_Patch
{
    public static void Postfix(ref float __result, Pawn __1)
    {
        if (!Genes.EffectsThing(__1))
        {
            return;
        }

        __result *= Genes.GetGene(__1, AnimalGenetics.Damage);
    }
}