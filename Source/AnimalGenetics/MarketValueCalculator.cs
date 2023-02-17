using System.Linq;
using RimWorld;
using Verse;

namespace AnimalGenetics;

internal class MarketValueCalculator : RimWorld.StatPart
{
    public override void TransformValue(StatRequest req, ref float val)
    {
        var pawn = req.Thing as Pawn;
        if (!Genes.EffectsThing(pawn))
        {
            return;
        }

        var genes = pawn.GetGenes();

        float Selector(StatDef g)
        {
            return pawn.GetGene(g);
        }

        var factor = genes.Select(Selector).Aggregate(1.0f, (lhs, rhs) => lhs * rhs);

        val *= factor * Settings.Core.geneValue;
    }

    public override string ExplanationPart(StatRequest req)
    {
        float factor = 1;
        TransformValue(req, ref factor);
        return "AG.Genetics".Translate() + ": x" + factor.ToStringPercent() + "\n";
    }
}