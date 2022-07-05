using AnimalGenetics.Assembly;
using RimWorld;
using Verse;

namespace AnimalGenetics;

public static class Genes
{
    public static float GetGene(Pawn pawn, StatDef gene)
    {
        return pawn.GetGene(gene);
    }

    public static string GetGenderSymbol(GeneRecord.Source source)
    {
        return source == GeneRecord.Source.Mother ? "♀" : "♂";
    }

    public static string GetTooltip(StatDef gene)
    {
        return Constants.GetDescription(gene);
    }

    public static bool EffectsThing(Thing thing)
    {
        if (thing is not Pawn pawn)
        {
            return false;
        }

        return pawn.RaceProps.Animal || pawn.RaceProps.Humanlike && Settings.Core.humanMode;
    }

    public static bool Gatherable(Pawn pawn)
    {
        foreach (var type in AnimalGeneticsAssemblyLoader.gatherableTypes)
        {
            if (pawn.def.HasComp(type))
            {
                return true;
            }
        }

        return false;
    }
}