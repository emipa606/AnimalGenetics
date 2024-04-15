using System;
using Verse;

namespace AnimalGenetics;

public class UISettings : ModSettings, ICloneable
{
    public int colorMode;
    public bool showBothParentsInPawnTab;
    public bool showGenesInAnimalsTab;
    public bool showGenesInWildlifeTab;
    public bool showGeneticsTab;
    public int sortMode;

    public UISettings()
    {
        Reset();
    }

    public object Clone()
    {
        return MemberwiseClone();
    }

    public void Reset()
    {
        colorMode = 1;
        showGenesInAnimalsTab = false;
        showGenesInWildlifeTab = false;
        showBothParentsInPawnTab = false;
        showGeneticsTab = true;
        sortMode = 0;
    }

    public override void ExposeData()
    {
        var defaults = new UISettings();
        Scribe_Values.Look(ref colorMode, "colorMode", defaults.colorMode);
        Scribe_Values.Look(ref showGenesInAnimalsTab, "showGenesInAnimalsTab", defaults.showGenesInAnimalsTab);
        Scribe_Values.Look(ref showGenesInWildlifeTab, "showGenesInWildlifeTab", defaults.showGenesInWildlifeTab);
        Scribe_Values.Look(ref showBothParentsInPawnTab, "showBothParentsInPawnTab", defaults.showBothParentsInPawnTab);
        Scribe_Values.Look(ref showGeneticsTab, "showGeneticsTab", defaults.showGeneticsTab);
    }
}