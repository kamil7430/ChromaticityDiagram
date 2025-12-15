using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using ChromaticityDiagram.Models;
using CommunityToolkit.Mvvm.Input;

namespace ChromaticityDiagram.ViewModels;

public partial class MainWindowViewModel
{
    private bool IsControlPointsCountPositive
        => BezierCurveControlPoints.Count > 0;
    
    [RelayCommand(CanExecute = nameof(IsControlPointsCountPositive))]
    private void RemoveLastControlPoint()
    {
        BezierCurveControlPoints.RemoveAt(BezierCurveControlPoints.Count - 1);
        OnPropertyChanged();
    }

    [RelayCommand]
    private async Task SaveCurve()
    {
        var file = await _storageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "Save as...",
            SuggestedFileName = "points.json",
            DefaultExtension = "json"
        });

        if (file is null) 
            return;
        
        await using var stream = await file.OpenWriteAsync();
        await using var streamWriter = new StreamWriter(stream);
        await streamWriter.WriteAsync(SerializeSelf());
    }

    private string SerializeSelf()
    {
        var dto = new CurveDto(
            BezierCurveControlPoints.ToList(),
            CurveType
        );

        return JsonSerializer.Serialize(dto);
    }

    [RelayCommand]
    private async Task LoadCurve()
    {
        var files = await _storageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Open file...",
            AllowMultiple = false
        });

        if (files.Count < 1)
            return;
        
        await using var stream = await files[0].OpenReadAsync();
        using var streamReader = new StreamReader(stream);
        var fileContent = await streamReader.ReadToEndAsync();

        DeserializeSelf(fileContent);
    }

    private bool DeserializeSelf(string json)
    {
        try
        {
            var dto = JsonSerializer.Deserialize<CurveDto>(json);
            BezierCurveControlPoints = [.. dto.BezierCurveControlPoints];
            CurveType = dto.CurveType;
            return true;
        }
        catch
        {
            return false;
        }
    }

    private void NotifyCanExecute()
    {
        RemoveLastControlPointCommand.NotifyCanExecuteChanged();
    }
}