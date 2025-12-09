using System;
using Avalonia.Controls;
using Avalonia.Input;
using ScottPlot.Avalonia;
using ScottPlot.Plottables;

namespace ChromaticityDiagram.Views;

public partial class MainWindow : Window
{
    private readonly AvaPlot _bezierPlot;
    private Scatter _bezierScatter;
    private int _draggedPointIndex = -1;
    
    public MainWindow()
    {
        InitializeComponent();

        _bezierPlot = this.Find<AvaPlot>("BezierDiagram")!;
        
        _bezierScatter = _bezierPlot.Plot.Add.Scatter([], []);
        _bezierScatter.LineWidth = 2;
        _bezierScatter.MarkerSize = 15;
        
        _bezierPlot.Plot.Axes.SetLimits(380, 780, 0, 2);
        
        _bezierPlot.Interaction.Disable();

        _bezierPlot.PointerPressed += OnBezierPlotPointerPressed;
        _bezierPlot.PointerMoved += OnBezierPlotPointerMoved;
        _bezierPlot.PointerReleased += OnBezierPlotPointerReleased;
        
        _bezierPlot.Refresh();
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