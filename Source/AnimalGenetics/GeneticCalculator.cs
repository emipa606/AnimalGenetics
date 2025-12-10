using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace AnimalGenetics;

public static class GeneticCalculator
{
    public static Dictionary<StatDef, GeneRecord> GenerateGenesRecord(GeneticInformation mother,
        GeneticInformation father)
    {
        var result = new Dictionary<StatDef, GeneRecord>();
        EnsureAllGenesExist(result, mother, father);
        return result;
    }

    public static void EnsureAllGenesExist(Dictionary<StatDef, GeneRecord> records, GeneticInformation mother,
        GeneticInformation father)
    {
        var affectedStats = Constants.AffectedStats;

        foreach (var stat in affectedStats)
        {
            if (records.ContainsKey(stat))
            {
                continue;
            }

            var motherStat = mother?.GeneRecords?[stat];
            var fatherStat = father?.GeneRecords?[stat];

            var motherValue = motherStat?.Value ??
                              Mathf.Max(Rand.Gaussian(Settings.Core.mean, Settings.Core.stdDev), 0.1f);
            var fatherValue = fatherStat?.Value ??
                              Mathf.Max(Rand.Gaussian(Settings.Core.mean, Settings.Core.stdDev), 0.1f);

            var highValue = Math.Max(motherValue, fatherValue);
            var lowValue = Math.Min(motherValue, fatherValue);

            var record = new GeneRecord();

            var parentValue = Rand.Chance(Settings.Core.bestGeneChance) ? highValue : lowValue;
            var delta = Rand.Gaussian(Settings.Core.mutationMean, Settings.Core.mutationStdDev);

            if (parentValue == motherValue)
            {
                record.Parent = motherStat != null ? GeneRecord.Source.Mother : GeneRecord.Source.None;
            }
            else
            {
                record.Parent = fatherStat != null ? GeneRecord.Source.Father : GeneRecord.Source.None;
            }

            if (Constants.InvertedStats.Contains(stat))
            {
                delta = -delta;
            }

            record.Value = Math.Max(parentValue + delta, 0);

            records[stat] = record;
        }
    }
}