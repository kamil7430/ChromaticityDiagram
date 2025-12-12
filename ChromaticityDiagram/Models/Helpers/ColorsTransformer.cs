using System;
using Avalonia;

namespace ChromaticityDiagram.Models.Helpers;

public static class ColorsTransformer
{
    public static uint XYZToColorUint(this Vector3D vec)
    {
        // https://stackoverflow.com/questions/43494018/converting-xyz-color-to-rgb
        var r = 3.2404542 * vec.X - 1.5371385 * vec.Y - 0.4985314 * vec.Z;
        var g = -0.9692660 * vec.X + 1.8760108 * vec.Y + 0.0415560 * vec.Z;
        var b = 0.0556434 * vec.X - 0.2040259 * vec.Y + 1.0572252 * vec.Z;

        // Normalization
        var max = Math.Max(Math.Max(r, g), b);
        if (max > 1.0)
        {
            r /= max;
            g /= max;
            b /= max;
        }
        
        // Gamma correction
        r = GammaCorrection(Math.Max(0, r));
        g = GammaCorrection(Math.Max(0, g));
        b = GammaCorrection(Math.Max(0, b));
        
        uint argb = 0xFF000000;
        argb |= (uint)(r * 255) << 16;
        argb |= (uint)(g * 255) << 8;
        argb |= (uint)(b * 255);
        
        return argb;
    }
    
    private static double GammaCorrection(double c)
    {
        if (c <= 0.0031308) return 12.92 * c;
        return 1.055 * Math.Pow(c, 1.0 / 2.4) - 0.055;
    }
}