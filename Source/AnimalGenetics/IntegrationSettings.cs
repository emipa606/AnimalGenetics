using System;
using Verse;

namespace AnimalGenetics;

public class IntegrationSettings : ModSettings, ICloneable
{
    public bool ColonyManagerIntegration;

    public IntegrationSettings()
    {
        Reset();
    }

    public object Clone()
    {
        return MemberwiseClone();
    }

    public void Reset()
    {
        ColonyManagerIntegration = false;
    }

    public override void ExposeData()
    {
        var defaults = new IntegrationSettings();
        Scribe_Values.Look(ref ColonyManagerIntegration, "colonyManagerIntegration", defaults.ColonyManagerIntegration);
    }
}