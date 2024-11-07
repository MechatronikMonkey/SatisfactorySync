using Avalonia.Data.Converters;
using Avalonia.Media;
using Avalonia.Threading;
using Microsoft.VisualBasic;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using System.Net;
using FluentFTP;




namespace SatisfatorySync.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
#pragma warning disable CA1822 // Mark members as static
        private string ColorRed { get; } = "#ffb3b3";
        private string ColorYellow { get; } = "#fcffb3";
        private string ColorGreen { get; } = "#b3ffb8";

        private byte[] LocalHeaderData { get; set; } = new byte[512];  // Initialize a byte array to hold 512 bytes.
        private byte[] RemoteHeaderData { get; set; } = new byte[512];  // Initialize a byte array to hold 512 bytes.

        private string _localGameName = "Local Game";
        public string LocalGameName
        {
            get => _localGameName;
            set => this.RaiseAndSetIfChanged(ref _localGameName, value);
        }

        private string _localSessionDefinition = "sess_steam";
        public string LocalSessionDefinition
        {
            get => _localSessionDefinition;
            set => this.RaiseAndSetIfChanged(ref _localSessionDefinition, value);
        }

        private string _localPlaytime = "10h 30m";
        public string LocalPlaytime
        {
            get => _localPlaytime;
            set => this.RaiseAndSetIfChanged(ref _localPlaytime, value);
        }

        private string _lastPushName = "Test User";
        public string LastPushName
        {
            get => _lastPushName;
            set => this.RaiseAndSetIfChanged(ref _lastPushName, value);
        }

        private string _lastPushDate = "2024-10-22";
        public string LastPushDate
        {
            get => _lastPushDate;
            set => this.RaiseAndSetIfChanged(ref _lastPushDate, value);
        }

        private string _remoteGameName = "Test Remote Game";
        public string RemoteGameName
        {
            get => _remoteGameName;
            set => this.RaiseAndSetIfChanged(ref _remoteGameName, value);
        }

        private string _remoteSessionDefinition = "Test Remote Session";
        public string RemoteSessionDefinition
        {
            get => _remoteSessionDefinition;
            set => this.RaiseAndSetIfChanged(ref _remoteSessionDefinition, value);
        }

        private string _remotePlaytime = "12h 45m";
        public string RemotePlaytime
        {
            get => _remotePlaytime;
            set => this.RaiseAndSetIfChanged(ref _remotePlaytime, value);
        }

        private string _remoteLastPushName = "Test Remote User";
        public string RemoteLastPushName
        {
            get => _remoteLastPushName;
            set => this.RaiseAndSetIfChanged(ref _remoteLastPushName, value);
        }

        private string _remoteLastPushDate = "2024-10-21";
        public string RemoteLastPushDate
        {
            get => _remoteLastPushDate;
            set => this.RaiseAndSetIfChanged(ref _remoteLastPushDate, value);
        }

        // Local backing fields for settings
        private string _name = string.Empty;
        private string _ftpAddress = string.Empty;
        private string _ftpUser = string.Empty;
        private string _ftpPassword = string.Empty;
        private string _filePath = string.Empty;
        private string _blueprintsPath = string.Empty;
        private bool _syncBlueprints = false;

        public string MyName
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }

        public string FtpAddress
        {
            get => _ftpAddress;
            set => this.RaiseAndSetIfChanged(ref _ftpAddress, value);
        }

        public string FtpUser
        {
            get => _ftpUser;
            set => this.RaiseAndSetIfChanged(ref _ftpUser, value);
        }

        public string FtpPassword
        {
            get => _ftpPassword;
            set => this.RaiseAndSetIfChanged(ref _ftpPassword, value);
        }

        public string FilePath
        {
            get => _filePath;
            set => this.RaiseAndSetIfChanged(ref _filePath, value);
        }

        public string BlueprintsPath
        {
            get => _blueprintsPath;
            set => this.RaiseAndSetIfChanged(ref _blueprintsPath, value);
        }

        public bool SyncBlueprints
        {
            get => _syncBlueprints;
            set => this.RaiseAndSetIfChanged(ref _syncBlueprints, value);
        }

        private int _selectedIndex;
        public int SelectedIndex
        {
            get => _selectedIndex;
            set => this.RaiseAndSetIfChanged(ref _selectedIndex, value);
        }

        public ObservableCollection<LogEntry> LogEntries { get; set; }

        private DispatcherTimer _timer;
        private LocalSettings _localSettings;
        List<FilePickerFileType> fileTypeList_XML { get; set; }

        // Reactive Commands
        public ReactiveCommand<Unit, Unit> ExportSettingsCommand { get; }
        public ReactiveCommand<Unit, Unit> ImportSettingsCommand { get; set; }
        public ReactiveCommand<Unit, Unit> SaveSettingsCommand { get; set; }

        public MainWindowViewModel() // Constructor
        {

            //Button Bindings
            ExportSettingsCommand = ReactiveCommand.CreateFromTask(CMDexportSettings);
            ImportSettingsCommand = ReactiveCommand.CreateFromTask(CMDimportSettings);
            SaveSettingsCommand = ReactiveCommand.CreateFromTask(CMDsaveSettings);

            //Initialisations
            initUpdateTimer();
            _localSettings = new LocalSettings();
            initLog();
            _selectedIndex = 0; // Default to showing the first tab

            //Filetypes
            // Create a list of FilePickerFileType instances
            fileTypeList_XML = new List<FilePickerFileType>
            {
                new FilePickerFileType("XML files")
                {
                Patterns = new List<string> { "*.xml" }.AsReadOnly(),
                MimeTypes = new List<string> { "application/xml" }.AsReadOnly()
                }
            };

            // load settings
            LoadSettingsOnStartup();
        }

        // initialization of the update Timer - add Callback function timer Tick event
        private void initUpdateTimer()
        {
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(2)
            };

            _timer.Tick += TimerTickUpdateCallback;

            _timer.Start();
        }

        // actual timer tick callback function for updating UI
        private void TimerTickUpdateCallback(object sender, EventArgs e)
        {
            // This will be called every 2 seconds
            //GetLocalHeaderData();
            //GetRemoteHeaderData();

        }

        // export settings
        private async Task CMDexportSettings()
        {
            var topLevel = GetMainWindow();

            if (topLevel != null)
            {
                var storageProvider = topLevel.StorageProvider;

                try
                {
                    var saveFileResult = await storageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
                    {
                        Title = "Save settings to file...",
                        FileTypeChoices = fileTypeList_XML,
                        SuggestedFileName = "SatfSyncSettings.xml"
                    });

                    // Check if a file path was selected
                    if (saveFileResult != null)
                    {

                        // copy text fields to _localSettings object
                        SaveSettings();

                        // Serialize the LocalSettings instance to XML with error handling
                        var serializer = new XmlSerializer(typeof(LocalSettings));
                        await using var stream = await saveFileResult.OpenWriteAsync();
                        using (var writer = new StreamWriter(stream))
                        {
                            serializer.Serialize(writer, _localSettings);
                        }

                        // Log successful export
                        LogMessage("settings export", $"successful: {saveFileResult.Path}", ColorGreen);
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions during the export process
                    LogMessage("settings export", $"failed: {ex.Message}", ColorRed);
                }

                // After saving, switch back to the Sync tab
                SelectedIndex = 0;
            }
        }

        private async Task CMDimportSettings()
        {
            var topLevel = GetMainWindow();

            if (topLevel != null)
            {
                var storageProvider = topLevel.StorageProvider;

                try
                {
                    var openFileResult = await storageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
                    {
                        Title = "Load settings from file...",
                        FileTypeFilter = fileTypeList_XML
                    });

                    if (openFileResult != null && openFileResult.Count > 0)
                    {
                        // Deserialize the LocalSettings instance from XML with error handling
                        var serializer = new XmlSerializer(typeof(LocalSettings));
                        await using var stream = await openFileResult[0].OpenReadAsync();

                        using (var reader = new StreamReader(stream))
                        {
                            _localSettings = (LocalSettings)serializer.Deserialize(reader);
                        }

                        // Copy Settings from Object to text fields
                        LoadSettings();

                        // Log successful loaded
                        LogMessage("settings load", $"successful: {openFileResult[0].Path}", ColorGreen);
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions during the load process
                    LogMessage("settings load", $"failed: {ex.Message}", ColorRed);
                }

                // After saving, switch back to the Sync tab
                SelectedIndex = 0;
            }
        }

        private async Task CMDsaveSettings()
        {
            // Copy text fields to settings Object
            SaveSettings();

            // Get the path to the user's application data directory
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var filePath = Path.Combine(appDataPath, "SatSync.conf");

            try
            {
                // Create the directory if it doesn't exist
                var directoryPath = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                // Serialize the LocalSettings instance to XML
                var serializer = new XmlSerializer(typeof(LocalSettings));
                await using var stream = new FileStream(filePath, FileMode.Create);
                using (var writer = new StreamWriter(stream))
                {
                    serializer.Serialize(writer, _localSettings);
                }

                // Log successful save
                LogMessage("settings save", $"successful: {filePath}", ColorGreen);
            }
            catch (Exception ex)
            {
                // Handle exceptions during the save process
                LogMessage("settings save", $"failed: {ex.Message}", ColorRed);
            }

            // After saving, switch back to the Sync tab
            SelectedIndex = 0;
        }

        private void LoadSettingsOnStartup()
        {
            // Get the path to the user's application data directory
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var filePath = Path.Combine(appDataPath, "SatSync.conf");

            try
            {
                // Check if the file exists
                if (File.Exists(filePath))
                {
                    // Deserialize the LocalSettings instance from XML
                    var serializer = new XmlSerializer(typeof(LocalSettings));
                    using (var stream = new FileStream(filePath, FileMode.Open))
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            _localSettings = (LocalSettings)serializer.Deserialize(reader);
                        }
                    }

                    // Load values into local properties
                    LoadSettings();

                    // Log successful load
                    LogMessage("settings load", $"successful: {filePath}", ColorGreen);
                }
                else
                {
                    LogMessage("settings load", $"file does not exist: {filePath}", ColorRed);
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions during the load process
                LogMessage("settings load", $"failed: {ex.Message}", ColorRed);
            }
        }

        private void SaveSettings()
        {
            // Copy text fields to settings Object
            _localSettings.Name = MyName;
            _localSettings.ftpAddress = FtpAddress;
            _localSettings.ftpUser = FtpUser;
            _localSettings.ftpPassword = FtpPassword;
            _localSettings.filePath = FilePath;
            _localSettings.blueprintsPath = BlueprintsPath;
            _localSettings.syncBlueprints = SyncBlueprints;

        }

        private void LoadSettings()
        {
            MyName = _localSettings.Name;
            FtpAddress = _localSettings.ftpAddress;
            FtpUser = _localSettings.ftpUser;
            FtpPassword = _localSettings.ftpPassword;
            FilePath = _localSettings.filePath;
            BlueprintsPath = _localSettings.blueprintsPath;
            SyncBlueprints = _localSettings.syncBlueprints;
        }
        private void initLog()
        {
            // Init Log
            LogEntries = new ObservableCollection<LogEntry>
            {
                new LogEntry { TimeStamp = DateTime.Now.ToString("o"), Action = "Application startup...", Result = "Completed", RowColor = ColorGreen},
            };
        }
        private void LogMessage(string action, string result, string rowColor)
        {
            LogEntries.Add(new LogEntry { TimeStamp = DateTime.Now.ToString("o"), Action = action, Result = result, RowColor = rowColor });
            SortLogEntries();
        }

        private void SortLogEntries()
        {
            // Sort LogEntries by TimeStamp in descending order
            var sorted = LogEntries.OrderByDescending(entry => DateTime.Parse(entry.TimeStamp)).ToList();

            // Clear the current collection and re-add sorted items
            LogEntries.Clear();
            foreach (var entry in sorted)
            {
                LogEntries.Add(entry);
            }
        }

        private Window GetMainWindow()
        {
            if (App.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime)
            {
                return lifetime.MainWindow; // Return the main window
            }
            return null; // Return null if the application lifetime is not of the expected type
        }

        // Method to read header data from a local file
        public void GetLocalHeaderData()
        {
            try
            {
                if (File.Exists(FilePath))
                {
                    using (FileStream fileStream = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
                    {
                        byte[] readData = new byte[512]; // Initialize a byte array for 512 bytes.
                        int bytesRead = fileStream.Read(readData, 0, readData.Length); // Read up to 512 bytes.

                        if (bytesRead < 512)
                        {
                            throw new InvalidOperationException("Local file must contain at least 512 bytes.");
                        }

                        LocalHeaderData = readData; // Store data

                        // Log a successful entry
                        LogMessage("Local header data read", "successful", ColorGreen);
                    }
                }
                else
                {
                    throw new FileNotFoundException($"The local file '{FilePath}' does not exist.");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions and log the error message
                LogMessage("Local header data read", $"failed: {ex.Message}", ColorRed);
            }
        }

        // Method to read header data from a remote FTP server
        public void GetRemoteHeaderData()
        {
            string fileName = Path.GetFileName(FilePath); // Extract the filename from FilePath
            string ftpPath = BuildFtpPath(fileName); // Construct the FTP path

            using (var client = new FtpClient(FtpAddress))
            {
                client.Credentials = new NetworkCredential(FtpUser, FtpPassword);
                try
                {
                    client.Connect(); // Connect to the FTP server

                    // Read the first 512 bytes
                    using (var stream = client.OpenRead(ftpPath))
                    {
                        byte[] readData = new byte[512];
                        int bytesRead = stream.Read(readData, 0, readData.Length);

                        if (bytesRead < 512)
                        {
                            throw new InvalidOperationException("FTP file must contain at least 512 bytes.");
                        }

                        RemoteHeaderData = readData; // Store data

                        // Log a successful entry
                        LogMessage("Remote header data read", "successful", ColorGreen);
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions and log the error message
                    LogMessage("Remote header data read", $"failed: {ex.Message}", ColorRed);
                }
            }
        }

        public string BuildFtpPath(string fileName)
        {
            // Ensure that the FTP address ends with a slash for proper URL formation
            if (!FtpAddress.EndsWith("/"))
            {
                FtpAddress += "/";
            }

            return $"{FtpAddress}{fileName}";
        }

    }
    public class LogEntry
    {
        public string TimeStamp { get; set; }
        public string Action { get; set; }
        public string Result { get; set; }
        public string RowColor { get; set; }
        // Read-only property for displaying the formatted date
        public string FormattedTimeStamp => FormatDate(TimeStamp);
        private string FormatDate(string timeStamp)
        {
            if (DateTime.TryParse(timeStamp, out DateTime dateTime))
            {
                return DateTime.Parse(timeStamp).ToString(); // Should format in local format
            }

            return timeStamp; // Return original if parsing fails
        }
    }

    public class LocalSettings
    {
        public string Name { get; set; } = string.Empty;
        public string ftpAddress { get; set; } = string.Empty;
        public string ftpUser { get; set; } = string.Empty;
        public string ftpPassword { get; set; } = string.Empty;
        public string filePath { get; set; } = string.Empty;
        public string blueprintsPath { get; set; } = string.Empty;
        public bool syncBlueprints { get; set; } = false;
    }

#pragma warning restore CA1822 // Mark members as static
}
