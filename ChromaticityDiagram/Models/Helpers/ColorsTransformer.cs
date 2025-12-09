using Avalonia;
using ScottPlot;

namespace ChromaticityDiagram.Models.Helpers;

public static class ColorsTransformer
{
    public static Color XYZToColor(this Vector3D vec)
    {
        // https://stackoverflow.com/questions/43494018/converting-xyz-color-to-rgb
        var r = (3.2404542 * vec.X - 1.5371385 * vec.Y - 0.4985314 * vec.Z).TruncateToZeroOne();
        var g = (-0.9692660 * vec.X + 1.8760108 * vec.Y + 0.0415560 * vec.Z).TruncateToZeroOne();
        var b = (0.0556434 * vec.X - 0.2040259 * vec.Y + 1.0572252 * vec.Z).TruncateToZeroOne();

        uint argb = 0;
        argb |= (uint)(r * 255) << 16;
        argb |= (uint)(g * 255) << 8;
        argb |= (uint)(b * 255);
        
        return Color.FromARGB(argb);
    }
}