using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace AnimalGenetics;

public static class Extensions
{
    extension(Pawn pawn)
    {
        public GeneticInformation AnimalGenetics()
        {
            return pawn.TryGetComp<BaseGeneticInformation>()?.GeneticInformation;
        }

        public float GetGene(StatDef stat)
        {
            var record = GetGeneRecord(pawn, stat);

            return record?.Value ?? 1.0f;
        }

        public GeneRecord GetGeneRecord(StatDef stat)
        {
            var records = pawn.AnimalGenetics()?.GeneRecords;

            return records?.GetValueOrDefault(stat);
        }

        public IEnumerable<StatDef> GetGenes()
        {
            if (!Genes.EffectsThing(pawn))
            {
                return new List<StatDef>();
            }

            return Constants.AffectedStats.Where(stat =>
                stat != global::AnimalGenetics.AnimalGenetics.GatherYield || Genes.Gatherable(pawn));
        }
    }
}