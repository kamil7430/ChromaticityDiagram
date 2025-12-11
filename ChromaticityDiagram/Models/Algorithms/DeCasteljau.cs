using System.Collections.Generic;
using ScottPlot;

namespace ChromaticityDiagram.Models.Algorithms;

public static class DeCasteljau
{
    public static (double[] xs, double[] ys) GetBezierPoints(List<Coordinates> controlPoints, int steps = 100)
    {
        int n = controlPoints.Count;

        if (n < 2) 
            return ([], []);

        // Tablice z wynikowymi współrzędnymi
        var xs = new double[steps + 1];
        var ys = new double[steps + 1];
        
        // Bufory tymczasowe, żeby nie alokować pamięci w pętli
        var tempX = new double[n];
        var tempY = new double[n];

        for (int s = 0; s <= steps; s++)
        {
            double t = (double)s / steps;

            // Kopiujemy punkty startowe do bufora
            for (int i = 0; i < n; i++)
            {
                tempX[i] = controlPoints[i].X;
                tempY[i] = controlPoints[i].Y;
            }

            // Algorytm De Casteljau (redukcja trójkątna)
            // Zmniejszamy liczbę punktów o 1 w każdej iteracji, aż zostanie jeden
            for (int k = 1; k < n; k++)
            {
                for (int i = 0; i < n - k; i++)
                {
                    // Liniowa interpolacja między sąsiednimi punktami
                    tempX[i] = (1 - t) * tempX[i] + t * tempX[i + 1];
                    tempY[i] = (1 - t) * tempY[i] + t * tempY[i + 1];
                }
            }

            // Ostatni punkt w buforze to nasz wynik dla danego t
            xs[s] = tempX[0];
            ys[s] = tempY[0];
        }
        
        return (xs, ys);
    }
}