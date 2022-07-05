using UnityEngine;
using Verse;

namespace AnimalGenetics.Utility;

internal class GUI
{
    public static void DrawGeneValueLabel(Rect box, float value, bool strikethrough = false, string extra = "")
    {
        var oldTextAnchor = Text.Anchor;
        var oldColor = UnityEngine.GUI.color;

        Text.Anchor = TextAnchor.MiddleCenter;
        UnityEngine.GUI.color = Utilities.TextColor(value);

        var text = $"{value * 100:F0}%{extra}";
        Widgets.Label(box, text);

        if (strikethrough)
        {
            var halfSize = (Text.CalcSize(text).x / 2) + 1;
            var midpoint = (box.xMin + box.xMax) / 2;

            Widgets.DrawLine(new Vector2(midpoint - halfSize, box.y + (box.height / 2) - 1),
                new Vector2(midpoint + halfSize, box.y + (box.height / 2) - 1), new Color(1f, 1f, 1f, 0.5f), 1);
        }

        UnityEngine.GUI.color = oldColor;
        Text.Anchor = oldTextAnchor;
    }
}