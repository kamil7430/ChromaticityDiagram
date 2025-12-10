using ScottPlot;

namespace ChromaticityDiagram.ViewModels;

public partial class MainWindowViewModel
{
    public static int MinPointsCount => 0;
    public static int MaxPointsCount => 30;

    public static Coordinates[] SRGBGamut
        => [
            new Coordinates(0.64, 0.33),
            new Coordinates(0.30, 0.60),
            new Coordinates(0.15, 0.06)
        ];
}