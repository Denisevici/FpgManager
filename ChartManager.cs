using System.Windows.Forms.DataVisualization.Charting;

namespace FpgManager
{
    class ChartManager
    {
        public static void DrawChart(Chart chart, bool drawNoFilter = true, bool drawFilters = false, bool drawPeaks = false)
        {
            foreach (var led in LedsHelper.leds)
            {
                var arrayNumber = 0;

                var lists = new List<Point[]>
                {
                    drawNoFilter ? led.NoFilterPoints : Array.Empty<Point>(),
                    drawFilters ? led.SimpleMovingAveragePoints : Array.Empty<Point>(),
                    drawPeaks ? led.MaxPeaksPoints : Array.Empty<Point>(),
                    drawPeaks ? led.MinPeaksPoints : Array.Empty<Point>(),
                };
                
                foreach (var points in lists)
                {
                    if (points.Length != 0)
                    {
                        var name = led.GetName() + arrayNumber switch
                        {
                            0 => "_NoFilter",
                            1 => "_SimpleMovingAverageFilter",
                            2 => "_MaxPeaks",
                            3 => "_MinPeaks",
                            _ => throw new Exception("Unknown array of points")
                        };

                        var serie = chart.Series.FindByName(name);
                        if (chart.Series.Contains(serie)) { chart.Series.Remove(serie); }

                        serie = new Series(name);
                        serie.ChartType = arrayNumber switch
                        {
                            0 => SeriesChartType.Spline,
                            1 => SeriesChartType.Spline,
                            2 => SeriesChartType.Point,
                            3 => SeriesChartType.Point,
                            _ => throw new Exception("Unknown array of points")
                        };
                        serie.Color = name.Contains("Peaks") ? Color.Black : 
                            name.Contains("MovingAverage") ? Color.Blue :
                            name.Contains("1") ? Color.Red :
                            name.Contains("2") ? Color.Green :
                            name.Contains("3") ? Color.Blue : Color.Orange;

                        var minX = points[0].X;
                        var maxX = points[^1].X;
                        var minY = points[0].Y;
                        var maxY = points[0].Y;

                        foreach (var point in points)
                        {
                            minY = minY > point.Y ? point.Y : minY;
                            maxY = maxY < point.Y ? point.Y : maxY;
                            serie.Points.AddXY(point.X, point.Y);
                        }
                        chart.Series.Add(serie);

                        var legend = new Legend(name);
                        if (!chart.Legends.Contains(legend) && !name.Contains("Peaks"))
                        {
                            chart.Legends.Add(legend);
                            chart.Series[name].Legend = name;
                            chart.Legends[name].Docking = Docking.Bottom;
                        }

                        if (chart.ChartAreas.Count == 0)
                        {
                            var chartArea = new ChartArea("ChartArea");
                            chartArea.AxisX.Minimum = minX;
                            chartArea.AxisX.Maximum = maxX;
                            chartArea.AxisY.Minimum = minY;
                            chartArea.AxisY.Maximum = maxY + (maxY - minY) * 0.25;
                            chart.ChartAreas.Add(chartArea);
                        }
                        else
                        {
                            var chartArea = chart.ChartAreas["ChartArea"];
                            chartArea.AxisX.Minimum = minX < chartArea.AxisX.Minimum ? minX : chartArea.AxisX.Minimum;
                            chartArea.AxisX.Maximum = maxX > chartArea.AxisX.Maximum ? maxX : chartArea.AxisX.Maximum;
                            chartArea.AxisY.Minimum = minY < chartArea.AxisY.Minimum ? minY : chartArea.AxisY.Minimum;
                            chartArea.AxisY.Maximum = maxY > chartArea.AxisY.Maximum
                                ? maxY + (maxY - minY) * 0.25
                                : chartArea.AxisY.Maximum;
                        }
                    }
                    arrayNumber++;
                }
            }
        }

        public static void SetNewRange(Chart chart, int from, int to)
        {
            chart.ChartAreas[0].AxisX.Minimum = from;
            chart.ChartAreas[0].AxisX.Maximum = to;
        }

        public static void ZoomAxis(Chart chart, ZoomAxisMode mode)
        {
            var chartArea = chart.ChartAreas[0];
            var moveX = (chartArea.AxisX.Maximum - chartArea.AxisX.Minimum) * 0.1;
            var moveY = (chartArea.AxisY.Maximum - chartArea.AxisY.Minimum) * 0.1;

            switch (mode)
            {
                case ZoomAxisMode.UpX:
                {
                    chartArea.AxisX.Minimum += moveX;
                    chartArea.AxisX.Maximum -= moveX;
                    break;
                }
                case ZoomAxisMode.DownX:
                {
                    chartArea.AxisX.Minimum -= moveX;
                    chartArea.AxisX.Maximum += moveX;
                    break;
                }
                case ZoomAxisMode.UpY:
                {
                    chartArea.AxisY.Minimum += moveY;
                    chartArea.AxisY.Maximum -= moveY;
                    break;
                }
                case ZoomAxisMode.DownY:
                {
                    chartArea.AxisY.Minimum -= moveY;
                    chartArea.AxisY.Maximum += moveY;
                    break;
                }
            }
            chart.ChartAreas.Clear();
            chart.ChartAreas.Add(chartArea);
        }

        public static void MoveAlongAxis(Chart chart, MoveAxisDirection direction)
        {
            var chartArea = chart.ChartAreas[0];
            var moveX = (chartArea.AxisX.Maximum - chartArea.AxisX.Minimum) * 0.1;
            var moveY = (chartArea.AxisY.Maximum - chartArea.AxisY.Minimum) * 0.1;

            switch (direction)
            {
                case MoveAxisDirection.LeftX:
                {
                    chartArea.AxisX.Minimum -= moveX;
                    chartArea.AxisX.Maximum -= moveX;
                    break;
                }
                case MoveAxisDirection.RightX:
                {
                    chartArea.AxisX.Minimum += moveX;
                    chartArea.AxisX.Maximum += moveX;
                    break;
                }
                case MoveAxisDirection.UpY:
                {
                    chartArea.AxisY.Minimum += moveY;
                    chartArea.AxisY.Maximum += moveY;
                    break;
                }
                case MoveAxisDirection.DownY:
                {
                    chartArea.AxisY.Minimum -= moveY;
                    chartArea.AxisY.Maximum -= moveY;
                    break;
                }
            }
            chart.ChartAreas.Clear();
            chart.ChartAreas.Add(chartArea);
        }

        public static void ShowPointCoordinates(Chart chart, Label infoLabel, MouseEventArgs args)
        {
            HitTestResult result = chart.HitTest(args.X, args.Y);
            if (result.ChartElementType == ChartElementType.DataPoint)
            {
                if (result.PointIndex >= 0)
                {
                    try
                    {
                        DataPoint point = chart.Series["Channel 1_MaxPeaks"].Points[result.PointIndex];
                        infoLabel.Text = $"(Max peak) X:{point.XValue} Y:{point.YValues[0]}";
                    }
                    catch { }
                    try
                    {
                        DataPoint point = chart.Series["Channel 1_MinPeaks"].Points[result.PointIndex];
                        infoLabel.Text = $"(Min peak) X:{point.XValue} Y:{point.YValues[0]}";
                    }
                    catch { }
                }
            }
        }
    }
}
