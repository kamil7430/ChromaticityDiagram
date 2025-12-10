using System;
using Avalonia.Controls;
using Avalonia.Input;
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
    private Scatter _bezierScatter;
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
        
        _bezierScatter = plot.Plot.Add.Scatter(Array.Empty<Coordinates>());
        _bezierScatter.LineWidth = 2;
        _bezierScatter.MarkerSize = 15;
        
        plot.Plot.Axes.SetLimits(380, 780, 0, 2);
        plot.Interaction.Disable();
        plot.PointerPressed += OnBezierPlotPointerPressed;
        plot.PointerMoved += OnBezierPlotPointerMoved;
        plot.PointerReleased += OnBezierPlotPointerReleased;
        
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