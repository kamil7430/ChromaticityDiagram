using System.Linq;
using ChromaticityDiagram.Models.Algorithms;

namespace ChromaticityDiagram.ViewModels;

public partial class MainWindowViewModel
{
    public (double[] xs, double[] ys) GetBezierCurve()
        => DeCasteljau.GetBezierPoints(BezierCurveControlPoints.ToList());
}