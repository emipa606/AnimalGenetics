using Verse;

namespace AnimalGenetics.HarmonyPatches;

public static class CompatibilityPatches
{
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