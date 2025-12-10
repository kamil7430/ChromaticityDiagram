using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Platform;
using ChromaticityDiagram.ViewModels;
using ScottPlot;
using ScottPlot.Avalonia;
using ScottPlot.Plottables;
using SkiaSharp;
using Image = ScottPlot.Image;

namespace ChromaticityDiagram.Views;

public partial class MainWindow : Window
{
    private const float POINT_SIZE = 5f;
    
    private readonly MainWindowViewModel _viewModel;
    
    private readonly AvaPlot _bezierPlot;
    private readonly AvaPlot _chromaticityPlot;
    private Scatter _bezierScatter;
    private int _draggedPointIndex = -1;
    
    public MainWindow(MainWindowViewModel viewModel)
    {
        InitializeComponent();
        
        _viewModel = viewModel;

        _bezierPlot = this.Find<AvaPlot>("BezierDiagram")!;
        
        _bezierScatter = _bezierPlot.Plot.Add.Scatter(Array.Empty<Coordinates>());
        _bezierScatter.LineWidth = 2;
        _bezierScatter.MarkerSize = 15;
        
        _bezierPlot.Plot.Axes.SetLimits(380, 780, 0, 2);
        
        _bezierPlot.Interaction.Disable();
        _bezierPlot.PointerPressed += OnBezierPlotPointerPressed;
        _bezierPlot.PointerMoved += OnBezierPlotPointerMoved;
        _bezierPlot.PointerReleased += OnBezierPlotPointerReleased;
        
        _bezierPlot.Refresh();
        
        _chromaticityPlot = this.Find<AvaPlot>("ChromaticityDiagram")!;
        _chromaticityPlot.Plot.Add.ImageRect(
            _viewModel.CIEXYZDiagramBackground,
            new CoordinateRect(0, 0.8, 0, 0.9)
        );
        foreach (var (coordinates, color) in _viewModel.GetChromaticityDiagramEdgePoints())
            _chromaticityPlot.Plot.Add.Marker(coordinates, size: POINT_SIZE, color: color);
        _chromaticityPlot.Plot.Axes.SetLimits(0, 1, 0, 1);
        _chromaticityPlot.Interaction.Disable();
        _chromaticityPlot.Refresh();
    }
    
    private void OnBezierPlotPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void OnBezierPlotPointerMoved(object? sender, PointerEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void OnBezierPlotPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        throw new NotImplementedException();
    }
}