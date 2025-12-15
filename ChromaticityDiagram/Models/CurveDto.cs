using System.Collections.Generic;
using ScottPlot;

namespace ChromaticityDiagram.Models;

public record CurveDto(List<Coordinates> BezierCurveControlPoints, CurveType CurveType)
{ }