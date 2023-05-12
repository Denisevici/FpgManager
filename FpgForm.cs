using System.Windows.Forms.DataVisualization.Charting;
using System.IO.Ports;
using System.Text;

namespace FpgManager
{
    class FpgForm : Form
    {
        private SerialPort comPort = new();
        private FileStream fileStream;

        private readonly Size buttonSize;
        private readonly Size shortLabelSize;
        private readonly Size mediumLabelSize;
        private readonly Size longLabelSize;

        private readonly Button settingsButton = new();

        #region AnalyzePart
        private readonly Label instructionLabel = new();
        private readonly Label comPortFileInstructionLabel = new();
        private readonly Label peakPointsInfoLabel = new();
        private readonly Label peakPointsFilePathLabel = new();
        private readonly Label durationTransmittingLabel = new();
        private readonly Label toLabel= new();
        private readonly Label fromLabel= new();

        private readonly TextBox inputFilePathTextBox = new();
        private readonly TextBox rangeFromTextBox = new();
        private readonly TextBox rangeToTextBox = new();
        private readonly TextBox intervalsCountBox = new();
        private readonly TextBox filePathToWriteDataFromComPortTextBox = new();
        private readonly TextBox peakPointsFilePathTextBox = new();
        private readonly TextBox peakPointBetweenDistanceTextBox = new();
        private readonly TextBox durationTransmittingTextBox = new();

        private readonly Button setNewRangeButton = new();
        private readonly Button zoomUpOnAxisXButton = new();
        private readonly Button zoomDownOnAxisXButton = new();
        private readonly Button zoomUpOnAxisYButton = new();
        private readonly Button zoomDownOnAxisYButton = new();
        private readonly Button moveLeftOnAxisXButton = new();
        private readonly Button moveRightOnAxisXButton = new();
        private readonly Button moveUpOnAxisYButton = new();
        private readonly Button moveDownOnAxisYButton = new();
        private readonly Button simpleMovingAverageFilterButton = new();
        private readonly Button clearAllChartsButton = new();
        private readonly Button startScanButton = new();
        private readonly Button startAnalyzeFromFileButton = new();
        private readonly Button showPeakPointsButton = new();
        private readonly Button writePeakPointsInfoToFileButton = new();

        private readonly Chart chart = new();

        private readonly CheckBox showNoFilterPlotCheckBox = new();
        private readonly CheckBox showFilteredPlotCheckBox = new();

        private readonly List<Control> analyzeControls;
        #endregion

        #region SettingsPart
        private readonly Button applySettingsButton = new();
        private readonly Button updatePortsButton = new();
        private readonly Button connectPortButton = new();

        private readonly ComboBox portsComboBox = new();

        private readonly Label firstChannelLabel = new();
        private readonly Label secondChannelLabel = new();
        private readonly Label thirdChannelLabel = new();
        private readonly Label fourthChannelLabel = new();
        private readonly Label onOffLedLabel = new();
        private readonly Label indexLabel = new();
        private readonly Label intensityLabel = new();
        private readonly Label transmissionFrequencyLabel = new();
        private readonly Label comPortSettingsLabel = new();
        private readonly Label baudrateLabel = new();
        private readonly Label dataBitsLabel = new();
        private readonly Label parityLabel = new();
        private readonly Label stopBitsLabel = new();
        private readonly Label singleLedWorkingModeLabel = new();

        private readonly CheckBox firstChannelOnCheckBox = new();
        private readonly CheckBox secondChannelOnCheckBox = new();
        private readonly CheckBox thirdChannelOnCheckBox = new();
        private readonly CheckBox fourthChannelOnCheckBox = new();

        private readonly ComboBox firstChannelIndexComboBox = new();
        private readonly ComboBox secondChannelIndexComboBox = new();
        private readonly ComboBox thirdChannelIndexComboBox = new();
        private readonly ComboBox fourthChanneIndexComboBox = new();

        private readonly ComboBox firstChannelIntensityComboBox = new();
        private readonly ComboBox secondChannelIntensityComboBox = new();
        private readonly ComboBox thirdChannelIntensityComboBox = new();
        private readonly ComboBox fourthChannelIntensityComboBox = new();

        private readonly ComboBox transmissionFrequencyComboBox = new();
        private readonly ComboBox singleLedOnWorkingModeComboBox = new();

        private readonly ComboBox baudrateComboBox = new();
        private readonly ComboBox dataBitsComboBox = new();
        private readonly ComboBox parityComboBox = new();
        private readonly ComboBox stopBitsComboBox = new();
        
        private readonly List<Control> settingsControls;
        #endregion

        private readonly string[] indexesArray = new[] { "1", "2", "3", "4" };

