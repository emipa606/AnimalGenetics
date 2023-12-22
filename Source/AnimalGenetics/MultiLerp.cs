using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AnimalGenetics;

internal class MultiLerp(KeyValuePair<float, Color>[] points)
{
    public Color Apply(float value)
    {
        if (value < points.First().Key)
        {
            return points.First().Value;
        }

        if (value > points.Last().Key)
        {
            return points.Last().Value;
        }

        var lhs = points.LastOrDefault(point => value >= point.Key);
        var rhs = points.FirstOrDefault(point => value < point.Key);

        return Color.Lerp(lhs.Value, rhs.Value, (value - lhs.Key) / (rhs.Key - lhs.Key));
    }
}