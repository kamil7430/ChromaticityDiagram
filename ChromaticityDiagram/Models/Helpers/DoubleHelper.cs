namespace ChromaticityDiagram.Models.Helpers;

public static class DoubleHelper
{
    public static double TruncateToZeroOne(this double x)
        => x switch
        {
            < 0 => 0,
            > 1 => 1,
            _ => x
        };
}