        public FpgForm()
        {
            WindowState = FormWindowState.Maximized;
            Width = Screen.PrimaryScreen.WorkingArea.Width;
            Height = Screen.PrimaryScreen.WorkingArea.Height;

            buttonSize = new Size(ClientSize.Width * 10 / 100, ClientSize.Height * 5 / 100);
            mediumLabelSize = new Size(buttonSize.Width, buttonSize.Height / 2);
            longLabelSize = new Size(mediumLabelSize.Width * 3, instructionLabel.Size.Height);
            shortLabelSize = new Size(mediumLabelSize.Width / 2, mediumLabelSize.Height);

            analyzeControls = new()
            {
                instructionLabel,
                comPortFileInstructionLabel,
                inputFilePathTextBox,
                rangeFromTextBox,
                rangeToTextBox,
                intervalsCountBox,
                setNewRangeButton,
                zoomUpOnAxisXButton,
                zoomDownOnAxisXButton,
                zoomUpOnAxisYButton,
                zoomDownOnAxisYButton,
                moveLeftOnAxisXButton,
                moveRightOnAxisXButton,
                moveUpOnAxisYButton,
                moveDownOnAxisYButton,
                simpleMovingAverageFilterButton,
                clearAllChartsButton,
                chart,
                showNoFilterPlotCheckBox,
                showFilteredPlotCheckBox,
                startScanButton,
                filePathToWriteDataFromComPortTextBox,
                startAnalyzeFromFileButton,
                peakPointsInfoLabel,
                peakPointBetweenDistanceTextBox,
                peakPointsFilePathLabel,
                peakPointsFilePathTextBox,
                showPeakPointsButton,
                writePeakPointsInfoToFileButton,
                durationTransmittingTextBox,
                durationTransmittingLabel,
                toLabel,
                fromLabel,
            };

            settingsControls = new()
            {
                applySettingsButton,
                updatePortsButton,
                connectPortButton,
                portsComboBox,
                firstChannelLabel,
                secondChannelLabel,
                thirdChannelLabel,
                fourthChannelLabel,
                onOffLedLabel,
                firstChannelOnCheckBox,
                secondChannelOnCheckBox,
                thirdChannelOnCheckBox,
                fourthChannelOnCheckBox,
                indexLabel,
                intensityLabel,
                transmissionFrequencyLabel,
                comPortSettingsLabel,
                baudrateLabel,
                dataBitsLabel,
                parityLabel,
                stopBitsLabel,
                firstChannelIndexComboBox,
                secondChannelIndexComboBox,
                thirdChannelIndexComboBox,
                fourthChanneIndexComboBox,
                firstChannelIntensityComboBox,
                secondChannelIntensityComboBox,
                thirdChannelIntensityComboBox,
                fourthChannelIntensityComboBox,
                transmissionFrequencyComboBox,
                baudrateComboBox,
                dataBitsComboBox,
                parityComboBox,
                stopBitsComboBox,
                singleLedOnWorkingModeComboBox,
                singleLedWorkingModeLabel,
            };

            settingsButton.Location = new Point(0, 0);
            settingsButton.Size = buttonSize;
            settingsButton.Text = "Close settings";
            settingsButton.Click += (sender, args) => SettingsButtonOnClick();
            Controls.Add(settingsButton);

            chart.Location = new Point(settingsButton.Left, settingsButton.Bottom);
            chart.Size = new Size(ClientSize.Width * 80 / 100, ClientSize.Height * 80 / 100);
            chart.MouseMove += (sender, args) => ChartManager.ShowPointCoordinates(chart, peakPointsInfoLabel, args);
            Controls.Add(chart);

            #region ScaleButtons
            zoomUpOnAxisXButton.Location = new Point(chart.Right + ClientSize.Width * 5 / 100, instructionLabel.Bottom);
            zoomUpOnAxisXButton.Size = buttonSize;
            zoomUpOnAxisXButton.Text = "Zoom up on axis X";
            zoomUpOnAxisXButton.Click += (sender, args) => ChartManager.ZoomAxis(chart, ZoomAxisMode.UpX);
            Controls.Add(zoomUpOnAxisXButton);

            zoomDownOnAxisXButton.Location = new Point(zoomUpOnAxisXButton.Left, zoomUpOnAxisXButton.Bottom);
            zoomDownOnAxisXButton.Size = buttonSize;
            zoomDownOnAxisXButton.Text = "Zoom down on axis X";
            zoomDownOnAxisXButton.Click += (sender, args) => ChartManager.ZoomAxis(chart, ZoomAxisMode.DownX);
            Controls.Add(zoomDownOnAxisXButton);

            zoomUpOnAxisYButton.Location = new Point(zoomUpOnAxisXButton.Left, zoomDownOnAxisXButton.Bottom);
            zoomUpOnAxisYButton.Size = buttonSize;
            zoomUpOnAxisYButton.Text = "Zoom up on axis Y";
            zoomUpOnAxisYButton.Click += (sender, args) => ChartManager.ZoomAxis(chart, ZoomAxisMode.UpY);
            Controls.Add(zoomUpOnAxisYButton);

            zoomDownOnAxisYButton.Location = new Point(zoomUpOnAxisXButton.Left, zoomUpOnAxisYButton.Bottom);
            zoomDownOnAxisYButton.Size = buttonSize;
            zoomDownOnAxisYButton.Text = "Zoom down on axis Y";
            zoomDownOnAxisYButton.Click += (sender, args) => ChartManager.ZoomAxis(chart, ZoomAxisMode.DownY);
            Controls.Add(zoomDownOnAxisYButton);
            #endregion

            #region MoveButtons
            moveUpOnAxisYButton.Location = new Point(zoomUpOnAxisXButton.Left, zoomDownOnAxisYButton.Bottom + zoomUpOnAxisXButton.Height);
            moveUpOnAxisYButton.Size = zoomUpOnAxisXButton.Size;
            moveUpOnAxisYButton.Text = "Move up on axis Y";
            moveUpOnAxisYButton.Click += (sender, args) => ChartManager.MoveAlongAxis(chart, MoveAxisDirection.UpY);
            Controls.Add(moveUpOnAxisYButton);

            moveLeftOnAxisXButton.Location = new Point(chart.Right, moveUpOnAxisYButton.Bottom);
            moveLeftOnAxisXButton.Size = zoomUpOnAxisXButton.Size;
            moveLeftOnAxisXButton.Text = "Move left on axis X";
            moveLeftOnAxisXButton.Click += (sender, args) => ChartManager.MoveAlongAxis(chart, MoveAxisDirection.LeftX);
            Controls.Add(moveLeftOnAxisXButton);

            moveRightOnAxisXButton.Location = new Point(moveLeftOnAxisXButton.Right, moveUpOnAxisYButton.Bottom);
            moveRightOnAxisXButton.Size = zoomUpOnAxisXButton.Size;
            moveRightOnAxisXButton.Text = "Move right on axis X";
            moveRightOnAxisXButton.Click += (sender, args) => ChartManager.MoveAlongAxis(chart, MoveAxisDirection.RightX);
            Controls.Add(moveRightOnAxisXButton);

            moveDownOnAxisYButton.Location = new Point(zoomUpOnAxisXButton.Left, moveRightOnAxisXButton.Bottom);
            moveDownOnAxisYButton.Size = zoomUpOnAxisXButton.Size;
            moveDownOnAxisYButton.Text = "Move down on axis Y";
            moveDownOnAxisYButton.Click += (sender, args) => ChartManager.MoveAlongAxis(chart, MoveAxisDirection.DownY);
            Controls.Add(moveDownOnAxisYButton);
            #endregion

            #region RangeContols
            rangeFromTextBox.Location = new Point(zoomUpOnAxisXButton.Left, moveDownOnAxisYButton.Bottom + moveDownOnAxisYButton.Height);
            rangeFromTextBox.Size = shortLabelSize;
            rangeFromTextBox.Text = "2000";
            Controls.Add(rangeFromTextBox);

            rangeToTextBox.Location = new Point(rangeFromTextBox.Right, rangeFromTextBox.Top);
            rangeToTextBox.Size = shortLabelSize;
            rangeToTextBox.Text = "3000";
            Controls.Add(rangeToTextBox);

            fromLabel.Location = new Point(rangeFromTextBox.Left, rangeFromTextBox.Top - shortLabelSize.Height);
            fromLabel.Size = shortLabelSize;
            fromLabel.Text = "from:";
            Controls.Add(fromLabel);

            toLabel.Location = new Point(rangeToTextBox.Left, rangeToTextBox.Top - shortLabelSize.Height);
            toLabel.Size = shortLabelSize;
            toLabel.Text = "to:";
            Controls.Add(toLabel);

            setNewRangeButton.Location = new Point(rangeFromTextBox.Left, rangeToTextBox.Bottom);
            setNewRangeButton.Size = buttonSize;
            setNewRangeButton.Text = "Set new range";
            setNewRangeButton.Click += (sender, arg) 
                => ChartManager.SetNewRange(chart, int.Parse(rangeFromTextBox.Text), int.Parse(rangeToTextBox.Text));
            Controls.Add(setNewRangeButton);
            #endregion

            #region Filters Controls
            intervalsCountBox.Location = new Point(zoomUpOnAxisXButton.Left, setNewRangeButton.Bottom + setNewRangeButton.Height);
            intervalsCountBox.Size = mediumLabelSize;
            intervalsCountBox.Text = "2";
            Controls.Add(intervalsCountBox);
            
            simpleMovingAverageFilterButton.Location = new Point(intervalsCountBox.Left, intervalsCountBox.Bottom);
            simpleMovingAverageFilterButton.Size = buttonSize;
            simpleMovingAverageFilterButton.Text = "Simple moving average";
            simpleMovingAverageFilterButton.Click += (sender, args) => FiltersOnClick(FilterMode.SimpleMovingAverage);
            Controls.Add(simpleMovingAverageFilterButton);
            #endregion

            #region Get Data
            startScanButton.Location = new Point(chart.Left, chart.Bottom);
            startScanButton.Size = buttonSize;
            startScanButton.Text = "Get data from COM Port";
            startScanButton.Click += (sender, args) => StartScanButtonOnClick();
            Controls.Add(startScanButton);

            comPortFileInstructionLabel.Location = new Point(startScanButton.Right, startScanButton.Top);
            comPortFileInstructionLabel.Size = longLabelSize;
            comPortFileInstructionLabel.Text = "Input file's path to write data from COM Port:";
            Controls.Add(comPortFileInstructionLabel);

            filePathToWriteDataFromComPortTextBox.Location = new Point(startScanButton.Right, comPortFileInstructionLabel.Bottom);
            filePathToWriteDataFromComPortTextBox.Size = longLabelSize;
            filePathToWriteDataFromComPortTextBox.Text = "C:\\Users\\dkomerzan\\Desktop\\Practice\\data\\good_80\\finger_1_ms_1.txt";
            Controls.Add(filePathToWriteDataFromComPortTextBox);

            durationTransmittingTextBox.Location = new Point(startScanButton.Left, startScanButton.Bottom);
            durationTransmittingTextBox.Size = shortLabelSize;
            durationTransmittingTextBox.Text = "15";
            Controls.Add(durationTransmittingTextBox);
            
            durationTransmittingLabel.Location = new Point(durationTransmittingTextBox.Right, durationTransmittingTextBox.Top);
            durationTransmittingLabel.Size = longLabelSize;
            durationTransmittingLabel.Text = " - write time transmitting duration in seconds (up to 16 000s)";
            Controls.Add(durationTransmittingLabel);

            startAnalyzeFromFileButton.Location = new Point(durationTransmittingTextBox.Left, durationTransmittingTextBox.Bottom);
            startAnalyzeFromFileButton.Size = buttonSize;
            startAnalyzeFromFileButton.Click += (sender, args) => StartAnalizeDataFromFileOnClick();
            startAnalyzeFromFileButton.Text = "Get data from file";
            Controls.Add(startAnalyzeFromFileButton);

            instructionLabel.Location = new Point(startAnalyzeFromFileButton.Right, startAnalyzeFromFileButton.Top);
            instructionLabel.Size = mediumLabelSize;
            instructionLabel.Text = "Input file's path to analyze data:";
            Controls.Add(instructionLabel);

            inputFilePathTextBox.Location = new Point(startAnalyzeFromFileButton.Right, instructionLabel.Bottom);
            inputFilePathTextBox.Size = longLabelSize;
            inputFilePathTextBox.Text = "C:\\Users\\dkomerzan\\Desktop\\Practice\\data\\test\\2.txt";
            Controls.Add(inputFilePathTextBox);
            #endregion

            #region Peak Points
            peakPointsInfoLabel.Location = new Point(inputFilePathTextBox.Right, chart.Bottom);
            peakPointsInfoLabel.Size = longLabelSize;
            peakPointsInfoLabel.Text = "Move mouse to peak point and get its coordinates";
            Controls.Add(peakPointsInfoLabel);

            peakPointBetweenDistanceTextBox.Location = new Point(simpleMovingAverageFilterButton.Left, simpleMovingAverageFilterButton.Bottom + buttonSize.Height);
            peakPointBetweenDistanceTextBox.Size = mediumLabelSize;
            peakPointBetweenDistanceTextBox.Text = "1";
            Controls.Add(peakPointBetweenDistanceTextBox);

            showPeakPointsButton.Location = new Point(peakPointBetweenDistanceTextBox.Left, peakPointBetweenDistanceTextBox.Bottom);
            showPeakPointsButton.Size = buttonSize;
            showPeakPointsButton.Text = "Show peak points";
            showPeakPointsButton.Click += (sender, args) => ShowPeakPointsOnClick();
            Controls.Add(showPeakPointsButton);

            writePeakPointsInfoToFileButton.Location = new Point(showPeakPointsButton.Left, showPeakPointsButton.Bottom + buttonSize.Height);
            writePeakPointsInfoToFileButton.Size = buttonSize;
            writePeakPointsInfoToFileButton.Text = "Write peak point's info into file";
            writePeakPointsInfoToFileButton.Click += (sender, args) => WritePeakpointsInfoOnClick();
            Controls.Add(writePeakPointsInfoToFileButton);

            peakPointsFilePathLabel.Location = new Point(writePeakPointsInfoToFileButton.Left, writePeakPointsInfoToFileButton.Bottom);
            peakPointsFilePathLabel.Size = mediumLabelSize;
            peakPointsFilePathLabel.Text = "File path to write peak points info:";
            Controls.Add(peakPointsFilePathLabel);

            peakPointsFilePathTextBox.Location = new Point(peakPointsFilePathLabel.Left - longLabelSize.Width / 2, peakPointsFilePathLabel.Bottom);
            peakPointsFilePathTextBox.Size = longLabelSize;
            peakPointsFilePathTextBox.Text = "C:\\Users\\dkomerzan\\Desktop\\Practice\\data\\good_80\\peaks\\1.txt";
            Controls.Add(peakPointsFilePathTextBox);
            #endregion

            #region Show Plot CheckBoxes
            showFilteredPlotCheckBox.Location = new Point(peakPointsInfoLabel.Left, peakPointsInfoLabel.Bottom); 
            showFilteredPlotCheckBox.Size = mediumLabelSize;
            showFilteredPlotCheckBox.Checked = true;
            showFilteredPlotCheckBox.Text = "Show filtered data plot";
            showFilteredPlotCheckBox.Click += (sender, args) =>
            {
                if (chart.Series.Count > 1)
                {
                    chart.Series[1].Enabled = showFilteredPlotCheckBox.Checked;
                }
            };
            Controls.Add(showFilteredPlotCheckBox);

            showNoFilterPlotCheckBox.Location = new Point(showFilteredPlotCheckBox.Left, showFilteredPlotCheckBox.Bottom);
            showNoFilterPlotCheckBox.Size = mediumLabelSize;
            showNoFilterPlotCheckBox.Checked = true;
            showNoFilterPlotCheckBox.Text = "Show no filtered data plot";
            showNoFilterPlotCheckBox.Click += (sender, args) =>
            {
                if (chart.Series.Count > 0)
                {
                    chart.Series[0].Enabled = showNoFilterPlotCheckBox.Checked;
                }
            };
            Controls.Add(showNoFilterPlotCheckBox);
            #endregion

            #region Settings
            connectPortButton.Location = new Point(settingsButton.Left, settingsButton.Bottom);
            connectPortButton.Size = buttonSize;
            connectPortButton.Text = "Connect";
            connectPortButton.Click += (sender, args) => ConnectPortButtonOnClick();
            Controls.Add(connectPortButton);

            applySettingsButton.Location = new Point(connectPortButton.Left, connectPortButton.Bottom);
            applySettingsButton.Size = buttonSize;
            applySettingsButton.Text = "Apply new settings";
            applySettingsButton.Click += (sender, args) => ApplyNewSettingsButtonOnClick();
            Controls.Add(applySettingsButton);

            updatePortsButton.Location = new Point(connectPortButton.Right, connectPortButton.Top);
            updatePortsButton.Size = buttonSize;
            updatePortsButton.Text = "Update COM Ports";
            updatePortsButton.Click += (sender, args) => UpdatePortsButtonOnClick();
            Controls.Add(updatePortsButton);

            portsComboBox.Location = new Point(updatePortsButton.Left, updatePortsButton.Bottom);
            portsComboBox.Size = buttonSize;
            portsComboBox.Items.Add("No COM Port");
            portsComboBox.SelectedIndex = 0;
            portsComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            Controls.Add(portsComboBox);

            firstChannelLabel.Location = new Point(updatePortsButton.Right + updatePortsButton.Width / 2, applySettingsButton.Top);
            firstChannelLabel.Size = shortLabelSize;
            firstChannelLabel.Text = "Channel 1";
            Controls.Add(firstChannelLabel);

            secondChannelLabel.Location = new Point(firstChannelLabel.Left, firstChannelLabel.Bottom + firstChannelLabel.Height);
            secondChannelLabel.Size = shortLabelSize;
            secondChannelLabel.Text = "Channel 2";
            Controls.Add(secondChannelLabel);

            thirdChannelLabel.Location = new Point(secondChannelLabel.Left, secondChannelLabel.Bottom + secondChannelLabel.Height);
            thirdChannelLabel.Size = shortLabelSize;
            thirdChannelLabel.Text = "Channel 3";
            Controls.Add(thirdChannelLabel);

            fourthChannelLabel.Location = new Point(thirdChannelLabel.Left, thirdChannelLabel.Bottom + thirdChannelLabel.Height);
            fourthChannelLabel.Size = shortLabelSize;
            fourthChannelLabel.Text = "Channel 4";
            Controls.Add(fourthChannelLabel);

            onOffLedLabel.Location = new Point(firstChannelLabel.Right + firstChannelLabel.Width / 2, firstChannelLabel.Top - applySettingsButton.Height / 2);
            onOffLedLabel.Size = mediumLabelSize;
            onOffLedLabel.Text = "Led should be on?";
            Controls.Add(onOffLedLabel);

            indexLabel.Location = new Point(onOffLedLabel.Right - onOffLedLabel.Width / 4, onOffLedLabel.Top);
            indexLabel.Size = shortLabelSize;
            indexLabel.Text = "Index";
            Controls.Add(indexLabel);

            intensityLabel.Location = new Point(indexLabel.Right + indexLabel.Width / 2, indexLabel.Top);
            intensityLabel.Size = shortLabelSize;
            intensityLabel.Text = "Intensity";
            Controls.Add(intensityLabel);

            singleLedWorkingModeLabel.Location = new Point(firstChannelLabel.Left, fourthChannelLabel.Bottom + fourthChannelLabel.Height);
            singleLedWorkingModeLabel.Size = mediumLabelSize;
            singleLedWorkingModeLabel.Text = "Single led's working mode:";
            Controls.Add(singleLedWorkingModeLabel);

            singleLedOnWorkingModeComboBox.Location = new Point(singleLedWorkingModeLabel.Right, singleLedWorkingModeLabel.Top);
            singleLedOnWorkingModeComboBox.Items.AddRange(CommandNamesHelper.WorkingModeOfSingleLed);
            singleLedOnWorkingModeComboBox.SelectedIndex = 1;
            singleLedOnWorkingModeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            Controls.Add(singleLedOnWorkingModeComboBox);

            transmissionFrequencyLabel.Location = new Point(intensityLabel.Right + intensityLabel.Width / 2, intensityLabel.Top);
            transmissionFrequencyLabel.Size = mediumLabelSize;
            transmissionFrequencyLabel.Text = "Transmission frequency";
            Controls.Add(transmissionFrequencyLabel);

            transmissionFrequencyComboBox.Location = new Point(transmissionFrequencyLabel.Left, transmissionFrequencyLabel.Bottom);
            transmissionFrequencyComboBox.Items.AddRange(CommandNamesHelper.FrequencyValueCommands);
            transmissionFrequencyComboBox.SelectedIndex = 3;
            transmissionFrequencyComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            Controls.Add(transmissionFrequencyComboBox);

            firstChannelOnCheckBox.Location = new Point(onOffLedLabel.Left, firstChannelLabel.Top);
            firstChannelOnCheckBox.Checked = true;
            firstChannelOnCheckBox.CheckedChanged += (sender, args) =>
            {
                firstChannelIntensityComboBox.SelectedIndex = firstChannelOnCheckBox.Checked ? 8 : 0;
            };
            Controls.Add(firstChannelOnCheckBox);

            firstChannelIndexComboBox.Location = new Point(indexLabel.Left, firstChannelLabel.Top);
            firstChannelIndexComboBox.Items.AddRange(indexesArray);
            firstChannelIndexComboBox.SelectedIndex = 0;
            firstChannelIndexComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            Controls.Add(firstChannelIndexComboBox);

            firstChannelIntensityComboBox.Location = new Point(intensityLabel.Left, firstChannelLabel.Top);
            firstChannelIntensityComboBox.Items.AddRange(CommandNamesHelper.VoltageValueCommands);
            firstChannelIntensityComboBox.SelectedIndex = 8;
            firstChannelIndexComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            Controls.Add(firstChannelIntensityComboBox);

            secondChannelOnCheckBox.Location = new Point(onOffLedLabel.Left, secondChannelLabel.Top);
            secondChannelOnCheckBox.Checked = false;
            secondChannelOnCheckBox.CheckedChanged += (sender, args) =>
            {
                secondChannelIntensityComboBox.SelectedIndex = secondChannelOnCheckBox.Checked ? 8 : 0;
            };
            Controls.Add(secondChannelOnCheckBox);

            secondChannelIndexComboBox.Location = new Point(indexLabel.Left, secondChannelLabel.Top);
            secondChannelIndexComboBox.Items.AddRange(indexesArray);
            secondChannelIndexComboBox.SelectedIndex = 1;
            secondChannelIndexComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            Controls.Add(secondChannelIndexComboBox);

            secondChannelIntensityComboBox.Location = new Point(intensityLabel.Left, secondChannelLabel.Top);
            secondChannelIntensityComboBox.Items.AddRange(CommandNamesHelper.VoltageValueCommands);
            secondChannelIntensityComboBox.SelectedIndex = 0;
            secondChannelIntensityComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            Controls.Add(secondChannelIntensityComboBox);

            thirdChannelOnCheckBox.Location = new Point(onOffLedLabel.Left, thirdChannelLabel.Top);
            thirdChannelOnCheckBox.Checked = false;
            thirdChannelOnCheckBox.CheckedChanged += (sender, args) =>
            {
                thirdChannelIntensityComboBox.SelectedIndex = thirdChannelOnCheckBox.Checked ? 8 : 0;
            };
            Controls.Add(thirdChannelOnCheckBox);

            thirdChannelIndexComboBox.Location = new Point(indexLabel.Left, thirdChannelLabel.Top);
            thirdChannelIndexComboBox.Items.AddRange(indexesArray);
            thirdChannelIndexComboBox.SelectedIndex = 2;
            thirdChannelIndexComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            Controls.Add(thirdChannelIndexComboBox);

            thirdChannelIntensityComboBox.Location = new Point(intensityLabel.Left, thirdChannelLabel.Top);
            thirdChannelIntensityComboBox.Items.AddRange(CommandNamesHelper.VoltageValueCommands);
            thirdChannelIntensityComboBox.SelectedIndex = 0;
            thirdChannelIntensityComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            Controls.Add(thirdChannelIntensityComboBox);

            fourthChannelOnCheckBox.Location = new Point(onOffLedLabel.Left, fourthChannelLabel.Top);
            fourthChannelOnCheckBox.Checked = false;
            fourthChannelOnCheckBox.CheckedChanged += (sender, args) =>
            {
                fourthChannelIntensityComboBox.SelectedIndex = fourthChannelOnCheckBox.Checked ? 8 : 0;
            };
            Controls.Add(fourthChannelOnCheckBox);

            fourthChanneIndexComboBox.Location = new Point(indexLabel.Left, fourthChannelLabel.Top);
            fourthChanneIndexComboBox.Items.AddRange(indexesArray);
            fourthChanneIndexComboBox.SelectedIndex = 3;
            fourthChanneIndexComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            Controls.Add(fourthChanneIndexComboBox);

            fourthChannelIntensityComboBox.Location = new Point(intensityLabel.Left, fourthChannelLabel.Top);
            fourthChannelIntensityComboBox.Items.AddRange(CommandNamesHelper.VoltageValueCommands);
            fourthChannelIntensityComboBox.SelectedIndex = 0;
            fourthChannelIntensityComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            Controls.Add(fourthChannelIntensityComboBox);

            comPortSettingsLabel.Location = new Point(applySettingsButton.Left, applySettingsButton.Bottom);
            comPortSettingsLabel.Size = mediumLabelSize;
            comPortSettingsLabel.Text = "COM Port Settings:";
            Controls.Add(comPortSettingsLabel);

            baudrateLabel.Location = new Point(comPortSettingsLabel.Left, comPortSettingsLabel.Bottom);
            baudrateLabel.Size = shortLabelSize;
            baudrateLabel.Text = "Baudrate:";
            Controls.Add(baudrateLabel);

            dataBitsLabel.Location = new Point(baudrateLabel.Left, baudrateLabel.Bottom);
            dataBitsLabel.Size = shortLabelSize;
            dataBitsLabel.Text = "Data bits:";
            Controls.Add(dataBitsLabel);

            parityLabel.Location = new Point(dataBitsLabel.Left, dataBitsLabel.Bottom);
            parityLabel.Size = shortLabelSize;
            parityLabel.Text = "Parity:";
            Controls.Add(parityLabel);

            stopBitsLabel.Location = new Point(parityLabel.Left, parityLabel.Bottom);
            stopBitsLabel.Size = shortLabelSize;
            stopBitsLabel.Text = "Stop bits:";
            Controls.Add(stopBitsLabel);

            baudrateComboBox.Location = new Point(baudrateLabel.Right, baudrateLabel.Top);
            baudrateComboBox.Items.AddRange(new[] {"9600", "14400", "19200", "28800", "38400", "57600", "115200", "230400" });
            baudrateComboBox.SelectedIndex = 2;
            baudrateComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            Controls.Add(baudrateComboBox);

            dataBitsComboBox.Location = new Point(dataBitsLabel.Right, dataBitsLabel.Top);
            dataBitsComboBox.Items.AddRange(new[] { "5", "6", "7", "8" });
            dataBitsComboBox.SelectedIndex = 3;
            dataBitsComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            Controls.Add(dataBitsComboBox);

            parityComboBox.Location = new Point(parityLabel.Right, parityLabel.Top);
            parityComboBox.Items.AddRange(new[] { "None", "Odd", "Even", "Mark", "Space" });
            parityComboBox.SelectedIndex = 0;
            parityComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            Controls.Add(parityComboBox);

            stopBitsComboBox.Location = new Point(stopBitsLabel.Right, stopBitsLabel.Top);
            stopBitsComboBox.Items.AddRange(new[] { "One", "Two", "OnePointFive" });
            stopBitsComboBox.SelectedIndex = 0;
            stopBitsComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            Controls.Add(stopBitsComboBox);
            #endregion

            clearAllChartsButton.Location = new Point(showNoFilterPlotCheckBox.Left, showNoFilterPlotCheckBox.Bottom);
            clearAllChartsButton.Size = buttonSize;
            clearAllChartsButton.Text = "Clear chart";
            clearAllChartsButton.Click += (sender, args) =>
            {
                chart.Series.Clear();
                chart.ChartAreas.Clear();
            };
            Controls.Add(clearAllChartsButton);

            comPort.DataReceived += (sender, args) => ReceiveData(sender, args);

            foreach (var control in analyzeControls) { control.Visible = false; }
        }
        
