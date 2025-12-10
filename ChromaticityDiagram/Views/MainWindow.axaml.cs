using System;
using Avalonia.Controls;
using Avalonia.Input;
using ChromaticityDiagram.Models.Helpers;
using ChromaticityDiagram.ViewModels;
using ScottPlot;
using ScottPlot.Avalonia;
using ScottPlot.Plottables;

namespace ChromaticityDiagram.Views;

public partial class MainWindow : Window
{
    private readonly MainWindowViewModel _viewModel;
    
    private readonly AvaPlot _bezierPlot;
    private readonly AvaPlot _chromaticityPlot;
    private int _draggedPointIndex = -1;
    
    public MainWindow(MainWindowViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        _bezierPlot = InitializeBezierPlot();
        _chromaticityPlot = InitializeChromaticityDiagram();
    }

    private AvaPlot InitializeBezierPlot()
    {
        var plot = this.Find<AvaPlot>("BezierDiagram")!;
        
        plot.Interaction.Disable();
        plot.PointerPressed += OnBezierPlotPointerPressed;
        plot.PointerMoved += OnBezierPlotPointerMoved;
        plot.PointerReleased += OnBezierPlotPointerReleased;
        plot.Plot.Axes.SetLimits(380, 780, 0, 2);
        
        plot.Refresh();

        return plot;
    }
    
    private AvaPlot InitializeChromaticityDiagram()
    {
        const float colorPointSize = 5f;
        const float gamutPointSize = 10f;
        var gamutColor = Colors.Black;
        
        var plot = this.Find<AvaPlot>("ChromaticityDiagram")!;
        
        // Background
        plot.Plot.Add.ImageRect(
            _viewModel.CIEXYZDiagramBackground,
            new CoordinateRect(0, 0.8, 0, 0.9)
        );
        
        // Wave length points
        foreach (var (coordinates, color) in _viewModel.GetChromaticityDiagramEdgePoints())
            plot.Plot.Add.Marker(coordinates, size: colorPointSize, color: color);
        
        // sRGB gamut
        var vertices = MainWindowViewModel.SRGBGamut;
        var polygon = plot.Plot.Add.Polygon(vertices);
        polygon.FillColor = Colors.Transparent;
        polygon.LineColor = gamutColor;
        foreach (var v in vertices)
            plot.Plot.Add.Marker(v, size: gamutPointSize, color: gamutColor);
        
        plot.Plot.Axes.SetLimits(0, 1, 0, 1);
        plot.Interaction.Disable();
        plot.Refresh();
        
        return plot;
    }
    
    private void OnBezierPlotPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        
    }

    private void OnBezierPlotPointerMoved(object? sender, PointerEventArgs e)
    {
        
    }

    private void OnBezierPlotPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        if (_draggedPointIndex == -1)
        {
            var coordinates = e.GetPosition(_bezierPlot).ToCoordinates();

            var limits = _bezierPlot.Plot.Axes.GetLimits().Rect;
            var bounds = _bezierPlot.Bounds;
            coordinates.Y = (bounds.Height - coordinates.Y) * limits.Top / bounds.Height;
            coordinates.X = coordinates.X * (limits.Right - limits.Left) / bounds.Width + limits.Left;
            
            AddControlPoint(coordinates);
        }
        else
        {
            
        }
    }

    private void AddControlPoint(Coordinates coordinates)
    {
        const float controlPointSize = 10f;
        var controlPointColor = Colors.Black;
        
        _viewModel.BezierCurveControlPoints.Add(coordinates);
        _bezierPlot.Plot.Add.Marker(coordinates, size: controlPointSize, color: controlPointColor);
        
        // TODO: obliczyć krzywą
        _bezierPlot.Refresh();
    }
}