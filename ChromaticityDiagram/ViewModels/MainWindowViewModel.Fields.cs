using System.Collections.ObjectModel;
using ChromaticityDiagram.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using ScottPlot;

namespace ChromaticityDiagram.ViewModels;

public partial class MainWindowViewModel
{
    public ColorMatching ColorMatching { get; } = new();

    public ObservableCollection<Coordinates> BezierCurveControlPoints { get; } = [];
    
    public Image CIEXYZDiagramBackground { get; }

    [ObservableProperty]
    private bool _shouldPaintAreaUnderBezierCurve;
}