        public static void Main()
        {
            Application.Run(new FpgForm());
        }

        private void StartAnalizeDataFromFileOnClick()
        {
            ReadData.TryGetDataFromProcessedFile(inputFilePathTextBox.Text);
            ChartManager.DrawChart(chart);
        }

        private void FiltersOnClick(FilterMode mode)
        {
            switch (mode)
            {
                case FilterMode.SimpleMovingAverage:
                {
                    FilterData.SimpleMovingAverageFilter(int.Parse(intervalsCountBox.Text));
                    break;
                }
                default: MessageBox.Show("There is not such filter"); break;
            }
            ChartManager.DrawChart(chart, false, true);
        }

        private void StartScanButtonOnClick()
        {
            if (startScanButton.Text == "Get data from COM Port")
            {
                
                startScanButton.Text = "Stop sending data and start analyze";
                fileStream = File.Create(filePathToWriteDataFromComPortTextBox.Text);
                var timeDuration = durationTransmittingTextBox.Text;
                timeDuration = timeDuration.PadLeft(5, '0');
                SendCommands(CommandNamesHelper.PrepareToSetTransmittingTimeDuration);
                SendCommands(CommandNamesHelper.CreateCommands(timeDuration[4], timeDuration[3], timeDuration[2],
                    timeDuration[1], timeDuration[0]));
                SendCommands(CommandNamesHelper.SetTimeToZero, CommandNamesHelper.EnableTransmitting);
            }
            else if (startScanButton.Text == "Stop sending data and start analyze")
            {
                startScanButton.Text = "Get data from COM Port";
                SendCommands(CommandNamesHelper.DisableTransmitting, CommandNamesHelper.DisableTransmitting);
                fileStream.Close();

                if (ReadData.TryGetDataFromComPortCreatedFile(filePathToWriteDataFromComPortTextBox.Text))
                {
                    ChartManager.DrawChart(chart);
                }
            }
        }

