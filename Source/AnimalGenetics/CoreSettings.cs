using System;
using Verse;

namespace AnimalGenetics;

public class CoreSettings : ModSettings, ICloneable
{
    public float bestGeneChance;
    public bool humanMode;
    public float mean;
    public float mutationMean;
    public float mutationStdDev;
    public bool omniscientMode;
    public float stdDev;

    public CoreSettings()
    {
        Reset();
    }

    public object Clone()
    {
        return MemberwiseClone();
    }

    public void Reset()
    {
        stdDev = 0.12f;
        mean = 1f;
        mutationStdDev = 0.05f;
        mutationMean = 0.03f;
        humanMode = false;
        omniscientMode = true;
        bestGeneChance = 0.5f;
    }

    public override void ExposeData()
    {
        var defaults = new CoreSettings();
        Scribe_Values.Look(ref stdDev, "stdDev", defaults.stdDev);
        Scribe_Values.Look(ref mean, "mean", defaults.mean);
        Scribe_Values.Look(ref mutationStdDev, "mutationStdDev", defaults.mutationStdDev);
        Scribe_Values.Look(ref mutationMean, "mutationMean", defaults.mutationMean);
        Scribe_Values.Look(ref humanMode, "humanMode", defaults.humanMode);
        Scribe_Values.Look(ref omniscientMode, "omniscientMode", defaults.omniscientMode);
        Scribe_Values.Look(ref bestGeneChance, "bestGeneChance", defaults.bestGeneChance);
    }
}