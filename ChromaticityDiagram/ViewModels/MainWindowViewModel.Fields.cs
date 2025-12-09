using System.Collections.ObjectModel;
using ChromaticityDiagram.Models;
using ScottPlot;

namespace ChromaticityDiagram.ViewModels;

public partial class MainWindowViewModel
{
    public ColorMatching ColorMatching { get; } = new();

    public ObservableCollection<Coordinates> BezierCurveControlPoints { get; } = [];
}