using System.Collections.ObjectModel;
using Avalonia;

namespace ChromaticityDiagram.ViewModels;

public partial class MainWindowViewModel
{
    public ObservableCollection<Point> BezierCurveControlPoints { get; } = [];
}