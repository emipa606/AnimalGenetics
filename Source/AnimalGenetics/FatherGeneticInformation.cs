using System.Reflection;
using Verse;

namespace AnimalGenetics;

public class FatherGeneticInformation : HediffComp
{
    private FieldInfo _fatherField;

    private GeneticInformation _geneticInformation;
    public GeneticInformation GeneticInformation => _geneticInformation;

    public override void CompPostMake()
    {
        _fatherField = parent.GetType().GetField("father");
    }

    public override void CompPostTick(ref float severityAdjustment)
    {
        if (_geneticInformation != null || _fatherField == null)
        {
            return;
        }

        var father = _fatherField?.GetValue(parent) as Pawn;
        _geneticInformation = father?.AnimalGenetics();
    }

    public override void CompExposeData()
    {
        base.CompExposeData();

        if (!Scribe.EnterNode("animalGenetics"))
        {
            return;
        }

        Scribe_References.Look(ref _geneticInformation, "fatherGeneRecords");
        Scribe.ExitNode();
    }
}