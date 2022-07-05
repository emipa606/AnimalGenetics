using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AnimalGenetics;

internal class MultiLerp
{
    private readonly KeyValuePair<float, Color>[] _Points;

    public MultiLerp(KeyValuePair<float, Color>[] points)
    {
        _Points = points;
    }

    public Color Apply(float value)
    {
        if (value < _Points.First().Key)
        {
            return _Points.First().Value;
        }

        if (value > _Points.Last().Key)
        {
            return _Points.Last().Value;
        }

        var lhs = _Points.LastOrDefault(point => value >= point.Key);
        var rhs = _Points.FirstOrDefault(point => value < point.Key);

        return Color.Lerp(lhs.Value, rhs.Value, (value - lhs.Key) / (rhs.Key - lhs.Key));
    }
}