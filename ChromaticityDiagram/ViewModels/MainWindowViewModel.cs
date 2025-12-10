using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Platform;
using ChromaticityDiagram.Models.Helpers;
using ScottPlot;
using SkiaSharp;

namespace ChromaticityDiagram.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel()
    {
        var uri = new Uri("avares://ChromaticityDiagram/Assets/chromaticity-diagram.png");
        using var stream = AssetLoader.Open(uri);
        CIEXYZDiagramBackground = new Image(SKBitmap.Decode(stream));
    }
    
    public IEnumerable<(Coordinates coordinates, Color color)> GetChromaticityDiagramEdgePoints()
        => ColorMatching.WaveLengthsToXYZ.Values.Select(vec =>
            (
                new Coordinates(vec.X / (vec.X + vec.Y + vec.Z), vec.Y / (vec.X + vec.Y + vec.Z)),
                vec.XYZToColor()
            ));
}