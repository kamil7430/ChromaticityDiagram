using System;
using ScottPlot;

namespace ChromaticityDiagram.Models.Helpers;

public static class GamutHelper
{
    // Wierzchołki sRGB
    private static readonly Coordinates R = new(0.64, 0.33);
    private static readonly Coordinates G = new(0.30, 0.60);
    private static readonly Coordinates B = new(0.15, 0.06);

    // Punkt bieli
    public static readonly Coordinates WhitePoint = new(0.3127, 0.3290);
    
    /// <summary>
    /// Ogranicza punkt do trójkąta rzutując go od środka (bieli).
    /// Zachowuje odcień, poświęca nasycenie.
    /// </summary>
    public static Coordinates ClampTosRGB(this Coordinates p)
    {
        if (IsInsideTriangle(p, R, G, B))
            return p;

        // Szukamy punktu przecięcia linii "Biały -> Kursor" z krawędziami trójkąta (RG, GB, BR).
        // Tylko jedna przetnie się w dobrą stronę
        var intersectRG = GetRaySegmentIntersection(WhitePoint, p, R, G);
        if (intersectRG.HasValue) return intersectRG.Value;

        var intersectGB = GetRaySegmentIntersection(WhitePoint, p, G, B);
        if (intersectGB.HasValue) return intersectGB.Value;

        var intersectBR = GetRaySegmentIntersection(WhitePoint, p, B, R);
        if (intersectBR.HasValue) return intersectBR.Value;

        // Fallback (teoretycznie niemożliwy, jeśli geometria jest poprawna)
        return R; 
    }

    private static bool IsInsideTriangle(Coordinates p, Coordinates a, Coordinates b, Coordinates c)
    {
        var as_x = p.X - a.X;
        var as_y = p.Y - a.Y;
        bool s_ab = (b.X - a.X) * as_y - (b.Y - a.Y) * as_x > 0;

        if ((c.X - a.X) * as_y - (c.Y - a.Y) * as_x > 0 == s_ab) 
            return false;
        if ((c.X - b.X) * (p.Y - b.Y) - (c.Y - b.Y) * (p.X - b.X) > 0 != s_ab) 
            return false;
        
        return true;
    }

    /// <summary>
    /// Oblicza punkt przecięcia półprostej (start -> directionPoint) z odcinkiem (p1-p2).
    /// Zwraca null, jeśli nie ma przecięcia.
    /// </summary>
    private static Coordinates? GetRaySegmentIntersection(Coordinates start, Coordinates directionPoint, Coordinates p1, Coordinates p2)
    {
        double x1 = p1.X, y1 = p1.Y;
        double x2 = p2.X, y2 = p2.Y;
        double x3 = start.X, y3 = start.Y;
        double x4 = directionPoint.X, y4 = directionPoint.Y;

        double den = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);
        
        // Linie równoległe
        if (Math.Abs(den) < 1e-9) 
            return null;

        double t = ((x1 - x3) * (y3 - y4) - (y1 - y3) * (x3 - x4)) / den;
        double u = -((x1 - x2) * (y1 - y3) - (y1 - y2) * (x1 - x3)) / den;

        // t musi być w zakresie [0, 1] (czyli przecięcie jest na odcinku p1-p2)
        // u musi być > 0 (czyli przecięcie jest "przed nami", w stronę kursora, a nie za plecami)
        if (t >= 0 && t <= 1 && u > 0)
        {
            return new Coordinates(x1 + t * (x2 - x1), y1 + t * (y2 - y1));
        }

        return null;
    }
}