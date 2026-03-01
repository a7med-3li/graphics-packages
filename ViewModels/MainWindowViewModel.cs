using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AvaloniaApplication1.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    // Event to notify view when line is drawn
    public event Action<List<(float xExact, float yExact, int xRounded, int yRounded)>>? LineDrawn;

    [ObservableProperty]
    private string x1 = "0";

    [ObservableProperty]
    private string y1 = "0";

    [ObservableProperty]
    private string x2 = "100";

    [ObservableProperty]
    private string y2 = "100";

    [ObservableProperty]
    private ObservableCollection<string> linePoints = new();

    [ObservableProperty]
    private string errorMessage = string.Empty;

    [RelayCommand]
    private void DrawLine()
    {
        try
        {
            ErrorMessage = string.Empty;

            // Parse input values
            if (!int.TryParse(X1, out int x1Val) || !int.TryParse(Y1, out int y1Val) ||
                !int.TryParse(X2, out int x2Val) || !int.TryParse(Y2, out int y2Val))
            {
                ErrorMessage = "Please enter valid integers for all coordinates";
                return;
            }

            // Run DDA algorithm
            var points = CalculateDDALine(x1Val, y1Val, x2Val, y2Val);

            // Update the points list
            LinePoints.Clear();
            foreach (var point in points)
            {
                LinePoints.Add($"Exact: ({point.xExact:F2}, {point.yExact:F2}) -> Rounded: ({point.xRounded}, {point.yRounded})");
            }

            // Notify view to draw the line
            LineDrawn?.Invoke(points);
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error: {ex.Message}";
        }
    }

    /// <summary>
    /// DDA (Digital Differential Analyzer) Line Algorithm
    /// Calculates all points on a line between two endpoints
    /// Returns both exact float values and rounded integer values
    /// </summary>
    private List<(float xExact, float yExact, int xRounded, int yRounded)> CalculateDDALine(int x1, int y1, int x2, int y2)
    {
        var points = new List<(float, float, int, int)>();

        int dx = x2 - x1;
        int dy = y2 - y1;

        int steps = Math.Max(Math.Abs(dx), Math.Abs(dy));

        if (steps == 0)
        {
            points.Add((x1, y1, x1, y1));
            return points;
        }

        float xIncrement = dx / (float)steps;
        float yIncrement = dy / (float)steps;

        float x = x1;
        float y = y1;

        for (int i = 0; i <= steps; i++)
        {
            int xRounded = (int)Math.Round(x);
            int yRounded = (int)Math.Round(y);
            points.Add((x, y, xRounded, yRounded));
            x += xIncrement;
            y += yIncrement;
        }

        return points;
    }
}