        #region Settings Methods
        private void UpdatePortsButtonOnClick()
        {
            var ports = SerialPort.GetPortNames();
            portsComboBox.Items.Clear();
            if (ports.Length != 0)
            {
                portsComboBox.Items.AddRange(ports);
                portsComboBox.SelectedIndex = 0;
            }
            else
            {
                portsComboBox.Text = "No COM ports detected.";
            }
        }

        private void ConnectPortButtonOnClick()
        {
            if (connectPortButton.Text == "Connect")
            {
                try
                {
                    var portName = portsComboBox.Text;
                    var baudRate = int.Parse(baudrateComboBox.Text);
                    var parity = parityComboBox.Text switch
                    {
                        "None" => Parity.None,
                        "Odd" => Parity.Odd,
                        "Even" => Parity.Even,
                        "Mark" => Parity.Mark,
                        "Space" => Parity.Space,
                        _ => Parity.None,
                    };
                    var dataBits = int.Parse(dataBitsComboBox.Text);
                    var stopBits = stopBitsComboBox.Text switch
                    {
                        "None" => StopBits.None,
                        "One" => StopBits.One,
                        "Two" => StopBits.Two,
                        "OnePointFive" => StopBits.OnePointFive,
                        _ => StopBits.None,
                    };

                    comPort = new(portName, baudRate, parity, dataBits, stopBits);
                    comPort.Open();
                    connectPortButton.Text = "Disconnect";
                }
                catch (Exception exception)
                {
                    MessageBox.Show($"Unable to connect, check device and settings: {exception.Message}");
                }
            }
            else if (connectPortButton.Text == "Disconnect")
            {
                fileStream.Close();
                comPort.Close();
                connectPortButton.Text = "Connect";
            }
            else { MessageBox.Show("Something wrong, restart the program"); }
        }

