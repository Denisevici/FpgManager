namespace FpgManager
{
    class FilterData
    {   
        public static void SimpleMovingAverageFilter(int intervals)
        {
            foreach (var led in LedsHelper.leds)
            {
                if (led.NoFilterPoints.Length != 0)
                {
                    var newPoints = new Point[led.NoFilterPoints.Length - intervals];

                    for (int i = intervals / 2; i < led.NoFilterPoints.Length - intervals / 2; i++)
                    {
                        int intervalSum = 0;
                        for (int j = -intervals / 2; j < intervals / 2; j++)
                        {
                            intervalSum += led.NoFilterPoints[i + j].Y;
                        }
                        newPoints[i - intervals / 2] = new Point(led.NoFilterPoints[i].X, intervalSum / intervals);
                    }
                    led.SimpleMovingAveragePoints = newPoints;
                }
            }
        }
    }
}
