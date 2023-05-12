namespace FpgManager
{
    public static class CommandNamesHelper
    {
        public static readonly Command DisableTransmitting = new (CommandName.DisableTransmitting, "0");
        public static readonly Command EnableTransmitting = new(CommandName.DisableTransmitting, "1");

        public static readonly Command SetTickPerMsFrequency = new(CommandName.SetTickPerMsFrequency, "2");
        public static readonly Command SetTickPerTenMsFrequency = new(CommandName.SetTickPerTenMsFrequency, "3");
        public static readonly Command SetTickPerHundredMsFrequency = new(CommandName.SetTickPerHundredMsFrequency, "4");
        public static readonly Command SetTickPerThousandMsFrequency = new(CommandName.SetTickPerThousandMsFrequency, "5");
        
        public static readonly Command SetTimeToZero = new(CommandName.SetTimeToZero, "6");
        public static readonly Command PrepareToConfigureChannels = new(CommandName.PrepareToConfigureChannels, "7");
        
        public static readonly Command PrepareToSetTransmittingTimeDuration = new(CommandName.PrepareToSetTransmittingTimeDuration, "8");

        public static readonly Command SetZeroVolt = new(CommandName.SetZeroVolt, "0");
        public static readonly Command SetOnePointFourVolt = new(CommandName.SetOnePointFourVolt, "1");
        public static readonly Command SetOnePointFiveVolt = new(CommandName.SetOnePointFiveVolt, "2");
        public static readonly Command SetOnePointSixVolt = new(CommandName.SetOnePointSixVolt, "3");
        public static readonly Command SetOnePointSevenVolt = new(CommandName.SetOnePointSevenVolt, "4");
        public static readonly Command SetOnePointEightVolt = new(CommandName.SetOnePointEightVolt, "5");
        public static readonly Command SetOnePointNineVolt = new(CommandName.SetOnePointNineVolt, "6");
        public static readonly Command SetTwoPointZeroVolt = new(CommandName.SetTwoPointZeroVolt, "7");
        public static readonly Command SetOnePointSixtyFiveVolt = new(CommandName.SetOnePointSixtyFiveVolt, "8");

        public static readonly Command FirstChannelPin = new(CommandName.SetPinToFirstChannel, "6");
        public static readonly Command SecondChannelPin = new(CommandName.SetPinToSecondChannel, "7");
        public static readonly Command ThirdChannelPin = new(CommandName.SetPinToThirdChannel, "8");
        public static readonly Command FourthChannelPin = new(CommandName.SetPinToFourthChannel, "9");

        public static readonly Command SetLedToBeOnDuringTransmitting = new(CommandName.SetLedToBeOnDuringTransmitting, "1");
        public static readonly Command SetLedToBeOffDuringTransmitting = new(CommandName.SetLedToBeOffDuringTransmitting, "0");

        public static readonly Command BlinkingMode = new(CommandName.BlinkingMode, "1");
        public static readonly Command AlwaysOnMode = new(CommandName.AlwaysOnMode, "0");

        public static readonly object[] FrequencyValueCommands = new object[] {
            SetTickPerMsFrequency,
            SetTickPerTenMsFrequency,
            SetTickPerHundredMsFrequency,
            SetTickPerThousandMsFrequency };

        public static readonly object[] VoltageValueCommands = new object[] {
            SetZeroVolt,
            SetOnePointFourVolt,
            SetOnePointFiveVolt,
            SetOnePointSixVolt,
            SetOnePointSevenVolt,
            SetOnePointEightVolt,
            SetOnePointNineVolt,
            SetTwoPointZeroVolt,
            SetOnePointSixtyFiveVolt };

        public static readonly object[] WorkingModeOfSingleLed = new object[] {
            BlinkingMode,
            AlwaysOnMode };

        public static Command[] CreateCommands(params char[] values)
        {
            var commands = new Command[values.Length];
            for (int i = 0; i < commands.Length; i++)
            {
                commands[i] = new(CommandName.SomeValue, values[i].ToString());
            }
            return commands;
        }
    }

    public enum CommandName
    {
        DisableTransmitting,
        EnableTransmitting,

        SetTickPerMsFrequency,
        SetTickPerTenMsFrequency,
        SetTickPerHundredMsFrequency,
        SetTickPerThousandMsFrequency,

        SetZeroVolt,
        SetOnePointFourVolt,
        SetOnePointFiveVolt,
        SetOnePointSixVolt,
        SetOnePointSevenVolt,
        SetOnePointEightVolt,
        SetOnePointNineVolt,
        SetTwoPointZeroVolt,
        SetOnePointSixtyFiveVolt,

        SetPinToFirstChannel,
        SetPinToSecondChannel,
        SetPinToThirdChannel,
        SetPinToFourthChannel,

        SetLedToBeOnDuringTransmitting,
        SetLedToBeOffDuringTransmitting,

        SetTimeToZero,
        PrepareToConfigureChannels,
        PrepareToSetTransmittingTimeDuration,

        BlinkingMode,
        AlwaysOnMode,

        SomeValue,
    }
    
    public readonly struct Command
    {
        public CommandName CommandName { get; }
        public string Value { get; }

        public Command(CommandName commandName, string value)
        {
            CommandName = commandName;
            Value = value;
        }

        public override string ToString()
        {
            return CommandName switch
            {
                CommandName.SetTickPerMsFrequency => "1 tick per ms",
                CommandName.SetTickPerTenMsFrequency => "1 tick per 10 ms",
                CommandName.SetTickPerHundredMsFrequency => "1 tick per 100 ms",
                CommandName.SetTickPerThousandMsFrequency => "1 tick per 1000 ms",

                CommandName.SetZeroVolt => "0",
                CommandName.SetOnePointFourVolt => "1.4",
                CommandName.SetOnePointFiveVolt => "1.5",
                CommandName.SetOnePointSixVolt => "1.6",
                CommandName.SetOnePointSevenVolt => "1.7",
                CommandName.SetOnePointEightVolt => "1.8",
                CommandName.SetOnePointNineVolt => "1.9",
                CommandName.SetTwoPointZeroVolt => "2.0",
                CommandName.SetOnePointSixtyFiveVolt => "1.65",

                CommandName.BlinkingMode => "Blink",
                CommandName.AlwaysOnMode => "Always on",

                _ => "No special name for this command"
            };
        }
    
        public override bool Equals(object? obj)
        {
            if (obj is null) throw new NullReferenceException();
            return CommandName == ((Command)obj).CommandName;
        }
    }
}
