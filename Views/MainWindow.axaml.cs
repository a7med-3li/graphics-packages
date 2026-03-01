using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using AvaloniaApplication1.ViewModels;
using System;
using System.Collections.Generic;

namespace AvaloniaApplication1.Views;

public partial class MainWindow : Window
{
    private Canvas? _canvas;
    private const float GridSpacing = 1; // pixels per unit
    private const int CanvasWidth = 600;
    private const int CanvasHeight = 500;
    private const int CenterX = CanvasWidth / 2;
    private const int CenterY = CanvasHeight / 2;

    public MainWindow()
    {
        InitializeComponent();

        // Get canvas reference after InitializeComponent
        this.Loaded += (s, e) =>
        {
            _canvas = this.FindControl<Canvas>("drawingCanvas");

            if (this.DataContext is MainWindowViewModel vm)
            {
                vm.LineDrawn += DrawLineOnCanvas;
            }
        };
    }

    private void DrawLineOnCanvas(List<(float xExact, float yExact, int xRounded, int yRounded)> points)
    {
        if (_canvas == null) return;

        // Clear previous drawings
        _canvas.Children.Clear();

        // Draw grid
        DrawGrid();

        // Draw axes
        DrawAxes();

        // Draw continuous line through all points
        if (points.Count > 0)
        {
            var polyline = new Polyline
            {
                Stroke = new SolidColorBrush(Colors.Red),
                StrokeThickness = 2
            };

            var pointCollection = new Avalonia.Collections.AvaloniaList<Avalonia.Point>();
            foreach (var point in points)
            {
                // Convert from unit circle coordinates to canvas coordinates
                float canvasX = CenterX + (point.xRounded * GridSpacing);
                float canvasY = CenterY - (point.yRounded * GridSpacing); // Y is inverted in canvas
                pointCollection.Add(new Avalonia.Point(canvasX, canvasY));
            }

            polyline.Points = pointCollection;
            _canvas.Children.Add(polyline);
        }
    }

    private void DrawGrid()
    {
        if (_canvas == null) return;

        var pen = new Pen(new SolidColorBrush(Colors.LightGray), 1);

        // Vertical lines
        for (int x = 0; x < CanvasWidth; x += (int)GridSpacing)
        {
            var line = new Line
            {
                StartPoint = new Avalonia.Point(x, 0),
                EndPoint = new Avalonia.Point(x, CanvasHeight),
                Stroke = new SolidColorBrush(Colors.LightGray),
                StrokeThickness = 0.5
            };
            _canvas.Children.Add(line);
        }

        // Horizontal lines
        for (int y = 0; y < CanvasHeight; y += (int)GridSpacing)
        {
            var line = new Line
            {
                StartPoint = new Avalonia.Point(0, y),
                EndPoint = new Avalonia.Point(CanvasWidth, y),
                Stroke = new SolidColorBrush(Colors.LightGray),
                StrokeThickness = 0.5
            };
            _canvas.Children.Add(line);
        }
    }

    private void DrawAxes()
    {
        if (_canvas == null) return;

        // X-axis
        var xAxis = new Line
        {
            StartPoint = new Avalonia.Point(0, CenterY),
            EndPoint = new Avalonia.Point(CanvasWidth, CenterY),
            Stroke = new SolidColorBrush(Colors.Black),
            StrokeThickness = 2
        };
        _canvas.Children.Add(xAxis);

        // Y-axis
        var yAxis = new Line
        {
            StartPoint = new Avalonia.Point(CenterX, 0),
            EndPoint = new Avalonia.Point(CenterX, CanvasHeight),
            Stroke = new SolidColorBrush(Colors.Black),
            StrokeThickness = 2
        };
        _canvas.Children.Add(yAxis);
    }
}