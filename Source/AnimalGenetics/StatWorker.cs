using RimWorld;
using Verse;

namespace AnimalGenetics;

internal class StatWorker : RimWorld.StatWorker
{
    public override bool ShouldShowFor(StatRequest req)
    {
        return Genes.EffectsThing(req.Thing);
    }

    public override float GetValueUnfinalized(StatRequest req, bool applyPostProcess = true)
    {
        return Genes.GetGene(req.Thing as Pawn, ((StatDefWrapper)stat).Underlying);
    }

    public override string GetExplanationUnfinalized(StatRequest req, ToStringNumberSense numberSense)
    {
        return "";
    }
}