using System;
using AnimalGenetics.HarmonyPatches;
using UnityEngine;
using Verse;

namespace AnimalGenetics;

internal class SettingsUI
{
    public static void DrawGraph(Rect rect, int xMin, int xMax, float mean, float stddev)
    {
        if (stddev < 1f / Math.Sqrt(2f * Math.PI))
        {
            stddev = (float)(1f / Math.Sqrt(2f * Math.PI));
        }

        var height = rect.height;
        var width = rect.width;

        var vscale = (height - 30) / Value(mean);
        var hscale = (width - 40 - 20) / (xMax - xMin);

        GUI.BeginGroup(rect);

        GUI.color = Color.grey;

        Widgets.DrawLineHorizontal(40, height - 20, width - 60);

        if (xMin <= 0 && xMax >= 0)
        {
            Widgets.DrawLineVertical(40 + (-xMin * hscale), 10, height - 30);
        }

        if (xMin <= 100 && xMax >= 100)
        {
            Widgets.DrawLineVertical(40 + ((100 - xMin) * hscale), 10, height - 30);
        }

        Text.Anchor = TextAnchor.MiddleCenter;

        Widgets.Label(new Rect(0, height - 30, 40, 20), "0%");
        Widgets.Label(new Rect(20, height - 20, 40, 20), xMin.ToString());
        Widgets.Label(new Rect(40 - 20 + ((width - 40 - 20) / 2), height - 20, 40, 20),
            (xMin + ((xMax - xMin) / 2)).ToString());
        Widgets.Label(new Rect(width - 40, height - 20, 40, 20), xMax.ToString());

        Widgets.Label(new Rect(0, 0, 40, 20), $"{(int)(100 * Value(mean))}%");

        GUI.color = Color.white;

        var prev = new Vector2(xMin, Value(xMin));

        for (var offsetX = -15; offsetX <= 15; ++offsetX)
        {
            var x = mean + (stddev * (offsetX / 5f));
            if (x <= xMin || x >= xMax)
            {
                continue;
            }

            DrawTo(new Vector2(x, Value(x)));
        }

        DrawTo(new Vector2(xMax, Value(xMax)));

        GUI.EndGroup();

        Text.Anchor = TextAnchor.UpperLeft;
        return;

        float Value(float x)
        {
            return (float)
                   (1f / (stddev * Math.Sqrt(2f * Math.PI))) *
                   (float)Math.Pow(Math.E, -(x - mean) * (x - mean) / (2f * stddev * stddev));
        }

        void DrawTo(Vector2 next)
        {
            Widgets.DrawLine(
                new Vector2(40 + (hscale * (prev.x - xMin)), -20 + height - (vscale * prev.y)),
                new Vector2(40 + (hscale * (next.x - xMin)), -20 + height - (vscale * next.y)),
                Color.white, 1);
            prev = next;
        }
    }

    public static void DoSettings(CoreSettings settings, Rect rect)
    {
        float curY = 80;

        var generationGraph = new Rect((rect.width / 2) + 10, 0, (rect.width / 2) - 10, 0);
        var mutationGraph = new Rect((rect.width / 2) + 10, 0, (rect.width / 2) - 10, 0);

        var listingStandard = new Listing_Standard();
        listingStandard.Begin(new Rect(10, curY, (rect.width / 2) - 10, 400f));

        generationGraph.y = listingStandard.CurHeight;

        listingStandard.Label("AG.Settings1".Translate(), -1f, "AG.Settings1Tooltip".Translate());
        listingStandard.Label("AG.Mean".Translate() + " : " + (settings.mean * 100).ToString("F0"));
        settings.mean = listingStandard.Slider(settings.mean, 0f, 2f);
        listingStandard.Label("AG.StandardDeviation".Translate() + " : " + (settings.stdDev * 100).ToString("F0"));
        settings.stdDev = listingStandard.Slider(settings.stdDev, 0f, 0.5f);

        generationGraph.height = listingStandard.CurHeight - generationGraph.y;

        listingStandard.Gap(20f);

        mutationGraph.y = listingStandard.CurHeight;

        listingStandard.Label("AG.Settings2".Translate(), -1f, "AG.Settings2Tooltip".Translate());
        listingStandard.Label("AG.Mean".Translate() + " : " + (settings.mutationMean * 100).ToString("F0"));
        settings.mutationMean = listingStandard.Slider(settings.mutationMean, -0.25f, 0.25f);
        listingStandard.Label("AG.StandardDeviation".Translate() + " : " +
                              (settings.mutationStdDev * 100).ToString("F0"));
        settings.mutationStdDev = listingStandard.Slider(settings.mutationStdDev, 0f, 0.5f);

        mutationGraph.height = listingStandard.CurHeight - mutationGraph.y;

        listingStandard.Gap(20f);

        generationGraph.y += curY;
        mutationGraph.y += curY;

        listingStandard.Label("AnimalGenetics.ChanceInheritBestGene".Translate() + " : " +
                              (settings.bestGeneChance * 100).ToString("F0"));
        settings.bestGeneChance = listingStandard.Slider(settings.bestGeneChance, 0.0f, 1.0f);

        listingStandard.Label("AnimalGenetics.GeneValue".Translate(settings.geneValue.ToStringPercent()), -1f,
            "AnimalGenetics.GeneValueToolTip".Translate());
        settings.geneValue = listingStandard.Slider(settings.geneValue, 0f, 2f);
        listingStandard.End();

        DrawGraph(generationGraph, 0, 200, settings.mean * 100, settings.stdDev * 100);
        DrawGraph(mutationGraph, -25, 25, settings.mutationMean * 100, settings.mutationStdDev * 100);

        curY += listingStandard.CurHeight;

        var listingStandard2 = new Listing_Standard();
        listingStandard2.Begin(new Rect(0, curY, (rect.width / 2) - 10, 250f));

        listingStandard2.Gap(30f);
        listingStandard2.CheckboxLabeled("AG.HumanlikeGenes".Translate(), ref settings.humanMode,
            "AG.HumanlikeGenesTooltip".Translate());
        listingStandard2.CheckboxLabeled("AG.Omniscient".Translate(), ref settings.omniscientMode,
            "AG.OmniscientTooltip".Translate());

        if (listingStandard2.ButtonText("AG.DefaultSettings".Translate()))
        {
            settings.Reset();
        }

        listingStandard2.Gap(30f);
        listingStandard2.End();
    }

