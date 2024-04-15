using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace AnimalGenetics;

public static class Extensions
{
    public static GeneticInformation AnimalGenetics(this Pawn pawn)
    {
        return pawn.TryGetComp<BaseGeneticInformation>()?.GeneticInformation;
    }

    public static float GetGene(this Pawn pawn, StatDef stat)
    {
        var record = GetGeneRecord(pawn, stat);

        return record?.Value ?? 1.0f;
    }

    public static GeneRecord GetGeneRecord(this Pawn pawn, StatDef stat)
    {
        var records = pawn.AnimalGenetics()?.GeneRecords;

        return records?.GetValueOrDefault(stat);
    }

    public static IEnumerable<StatDef> GetGenes(this Pawn pawn)
    {
        if (!Genes.EffectsThing(pawn))
        {
            return new List<StatDef>();
        }

        return Constants.affectedStats.Where(stat =>
            stat != global::AnimalGenetics.AnimalGenetics.GatherYield || Genes.Gatherable(pawn));
    }
}