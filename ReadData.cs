using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace FpgManager
{
    class ReadData
    {
        public static bool TryGetDataFromComPortCreatedFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                var ledsOnCount = LedsHelper.leds.Select(led => double.Parse(led.Intensity.Value) > 0).Count();

                var result0 = new List<Point>();
                var result1 = new List<Point>();
                var result2 = new List<Point>();
                var result3 = new List<Point>();

                var pointStringBuilder = new StringBuilder();
                var dataStringBuilder = new StringBuilder();
                var count = 0;
                int number;

                var fs = new FileStream(filePath, FileMode.Open);
                while ((number = fs.ReadByte()) != -1)
                {
                    pointStringBuilder.Append($"{number:X2}");
                    if (pointStringBuilder.Length == 8)
                    {
                        var stringValue = pointStringBuilder.ToString();
                        pointStringBuilder.Clear();
                        var time = int.Parse(stringValue[..4].ToString(), NumberStyles.HexNumber);
                        var value = int.Parse(stringValue[4..].ToString(), NumberStyles.HexNumber);

                        if (count == 0) { result0.Add(new Point(time, value)); }
                        if (count == 1) { result1.Add(new Point(time, value)); }
                        if (count == 2) { result2.Add(new Point(time, value)); }
                        if (count == 3) { result3.Add(new Point(time, value)); }

                        count++;
                        dataStringBuilder.AppendLine($"X:{time} Y:{value}");
                        if (count == ledsOnCount)
                        {
                            dataStringBuilder.AppendLine();
                            count = 0;
                        }
                    }
                }
                fs.Close();
                File.WriteAllText(filePath, dataStringBuilder.ToString());

                LedsHelper.leds[0].NoFilterPoints = result0.ToArray();
                LedsHelper.leds[1].NoFilterPoints = result1.ToArray();
                LedsHelper.leds[2].NoFilterPoints = result2.ToArray();
                LedsHelper.leds[3].NoFilterPoints = result3.ToArray();
                return true;
            }
            return false;
        }

        public static bool TryGetDataFromProcessedFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                var result0 = new List<Point>();
                var result1 = new List<Point>();
                var result2 = new List<Point>();
                var result3 = new List<Point>();

                var block = 0;
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    if (line.Length > 2)
                    {
                        var match = new Regex(@"X:(\d+) Y:(\d+)").Match(line);
                        var point = new Point(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value));

                        if (block == 0) { result0.Add(point); }
                        else if (block == 1) { result1.Add(point); }
                        else if (block == 2) { result2.Add(point); }
                        else if (block == 3) { result3.Add(point); }
                        block++;
                    }
                    else { block = 0; }
                }

                LedsHelper.leds[0].NoFilterPoints = result0.ToArray();
                LedsHelper.leds[1].NoFilterPoints = result1.ToArray();
                LedsHelper.leds[2].NoFilterPoints = result2.ToArray();
                LedsHelper.leds[3].NoFilterPoints = result3.ToArray();
                return true;
            }
            return false;
        }
    }
}
