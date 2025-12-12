using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Media;
using ChromaticityDiagram.Models.Algorithms;
using ChromaticityDiagram.Models.Helpers;
using ScottPlot;
using Color = ScottPlot.Color;
using Colors = ScottPlot.Colors;

namespace ChromaticityDiagram.ViewModels;

public partial class MainWindowViewModel
{
    public (double[] xs, double[] ys) GetBezierCurve()
        => DeCasteljau.GetBezierPoints(BezierCurveControlPoints.ToList());

    public IEnumerable<(Coordinates coordinates, Color color)> GetChromaticityDiagramEdgePoints()
        => ColorMatching.WaveLengthsToXYZ.Values.Select(vec =>
        (
            vec.XYZToxy(),
            Color.FromARGB(vec.XYZToColorUint())
        )).Where(t => t.Item2 != Colors.Black);

    public IDictionary<int, double> GetBezierValues(double[] xs, double[] ys)
    {
        if (xs.Length != ys.Length)
            throw new ArgumentException("Arrays xs and ys must have the same length!");
        
        Dictionary<int, double> values = [];

        for (int i = 0; i < xs.Length - 1; i++)
        {
            var xStart = xs[i];
            var xEnd = xs[i + 1];
            var yStart = ys[i];
            var yEnd = ys[i + 1];

            if (xStart > xEnd)
                (xStart, xEnd, yStart, yEnd) = (xEnd, xStart, yEnd, yStart);

            var xStartInt = (int)xStart;
            var xEndInt = (int)xEnd;

            if (xStartInt == xEndInt)
                values[xStartInt] = (yStart + yEnd) / 2;
            else
            {
                var steps = xEndInt - xStartInt;
                for (int s = 0; s <= steps; s++)
                {
                    var t = (double)s / steps;
                    values[xStartInt + s] = (1 - t) * yStart + t * yEnd;
                }
            }
        }
        
        return values;
    }

    public (IBrush, Coordinates) CalculatePointOnChromaticityDiagramAndGetColorPreview(IDictionary<int, double> bezierValues)
    {
        if (bezierValues.Count < 2)
            return (new SolidColorBrush(Avalonia.Media.Colors.White), GamutHelper.WhitePoint);
        
        double sumX = 0, sumY = 0, sumZ = 0;
        
        foreach (var (x, y) in bezierValues)
        {
            sumX += y * ColorMatching.WaveLengthsToXYZ[x].X;
            sumY += y * ColorMatching.WaveLengthsToXYZ[x].Y;
            sumZ += y * ColorMatching.WaveLengthsToXYZ[x].Z;
        }

        var vector = new Vector3D(sumX, sumY, sumZ);

        return (new SolidColorBrush(vector.XYZToColorUint()), vector.XYZTosRGBxy());
    }
}