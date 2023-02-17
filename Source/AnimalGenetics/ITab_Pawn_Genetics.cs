using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace AnimalGenetics;

internal class ITab_Pawn_Genetics : ITab
{
    public ITab_Pawn_Genetics()
    {
        labelKey = "AG.TabGenetics";
        tutorTag = "AG.Genetics";
    }

    public override bool IsVisible
    {
        get
        {
            if (!Settings.Core.omniscientMode && SelPawn.Faction != Faction.OfPlayer)
            {
                return false;
            }

            return Genes.EffectsThing(SelPawn);
        }
    }

    public override void FillTab()
    {
        var rect = new Rect(0f, 0f, size.x, size.y).ContractedBy(20f);
        var pawn = SelPawn;

        string str =
            (pawn.gender == Gender.None ? "PawnSummary" : "PawnSummaryWithGender").Translate(pawn.Named("PAWN"));
        var xAddition = 0f;
        if (DebugSettings.godMode)
        {
            xAddition = 17f;
            var buttonRect = new Rect(15f, 15f, 15f, 15f);
            TooltipHandler.TipRegion(buttonRect, "AG.RegenerateGenes".Translate());
            if (Widgets.ButtonImage(buttonRect, PermitsCardUtility.SwitchFactionIcon))
            {
                var pawnRecord = pawn.AnimalGenetics();
                pawnRecord._geneRecords = new Dictionary<StatDef, GeneRecord>();
                GeneticCalculator.EnsureAllGenesExist(pawnRecord.GeneRecords, pawnRecord.Mother,
                    pawnRecord.Father);
            }
        }

        Text.Font = GameFont.Small;
        Widgets.Label(new Rect(15f + xAddition, 15f, (rect.width * 0.9f) - xAddition, 30f),
            "AG.GeneticsOf".Translate() + ":  " + pawn.Label);
        Text.Font = GameFont.Tiny;
        Widgets.Label(new Rect(15f, 35f, rect.width * 0.9f, 30f), str);
        Text.Font = GameFont.Small;

        var headerY = 55f;
        var curY = headerY;

        Text.Anchor = TextAnchor.MiddleCenter;

        var rectValue = new Rect(rect.x + (rect.width * 0.4f), curY, rect.width * 0.2f, 20f);
        Widgets.Label(rectValue, "AG.Value".Translate());
        TooltipHandler.TipRegion(rectValue, "AG.ValueTooltop".Translate());

        curY += 20;

        var stats = Constants.affectedStats.Where(stat => stat != AnimalGenetics.GatherYield || Genes.Gatherable(pawn));
        foreach (var stat in stats)
        {
            var rect2 = new Rect(rect.x, curY, rect.width, 20f);
            TooltipHandler.TipRegion(rect2, Genes.GetTooltip(stat));
            if (Mouse.IsOver(rect2))
            {
                GUI.color = new Color(0.5f, 0.5f, 0.5f, 1f);
                GUI.DrawTexture(rect2, TexUI.HighlightTex);
                GUI.color = Color.white;
            }

            Text.Anchor = TextAnchor.MiddleLeft;

            Widgets.Label(new Rect(20f, curY, rect.x + (rect.width * 0.4f) - 20f, 20f), Constants.GetLabel(stat));

            Utility.GUI.DrawGeneValueLabel(new Rect(rect.x + (rect.width * 0.4f), curY, rect.width * 0.2f, 20f),
                pawn.GetGene(stat));

            curY += 20;
        }

        if (Settings.UI.showBothParentsInPawnTab)
        {
            DrawBothParentData(rect, headerY, pawn);
        }
        else
        {
            DrawSingleParentData(rect, headerY, pawn);
        }

        Text.Anchor = TextAnchor.UpperLeft;
    }

    public override void UpdateSize()
    {
        base.UpdateSize();
        size = new Vector2(300f, 225f);
    }

    private static void DrawBothParentData(Rect rect, float curY, Pawn pawn)
    {
        Text.Anchor = TextAnchor.MiddleCenter;

        var rectMother = new Rect(rect.x + (rect.width * 0.6f), curY, rect.width * 0.2f, 20f);
        Widgets.Label(rectMother, "AnimalGenetics.Mother".Translate());

        var rectFather = new Rect(rect.x + (rect.width * 0.8f), curY, rect.width * 0.2f, 20f);
        Widgets.Label(rectFather, "AnimalGenetics.Father".Translate());

        curY += 20;

        var statsGroup = pawn.AnimalGenetics()?.GeneRecords;
        if (statsGroup == null)
        {
            return;
        }

        var motherGeneRecords = pawn.AnimalGenetics()?.Mother?.GeneRecords;
        var fatherGeneRecords = pawn.AnimalGenetics()?.Father?.GeneRecords;

        var stats = Constants.affectedStats.Where(stat => stat != AnimalGenetics.GatherYield || Genes.Gatherable(pawn));
        foreach (var stat in stats)
        {
            var statRecord = statsGroup[stat];
            var motherStatRecord = motherGeneRecords?.TryGetValue(stat);
            var fatherStatRecord = fatherGeneRecords?.TryGetValue(stat);

            if (motherStatRecord != null)
            {
                Utility.GUI.DrawGeneValueLabel(new Rect(rect.x + (rect.width * 0.6f), curY, rect.width * 0.2f, 20f),
                    motherStatRecord.Value, statRecord.Parent != GeneRecord.Source.Mother);
            }

            if (fatherStatRecord != null)
            {
                Utility.GUI.DrawGeneValueLabel(new Rect(rect.x + (rect.width * 0.8f), curY, rect.width * 0.2f, 20f),
                    fatherStatRecord.Value, statRecord.Parent != GeneRecord.Source.Father);
            }

            curY += 20f;
        }
    }

    private static void DrawSingleParentData(Rect rect, float curY, Pawn pawn)
    {
        Text.Anchor = TextAnchor.MiddleCenter;

        var rectParent = new Rect(rect.x + (rect.width * 0.6f), curY, rect.width * 0.2f, 20f);
        Widgets.Label(rectParent, "AG.Parent".Translate());
        TooltipHandler.TipRegion(rectParent, "AG.ParentTooltop".Translate());

        curY += 20f;

        var motherGeneRecords = pawn.AnimalGenetics()?.Mother?.GeneRecords;
        var fatherGeneRecords = pawn.AnimalGenetics()?.Father?.GeneRecords;

        var stats = Constants.affectedStats.Where(stat => stat != AnimalGenetics.GatherYield || Genes.Gatherable(pawn));
        foreach (var stat in stats)
        {
            var statRecord = pawn.GetGeneRecord(stat);
            if (statRecord.Parent != GeneRecord.Source.None)
            {
                var extra = Genes.GetGenderSymbol(statRecord.Parent);

                var record = statRecord.Parent == GeneRecord.Source.Mother
                    ? motherGeneRecords?[stat]
                    : fatherGeneRecords?[stat];

                if (record != null)
                {
                    Utility.GUI.DrawGeneValueLabel(new Rect(rect.x + (rect.width * 0.6f), curY, rect.width * 0.2f, 20f),
                        record.Value, false, extra);
                }
            }

            curY += 20f;
        }
    }
}