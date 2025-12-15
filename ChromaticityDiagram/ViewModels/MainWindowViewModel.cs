using System;
using System.Collections.Specialized;
using Avalonia.Platform;
using Avalonia.Platform.Storage;
using ScottPlot;
using SkiaSharp;

namespace ChromaticityDiagram.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly IStorageProvider _storageProvider;
    
    public MainWindowViewModel(IStorageProvider storageProvider)
    {
        _storageProvider = storageProvider;
        
        BezierCurveControlPoints.CollectionChanged += BezierCurveControlPoints_OnCollectionChanged;
        
        var uri = new Uri("avares://ChromaticityDiagram/Assets/chromaticity-diagram.png");
        using var stream = AssetLoader.Open(uri);
        CIEXYZDiagramBackground = new Image(SKBitmap.Decode(stream));
    }

    private void BezierCurveControlPoints_OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        => NotifyCanExecute();
}