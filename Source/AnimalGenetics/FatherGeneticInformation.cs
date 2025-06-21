using System.Reflection;
using Verse;

namespace AnimalGenetics;

public class FatherGeneticInformation : HediffComp
{
    private FieldInfo fatherField;

    private GeneticInformation geneticInformation;
    public GeneticInformation GeneticInformation => geneticInformation;

    public override void CompPostMake()
    {
        fatherField = parent.GetType().GetField("father");
    }

    public override void CompPostTick(ref float severityAdjustment)
    {
        if (geneticInformation != null || fatherField == null)
        {
            return;
        }

        var father = fatherField?.GetValue(parent) as Pawn;
        geneticInformation = father?.AnimalGenetics();
    }

    public override void CompExposeData()
    {
        base.CompExposeData();

        if (!Scribe.EnterNode("animalGenetics"))
        {
            return;
        }

        Scribe_References.Look(ref geneticInformation, "fatherGeneRecords");
        Scribe.ExitNode();
    }
}