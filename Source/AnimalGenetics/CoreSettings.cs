using Verse;

namespace AnimalGenetics;

public class CoreSettings : ModSettings
{
    public float bestGeneChance;
    public bool ColonyManagerIntegration;
    public int colorMode;
    public float geneValue;
    public bool humanMode;
    public float mean;
    public float mutationMean;
    public float mutationStdDev;
    public bool omniscientMode;
    public bool showBothParentsInPawnTab;
    public bool showGenesInAnimalsTab;
    public bool showGenesInWildlifeTab;
    public bool showGeneticsTab;
    public int sortMode;
    public float stdDev;
    public bool wildnessMode;

    public CoreSettings()
    {
        ResetCore();
        ResetUI();
        ResetIntegration();
    }

    public void ResetCore()
    {
        stdDev = 0.12f;
        mean = 1f;
        mutationStdDev = 0.05f;
        mutationMean = 0.03f;
        humanMode = false;
        wildnessMode = true;
        omniscientMode = true;
        bestGeneChance = 0.5f;
        geneValue = 1f;
    }

    public void ResetUI()
    {
        colorMode = 1;
        showGenesInAnimalsTab = false;
        showGenesInWildlifeTab = false;
        showBothParentsInPawnTab = false;
        showGeneticsTab = true;
        sortMode = 0;
    }

    public void ResetIntegration()
    {
        ColonyManagerIntegration = false;
    }

    public override void ExposeData()
    {
        var defaults = new CoreSettings();
        Scribe_Values.Look(ref stdDev, "stdDev", defaults.stdDev);
        Scribe_Values.Look(ref mean, "mean", defaults.mean);
        Scribe_Values.Look(ref geneValue, "geneValue", defaults.geneValue);
        Scribe_Values.Look(ref mutationStdDev, "mutationStdDev", defaults.mutationStdDev);
        Scribe_Values.Look(ref mutationMean, "mutationMean", defaults.mutationMean);
        Scribe_Values.Look(ref humanMode, "humanMode", defaults.humanMode);
        Scribe_Values.Look(ref wildnessMode, "wildnessMode", defaults.wildnessMode);
        Scribe_Values.Look(ref omniscientMode, "omniscientMode", defaults.omniscientMode);
        Scribe_Values.Look(ref bestGeneChance, "bestGeneChance", defaults.bestGeneChance);
        Scribe_Values.Look(ref colorMode, "colorMode", defaults.colorMode);
        Scribe_Values.Look(ref showGenesInAnimalsTab, "showGenesInAnimalsTab", defaults.showGenesInAnimalsTab);
        Scribe_Values.Look(ref showGenesInWildlifeTab, "showGenesInWildlifeTab", defaults.showGenesInWildlifeTab);
        Scribe_Values.Look(ref showBothParentsInPawnTab, "showBothParentsInPawnTab", defaults.showBothParentsInPawnTab);
        Scribe_Values.Look(ref showGeneticsTab, "showGeneticsTab", defaults.showGeneticsTab);
        Scribe_Values.Look(ref ColonyManagerIntegration, "colonyManagerIntegration", defaults.ColonyManagerIntegration);
    }
}