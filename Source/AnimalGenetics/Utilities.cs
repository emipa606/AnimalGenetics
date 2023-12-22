using System.Collections.Generic;
using UnityEngine;

namespace AnimalGenetics;

public static class Utilities
{
    private static readonly KeyValuePair<float, Color>[] pointsRPG =
    [
        new KeyValuePair<float, Color>(0.80f, Color.gray),
        new KeyValuePair<float, Color>(0.95f, Color.white),
        new KeyValuePair<float, Color>(1.05f, new Color(0.1f, 0.7f, 0.1f)),
        new KeyValuePair<float, Color>(1.20f, new Color(0.3f, 0.3f, 1.0f)),
        new KeyValuePair<float, Color>(1.40f, new Color(0.5f, 0.2f, 0.7f)),
        new KeyValuePair<float, Color>(1.60f, new Color(1.0f, 0.6f, 0.1f)),
        new KeyValuePair<float, Color>(1.80f, Color.yellow)
    ];

    private static readonly KeyValuePair<float, Color>[] pointsNormal =
    [
        new KeyValuePair<float, Color>(0.5f, new Color(0.9f, 0f, 0f)),
        new KeyValuePair<float, Color>(1.0f, Color.yellow),
        new KeyValuePair<float, Color>(1.70f, Color.green)
    ];

    private static readonly List<KeyValuePair<float, Color>[]> colorProfiles =
    [
        pointsNormal,
        pointsRPG
    ];

    public static Color TextColor(float mod)
    {
        var points = colorProfiles[Settings.UI.colorMode];
        var ml = new MultiLerp(points);

        return ml.Apply(mod);
    }
}