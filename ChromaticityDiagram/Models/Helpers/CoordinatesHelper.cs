using Avalonia;
using ScottPlot;

namespace ChromaticityDiagram.Models.Helpers;

public static class CoordinatesHelper
{
    public static Coordinates ToCoordinates(this Point p)
        => new Coordinates(p.X, p.Y);
}