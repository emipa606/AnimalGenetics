using RimWorld;

namespace AnimalGenetics;

[DefOf]
public static class PawnTableColumnsDefOf
{
    public static PawnTableColumnsDef Genetics;

    static PawnTableColumnsDefOf()
    {
        DefOfHelper.EnsureInitializedInCtor(typeof(PawnTableColumnsDefOf));
    }
}