        private void SettingsButtonOnClick()
        {
            if (settingsButton.Text == "Open settings")
            {
                settingsButton.Text = "Close settings";
                foreach (var control in analyzeControls) { control.Visible = false; }
                foreach (var control in settingsControls) { control.Visible = true; }
            }
            else if (settingsButton.Text == "Close settings")
            {
                settingsButton.Text = "Open settings";
                foreach (var control in analyzeControls) { control.Visible = true; }
                foreach (var control in settingsControls) { control.Visible = false; }
            }
        }

        private void ApplyNewSettingsButtonOnClick()
        {
            var indexes = firstChannelIndexComboBox.Text + secondChannelIndexComboBox.Text + thirdChannelIndexComboBox.Text + fourthChanneIndexComboBox.Text;

            if (indexes.Length != indexes.Distinct().Count())
            {
                MessageBox.Show("Please, create correct sequence");
                return;
            }

            LedsHelper.firstChannelLed = new Led(6, (Command)firstChannelIntensityComboBox.SelectedItem, int.Parse(firstChannelIndexComboBox.Text));
            LedsHelper.secondChannelLed = new Led(7, (Command)secondChannelIntensityComboBox.SelectedItem, int.Parse(secondChannelIndexComboBox.Text));
            LedsHelper.thirdChannelLed = new Led(8, (Command)thirdChannelIntensityComboBox.SelectedItem, int.Parse(thirdChannelIndexComboBox.Text));
            LedsHelper.fourthChannelLed = new Led(9, (Command)fourthChannelIntensityComboBox.SelectedItem, int.Parse(fourthChanneIndexComboBox.Text));
            LedsHelper.LedsBubbleSort();

            SendCommands(CommandNamesHelper.DisableTransmitting, CommandNamesHelper.DisableTransmitting,
                CommandNamesHelper.PrepareToConfigureChannels);

            foreach (var led in LedsHelper.leds) { SendCommands(led.Channel, led.Intensity); }

            SendCommands((Command)singleLedOnWorkingModeComboBox.SelectedItem, (Command)transmissionFrequencyComboBox.SelectedItem);
        }
        
