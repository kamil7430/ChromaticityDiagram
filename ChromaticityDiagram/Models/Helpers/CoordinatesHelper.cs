using Avalonia;
using ScottPlot;

namespace ChromaticityDiagram.Models.Helpers;

public static class CoordinatesHelper
{
    public static Coordinates ToCoordinates(this Point p)
        => new Coordinates(p.X, p.Y);

    public static Coordinates XYZToxy(this Vector3D vec)
        => new Coordinates(vec.X / (vec.X + vec.Y + vec.Z), vec.Y / (vec.X + vec.Y + vec.Z));

    public static Coordinates XYZTosRGBxy(this Vector3D vec)
        => new Coordinates(vec.X / (vec.X + vec.Y + vec.Z), vec.Y / (vec.X + vec.Y + vec.Z)).ClampTosRGB();
}