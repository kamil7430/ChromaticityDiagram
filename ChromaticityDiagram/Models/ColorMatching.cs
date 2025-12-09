using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Platform;

namespace ChromaticityDiagram.Models;

public class ColorMatching
{
    private const string FILE_PATH = "ChromaticityDiagram/Assets/color_matching_functions.txt";
    
    public int MinWaveLength { get; }
    public int MaxWaveLength { get; }
    
    public IDictionary<int, Vector3D> WaveLengthsToXYZ { get; }
    
    public ColorMatching()
    {
        var uri = new Uri($"avares://{FILE_PATH}");
        if (!AssetLoader.Exists(uri))
            throw new FileNotFoundException("Nie znaleziono zasobu: " + uri);
        
        using var stream = AssetLoader.Open(uri);
        using var reader = new StreamReader(stream);

        Dictionary<int, Vector3D> waveLengthsToXYZ = [];
        
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine() ?? throw new InvalidDataException();
            var splitLine = line.Split('\t');
            
            var wl = int.Parse(splitLine[0], CultureInfo.InvariantCulture);
            var x = float.Parse(splitLine[1], CultureInfo.InvariantCulture);
            var y = float.Parse(splitLine[2], CultureInfo.InvariantCulture);
            var z = float.Parse(splitLine[3], CultureInfo.InvariantCulture);
            
            waveLengthsToXYZ.Add(wl, new Vector3D(x, y, z));
        }

        MinWaveLength = waveLengthsToXYZ.Keys.Min();
        MaxWaveLength = waveLengthsToXYZ.Keys.Max();
        WaveLengthsToXYZ = waveLengthsToXYZ;
    }
}