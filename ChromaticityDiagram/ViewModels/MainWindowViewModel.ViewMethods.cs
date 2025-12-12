using System.Collections.Generic;
using System.Linq;
using ChromaticityDiagram.Models.Algorithms;
using ChromaticityDiagram.Models.Helpers;
using ScottPlot;

namespace ChromaticityDiagram.ViewModels;

public partial class MainWindowViewModel
{
    public (double[] xs, double[] ys) GetBezierCurve()
        => DeCasteljau.GetBezierPoints(BezierCurveControlPoints.ToList());

    public IEnumerable<(Coordinates coordinates, Color color)> GetChromaticityDiagramEdgePoints()
        => ColorMatching.WaveLengthsToXYZ.Values.Select(vec =>
        (
            new Coordinates(vec.X / (vec.X + vec.Y + vec.Z), vec.Y / (vec.X + vec.Y + vec.Z)),
            vec.XYZToColor()
        )).Where(t => t.Item2 != Colors.Black);

    public IDictionary<int, double> GetBezierValues(double[] xs, double[] ys)
    {
        var xMin = xs.Min();
        var xMax = xs.Max();

        Dictionary<int, double> values = [];

        for (int i = 0; i < xs.Length - 1; i++)
        {
            var xStart = xs[i];
            var xEnd = xs[i + 1];
            var yStart = ys[i];
            var yEnd = ys[i + 1];

            // Jeżeli rozważany fragment wykresu jest rysowany "od prawej", to chcemy to odwrócić
            if (xStart > xEnd)
                (xStart, xEnd, yStart, yEnd) = (xEnd, xStart, yEnd, yStart);

            var xStartInt = (int)xStart;
            var xEndInt = (int)xEnd;

            if (xStartInt == xEndInt)
                values[xStartInt] = (yStart + yEnd) / 2;
            else
            {
                
            }
        }
        
        return values;
    }
}