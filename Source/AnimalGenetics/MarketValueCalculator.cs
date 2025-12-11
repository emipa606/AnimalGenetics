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

        var factor = genes.Select(selector).Aggregate(1.0f, (lhs, rhs) => lhs * rhs);

        val *= factor * CoreMod.Instance.Settings.geneValue;
        return;

        float selector(StatDef g)
        {
            return pawn.GetGene(g);
        }
    }

    public override string ExplanationPart(StatRequest req)
    {
        float factor = 1;
        TransformValue(req, ref factor);
        return "AG.Genetics".Translate() + ": x" + factor.ToStringPercent() + "\n";
    }
}