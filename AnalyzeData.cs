namespace FpgManager
{
    class AnalyzeData
    {
        public static void GetPeaks(int nearPointsCount = 10)
        {
            foreach (var led in LedsHelper.leds)
            {
                var maxValuesList = new List<Point>();
                var minValuesList = new List<Point>();

                for (int i = nearPointsCount + 1; i < led.SimpleMovingAveragePoints.Length - nearPointsCount - 1; i++)
                {
                    var isMaxPeak = true;
                    var isMinPeak = true;
                    for (int j = 0; j < nearPointsCount; j++)
                    {
                        if (isMaxPeak && 
                            !(led.SimpleMovingAveragePoints[i].Y >= led.SimpleMovingAveragePoints[i + 1 + j].Y && 
                            led.SimpleMovingAveragePoints[i].Y >= led.SimpleMovingAveragePoints[i - 1 - j].Y)) { isMaxPeak = false; }
                        
                        if (isMinPeak && 
                            !(led.SimpleMovingAveragePoints[i].Y <= led.SimpleMovingAveragePoints[i + 1 + j].Y &&
                            led.SimpleMovingAveragePoints[i].Y <= led.SimpleMovingAveragePoints[i - 1 - j].Y)) { isMinPeak = false; }
                        
                        if (!(isMaxPeak || isMinPeak)) { break; }
                    }
                    if (isMaxPeak) { maxValuesList.Add(led.SimpleMovingAveragePoints[i]); }
                    if (isMinPeak) { minValuesList.Add(led.SimpleMovingAveragePoints[i]); }
                }

                for (int i = 0; i < maxValuesList.Count - 1; i++)
                {
                    if (maxValuesList[i].Y == maxValuesList[i + 1].Y) { maxValuesList.RemoveAt(i + 1); }
                }

                for (int i = 0; i < minValuesList.Count - 1; i++)
                {
                    if (minValuesList[i].Y == minValuesList[i + 1].Y) { minValuesList.RemoveAt(i + 1); }
                }

                led.MaxPeaksPoints = maxValuesList.ToArray();
                led.MinPeaksPoints = minValuesList.ToArray();
            }
        }
    }
}
