using System.Collections.Generic;
using System.Linq;
using ChromaticityDiagram.Models.Helpers;
using ScottPlot;

namespace ChromaticityDiagram.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public IEnumerable<(Coordinates coordinates, Color color)> GetChromaticityDiagramEdgePoints()
        => ColorMatching.WaveLengthsToXYZ.Values.Select(vec =>
            (
                new Coordinates(vec.X / (vec.X + vec.Y + vec.Z), vec.Y / (vec.X + vec.Y + vec.Z)),
                vec.XYZToColor()
            ));
}