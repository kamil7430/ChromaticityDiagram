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

    private void NotifyCanExecute()
    {
        RemoveLastControlPointCommand.NotifyCanExecuteChanged();
    }
}