    public static void DoSettings(UISettings settings, Rect rect)
    {
        float curY = 80;

        var rect2 = new Rect(0, curY, (rect.width / 2) - 10, 250f);
        var listingStandard2 = new Listing_Standard();
        listingStandard2.Begin(rect2);
        listingStandard2.Label("AG.ColorMode".Translate());
        if (listingStandard2.RadioButton("AG.ColorNormal".Translate(), settings.colorMode == 0, 8f,
                "AG.ColorNormalTooltip".Translate(), 0f))
        {
            settings.colorMode = 0;
        }

        if (listingStandard2.RadioButton("AG.ColorRPG".Translate(), settings.colorMode == 1, 8f,
                "AG.ColorRPGTooltip".Translate(), 0f))
        {
            settings.colorMode = 1;
        }

        listingStandard2.Gap(30f);
        listingStandard2.End();

        var rhs = new Rect((rect.width / 2) + 10, curY, (rect.width / 2) - 10, 250f);
        var listingStandardRhs = new Listing_Standard();
        listingStandardRhs.Begin(rhs);
        listingStandardRhs.CheckboxLabeled("AnimalGenetics.ShowGenesInAnimalsTab".Translate(),
            ref settings.showGenesInAnimalsTab, "AnimalGenetics.ShowGenesInAnimalsTabTooltip".Translate());
        listingStandardRhs.CheckboxLabeled("AnimalGenetics.ShowGenesInWildlifeTab".Translate(),
            ref settings.showGenesInWildlifeTab, "AnimalGenetics.ShowGenesInWildlifeTabTooltip".Translate());
        listingStandardRhs.CheckboxLabeled("AnimalGenetics.ShowBothParentsInPawnTab".Translate(),
            ref settings.showBothParentsInPawnTab, "AnimalGenetics.ShowBothParentsInPawnTabTooltip".Translate());
        listingStandardRhs.CheckboxLabeled("AnimalGenetics.ShowGeneticsTab".Translate(), ref settings.showGeneticsTab,
            "AnimalGenetics.ShowGeneticsTabTooltip".Translate());

        listingStandardRhs.End();

        var bottom = new Listing_Standard();
        var bottomRect = new Rect(0, rect2.y + listingStandard2.CurHeight, rect.width, 100);
        bottom.Begin(bottomRect);
        if (bottom.ButtonText("AG.DefaultSettings".Translate()))
        {
            settings.Reset();
        }

        bottom.End();

        AnimalGeneticsAssemblyLoader.PatchUI();
    }

    public static void DoSettings(IntegrationSettings settings, Rect rect)
    {
        float curY = 80;

        var rect2 = new Rect(0, curY, (rect.width / 2) - 10, 250f);
        var listingStandard2 = new Listing_Standard();
        listingStandard2.Begin(rect2);

        listingStandard2.Gap(30f);
        if (AnimalGeneticsAssemblyLoader.ColonyManagerLoaded)
        {
            listingStandard2.CheckboxLabeled("AnimalGenetics.ColonyManager.Integrate".Translate(),
                ref settings.ColonyManagerIntegration, "AnimalGenetics.ColonyManager.IntegrateTooltip".Translate());
            listingStandard2.Label("AnimalGenetics.NeedsRestart".Translate());
            listingStandard2.Gap(30f);
        }
        else
        {
            settings.ColonyManagerIntegration = false;
        }

        listingStandard2.End();

        var bottom = new Listing_Standard();
        var bottomRect = new Rect(0, rect2.y + listingStandard2.CurHeight, rect.width, 100);
        bottom.Begin(bottomRect);
        if (bottom.ButtonText("AG.DefaultSettings".Translate()))
        {
            settings.Reset();
        }

        bottom.End();

        AnimalGeneticsAssemblyLoader.PatchUI();
    }
}