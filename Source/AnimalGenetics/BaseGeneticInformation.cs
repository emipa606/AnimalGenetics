using System.Collections.Generic;
using RimWorld;
using Verse;

namespace AnimalGenetics;

public class BaseGeneticInformation : ThingComp
{
    private static readonly Dictionary<BaseGeneticInformation, Dictionary<StatDef, GeneRecord>> LegacyGenesRecords =
        new Dictionary<BaseGeneticInformation, Dictionary<StatDef, GeneRecord>>();

    public GeneticInformation GeneticInformation;

    public override void Initialize(CompProperties props)
    {
        base.Initialize(props);
        GeneticInformation = ParentReferences.ThisGeneticInformation ?? new GeneticInformation();
    }

    public override void PostExposeData()
    {
        base.PostExposeData();

        if (!Scribe.EnterNode("animalGenetics"))
        {
            return;
        }

        Scribe_References.Look(ref GeneticInformation, "geneticInformation");

        Dictionary<StatDef, GeneRecord> legacyGenesRecord = null;
        Scribe_Collections.Look(ref legacyGenesRecord, "geneRecords");
        if (legacyGenesRecord != null)
        {
            LegacyGenesRecords[this] = legacyGenesRecord;
        }

        if (Scribe.mode == LoadSaveMode.PostLoadInit)
        {
            if (GeneticInformation == null)
            {
                if (LegacyGenesRecords.ContainsKey(this))
                {
                    Log.Message($"Migrating Legacy Genetic Information for {parent}");
                    GeneticInformation = new GeneticInformation(LegacyGenesRecords[this]);
                    LegacyGenesRecords.Remove(this);
                }
                else
                {
                    Log.Message($"Generating Genetic Information for {parent}");
                    GeneticInformation = new GeneticInformation(null);
                }

                GeneticCalculator.EnsureAllGenesExist(GeneticInformation.GeneRecords, null, null);
            }
            else
            {
                GeneticCalculator.EnsureAllGenesExist(GeneticInformation.GeneRecords, GeneticInformation.Mother,
                    GeneticInformation.Father);
            }
        }

        Scribe.ExitNode();
    }
}