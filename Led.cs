namespace FpgManager
{
    public static class LedsHelper
    {
        public static Led firstChannelLed = new(6, CommandNamesHelper.SetOnePointSevenVolt, 1);
        public static Led secondChannelLed = new(7, CommandNamesHelper.SetZeroVolt, 2);
        public static Led thirdChannelLed = new(8, CommandNamesHelper.SetZeroVolt, 3);
        public static Led fourthChannelLed = new(9, CommandNamesHelper.SetZeroVolt, 4);

        public static Led[] leds = new[] { firstChannelLed, secondChannelLed, thirdChannelLed, fourthChannelLed };

        public static void LedsBubbleSort()
        {
            leds = new[] { firstChannelLed, secondChannelLed, thirdChannelLed, fourthChannelLed };
            for (int i = leds.Length - 1; i >= 0; i--)
            {
                bool isSorted = true;

                for (int j = 0; j < i; j++)
                {
                    if (leds[j].Index > leds[j + 1].Index)
                    {
                        (leds[j + 1], leds[j]) = (leds[j], leds[j + 1]);
                        isSorted = false;
                    }
                }
                if (isSorted) { break; }
            }
        }
    }

    public class Led
    {
        public Command Channel { get; }
        public Command Intensity { get; }
        public int Index { get; }
        public Point[] NoFilterPoints { get; set; } = Array.Empty<Point>();
        public Point[] SimpleMovingAveragePoints { get; set; } = Array.Empty<Point>();
        public Point[] MaxPeaksPoints { get; set; } = Array.Empty<Point>();
        public Point[] MinPeaksPoints { get; set; } = Array.Empty<Point>();

        private readonly int pin;

        public Led(int pin, Command intensity, int index)
        {
            Intensity = intensity;
            Index = index;

            this.pin = pin;
            Channel = pin switch
            {
                6 => CommandNamesHelper.FirstChannelPin,
                7 => CommandNamesHelper.SecondChannelPin,
                8 => CommandNamesHelper.ThirdChannelPin,
                9 => CommandNamesHelper.FourthChannelPin,
                _ => throw new Exception("There is not such channel to connect pin")
            };
        }

        public override string ToString()
        {
            return $"Pin:{pin}, Index:{Index}, Voltage:{Intensity}";
        }

        public string GetName() => "Channel " + pin switch
        {
            6 => "1",
            7 => "2",
            8 => "3",
            9 => "4",
            _ => "Unknown channel"
        };
    }
}
