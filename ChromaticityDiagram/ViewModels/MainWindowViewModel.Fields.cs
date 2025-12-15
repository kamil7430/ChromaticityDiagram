using System.Collections.ObjectModel;
using ChromaticityDiagram.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using ScottPlot;

namespace ChromaticityDiagram.ViewModels;

public partial class MainWindowViewModel
{
    public ColorMatching ColorMatching { get; } = new();

    [ObservableProperty]
    private ObservableCollection<Coordinates> _bezierCurveControlPoints = [];
    
    public Image CIEXYZDiagramBackground { get; }

    [ObservableProperty]
    private bool _shouldPaintAreaUnderBezierCurve;
    
    [ObservableProperty]
    private CurveType _curveType = CurveType.BezierCurve;
}