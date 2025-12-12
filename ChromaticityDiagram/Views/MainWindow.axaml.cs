using System;
using System.Collections.Generic;
using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using ChromaticityDiagram.ViewModels;
using ScottPlot;
using ScottPlot.Avalonia;

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
        DataContext = _viewModel = viewModel;
        _bezierPlot = InitializeBezierPlot();
        _chromaticityPlot = InitializeChromaticityDiagram();
        _viewModel.PropertyChanged += ViewModel_OnPropertyChanged;
    }

    private void ViewModel_OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        => RenderPlots();

    private AvaPlot InitializeBezierPlot()
    {
        var plot = this.Find<AvaPlot>("BezierDiagram")!;
        
        plot.PointerPressed += OnBezierPlotPointerPressed;
        plot.PointerMoved += OnBezierPlotPointerMoved;
        plot.PointerReleased += OnBezierPlotPointerReleased;
        plot.Plot.Axes.SetLimits(380, 780, 0, 2);
        plot.Interaction.Disable();
        
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
        const float catchRadius = 7f;
        
        var clickCoordinates = MousePointToPlotCoordinates(e.GetPosition(_bezierPlot), _bezierPlot);
        if (!clickCoordinates.HasValue)
            return;

        for (int i = 0; i < _viewModel.BezierCurveControlPoints.Count; i++)
        {
            var coords = _viewModel.BezierCurveControlPoints[i];

            if (Math.Abs(clickCoordinates.Value.X - coords.X) < catchRadius &&
                Math.Abs(clickCoordinates.Value.Y - coords.Y) < catchRadius)
            {
                _draggedPointIndex = i;
                return;
            }
        }
    }

    private void OnBezierPlotPointerMoved(object? sender, PointerEventArgs e)
    {
        if (_draggedPointIndex == -1)
            return;
        
        var coordinates = MousePointToPlotCoordinates(e.GetPosition(_bezierPlot), _bezierPlot);
        if (!coordinates.HasValue)
            return;

        _viewModel.BezierCurveControlPoints[_draggedPointIndex] = coordinates.Value;
        RenderPlots();
    }

    private void OnBezierPlotPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        if (_draggedPointIndex == -1)
        {
            var coordinates = MousePointToPlotCoordinates(e.GetPosition(_bezierPlot), _bezierPlot);
            if (coordinates.HasValue)
                _viewModel.BezierCurveControlPoints.Add(coordinates.Value);
        }
        else
            _draggedPointIndex = -1;
        
        RenderPlots();
    }

    private Coordinates? MousePointToPlotCoordinates(Point mousePoint, AvaPlot plot)
    {
        var coordinates = plot.Plot.GetCoordinates((float)mousePoint.X, (float)mousePoint.Y);

        var limits = plot.Plot.Axes.GetLimits().Rect;
        if (coordinates.X < limits.Left || coordinates.X > limits.Right || coordinates.Y < limits.Bottom ||
            coordinates.Y > limits.Top)
            return null;
        
        return coordinates;
    }

    private void RenderPlots()
    {
        var (xs, ys) = _viewModel.GetBezierCurve();
        var bezierValues = _viewModel.GetBezierValues(xs, ys);
        RenderBezierPlot(xs, ys);
        PaintAreaUnderBezierCurve(bezierValues);
        UpdateColorPointOnChromaticityDiagram(bezierValues);
        UpdateColorPreview(bezierValues);
        
        _bezierPlot.Refresh();
        _chromaticityPlot.Refresh();
    }
    
    private void RenderBezierPlot(double[] xs, double[] ys)
    {
        const float controlPointSize = 10f;
        var controlPointColor = Colors.Black;
        var curveColor = Colors.Black;

        _bezierPlot.Plot.Clear();
        
        // Control points
        foreach (var coordinates in _viewModel.BezierCurveControlPoints)
            _bezierPlot.Plot.Add.Marker(coordinates, size: controlPointSize, color: controlPointColor);
        
        // Curve
        _bezierPlot.Plot.Add.ScatterLine(xs, ys, curveColor);
    }

    private void PaintAreaUnderBezierCurve(IDictionary<int, double> bezierValues)
    {
        if (!_viewModel.ShouldPaintAreaUnderBezierCurve)
            return;

        foreach (var (x, y) in bezierValues)
            _bezierPlot.Plot.Add.Line(x, 0, x, y);
    }
    
    private void UpdateColorPointOnChromaticityDiagram(IDictionary<int, double> bezierValues)
    {
        
    }

    private void UpdateColorPreview(IDictionary<int, double> bezierValues)
    {
        
    }
}