        #endregion

        #region Peak Points Methods
        private void ShowPeakPointsOnClick()
        {
            AnalyzeData.GetPeaks(int.Parse(peakPointBetweenDistanceTextBox.Text));
            ChartManager.DrawChart(chart, false, false, true);
        }

        private void WritePeakpointsInfoOnClick()
        {
            ShowPeakPointsOnClick();
            var sb = new StringBuilder();

            foreach (var led in LedsHelper.leds)
            {
                if (led.MaxPeaksPoints.Length != 0 && led.MinPeaksPoints.Length != 0)
                {
                    sb.AppendLine(led.GetName());
                    sb.AppendLine("Max peaks:");
                    foreach (var point in led.MaxPeaksPoints)
                    {
                        sb.AppendLine($"X:{point.X} Y:{point.Y}");
                    }
                    sb.AppendLine();
                    sb.AppendLine("Min peaks:");
                    foreach (var point in led.MinPeaksPoints)
                    {
                        sb.AppendLine($"X:{point.X} Y:{point.Y}");
                    }
                    sb.AppendLine();
                }
            }
            File.WriteAllText(peakPointsFilePathTextBox.Text, sb.ToString());
        }
        #endregion

        private void ReceiveData(object sender, SerialDataReceivedEventArgs args)
        {
            var serialPort = (SerialPort)sender;
            var buffer = new byte[serialPort.BytesToRead];
            serialPort.Read(buffer, 0, buffer.Length);
            if (buffer.All(b => b.Equals(255))) { startScanButton.Invoke(() => StartScanButtonOnClick()); }
            else { fileStream.Write(buffer, 0, buffer.Length); }
        }
        
        private void SendCommands(params Command[] commands)
        {
            foreach (var command in commands)
            {
                comPort.Write(command.Value);
                Thread.Sleep(10);
            }
        }
    }
}
