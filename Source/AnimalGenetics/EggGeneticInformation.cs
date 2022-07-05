using RimWorld;
using Verse;

namespace AnimalGenetics;

public class EggGeneticInformation : BaseGeneticInformation
{
    public override void CompTick()
    {
        var comp = parent.TryGetComp<CompHatcher>();
        if (comp == null)
        {
            return;
        }

        if (GeneticInformation.Mother == null && comp.hatcheeParent != null)
        {
            GeneticInformation.Mother = comp.hatcheeParent.AnimalGenetics();
        }

        if (GeneticInformation.Father == null && comp.otherParent != null)
        {
            GeneticInformation.Father = comp.otherParent.AnimalGenetics();
        }
    }
}