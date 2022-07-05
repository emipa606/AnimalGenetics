using RimWorld;
using Verse;

namespace AnimalGenetics;

public class StatPart : RimWorld.StatPart
{
    private readonly StatDef _StatDef;

    public StatPart(StatDef statDef)
    {
        _StatDef = statDef;
        priority = 1.1f;
    }

    public override void TransformValue(StatRequest req, ref float val)
    {
        var factor = GetFactor(req);
        if (factor != null)
        {
            val *= (float)factor;
        }
    }

    public override string ExplanationPart(StatRequest req)
    {
        if (!Genes.EffectsThing(req.Thing))
        {
            return null;
        }

        if (!(req.Thing is Pawn pawn))
        {
            return "";
        }

        if (!Settings.Core.omniscientMode && pawn.Faction != Faction.OfPlayer)
        {
            return null;
        }

        var statRecord = pawn.GetGeneRecord(_StatDef);

        if (statRecord == null)
        {
            return null;
        }

        var postfix = "";
        if (statRecord.Parent == GeneRecord.Source.None)
        {
            return "AG.Genetics".Translate() + ": x" + statRecord.Value.ToStringPercent() + postfix;
        }

        var icon = statRecord.Parent == GeneRecord.Source.Mother ? "♀" : "♂";

        var parentGeneticInformation = statRecord.Parent == GeneRecord.Source.Mother
            ? pawn.AnimalGenetics()?.Mother
            : pawn.AnimalGenetics()?.Father;

        // Shouldn't really occur...
        if (parentGeneticInformation == null)
        {
            return "AG.Genetics".Translate() + ": x" + statRecord.Value.ToStringPercent() + postfix;
        }

        var parentValue = parentGeneticInformation.GeneRecords[_StatDef].Value;
        postfix = $" (x{parentValue.ToStringPercent()}{icon})";

        return "AG.Genetics".Translate() + ": x" + statRecord.Value.ToStringPercent() + postfix;
    }

    private float? GetFactor(StatRequest req)
    {
        if (!req.HasThing)
        {
            return null;
        }

        if (!Genes.EffectsThing(req.Thing))
        {
            return null;
        }

        var pawn = req.Thing as Pawn;

        if (pawn == null)
        {
            Log.Error($"{req.Thing.ToStringSafe()} is not a Pawn");
        }

        var statRecord = pawn.GetGeneRecord(_StatDef);
        return statRecord?.Value ?? 1.0f;
    }
}