using RimWorld;
using Verse;

namespace AnimalGenetics.Assembly;

public static class CompatibilityPatches
{
    public static void AlphaAnimals_get_ResourceAmount_Patch(ref int __result, CompHasGatherableBodyResource __instance)
    {
        __result = (int)(__result * Genes.GetGene((Pawn)__instance.parent, AnimalGenetics.GatherYield));
    }

    public static void RJW_GenerateBabies_Prefix(Pawn ___pawn, Pawn ___father)
    {
        ParentReferences.Push(new ParentReferences.Record
        {
            Mother = ___pawn?.AnimalGenetics(),
            Father = ___father?.AnimalGenetics()
        });
    }

    public static void RJW_GenerateBabies_Postfix(Pawn ___pawn)
    {
        ParentReferences.Pop();
    }
}