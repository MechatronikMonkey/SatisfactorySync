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
using System.Runtime.InteropServices;




namespace SatisfatorySync.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
#pragma warning disable CA1822 // Mark members as static
        private string ColorRed { get; } = "#ffb3b3";
        private string ColorYellow { get; } = "#fcffb3";
        private string ColorGreen { get; } = "#b3ffb8";
        private string NewItem { get; } = "--- NEW ---";

        private byte[] LocalHeaderData { get; set; } = new byte[512];  // Initialize a byte array to hold 512 bytes.
        private byte[] RemoteHeaderData { get; set; } = new byte[512];  // Initialize a byte array to hold 512 bytes.

        private ObservableCollection<string> _localSaveGameList;
        private ObservableCollection<string> _remoteSaveGameList;
        public ObservableCollection<string> LocalSaveGameList
        {
            get => _localSaveGameList;
            private set => this.RaiseAndSetIfChanged(ref _localSaveGameList, value);
        }
        public ObservableCollection<string> RemoteSaveGameList
        {
            get => _remoteSaveGameList;
            private set => this.RaiseAndSetIfChanged(ref _remoteSaveGameList, value);
        }

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
        private string _selectedFileLocal = string.Empty;
        private string _selectedFileRemote = string.Empty;

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

        public string SelectedFileLocal
        {
            get => _selectedFileLocal;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedFileLocal, value);

                // Set the corresponding value only if it's different from the current selection
                if (_selectedFileRemote != value && value != NewItem)
                {
                    // Check if the selected value is in RemoteSaveGameList
                    if (RemoteSaveGameList.Contains(value))
                    {
                        SelectedFileRemote = value; // Set the corresponding value if it exists
                    }
                    else
                    {
                        SelectedFileRemote = NewItem;
                    }
                }
            }
        }

        public string SelectedFileRemote
        {
            get => _selectedFileRemote;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedFileRemote, value);

                if (_selectedFileLocal != value && value != NewItem)
                {
                    // Check if the selected value is in RemoteSaveGameList
                    if (LocalSaveGameList.Contains(value))
                    {
                        SelectedFileLocal = value; // Set the corresponding value if it exists
                    }
                    else
                    {
                        SelectedFileLocal = NewItem;
                    }
                }
            }
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
        public ReactiveCommand<Unit, Unit> PickLocalFolderCommand { get; set; }
        public ReactiveCommand<Unit, Unit> PickBlueprintsFolderCommand { get; set; }
        public ReactiveCommand<Unit, Unit> RefreshLocalSaveGameListCommand { get; set; }
        public ReactiveCommand<Unit, Unit> RefreshRemoteSaveGameListCommand { get; set; }

        public MainWindowViewModel() // Constructor
        {

            //Button Bindings
            ExportSettingsCommand = ReactiveCommand.CreateFromTask(CMDexportSettings);
            ImportSettingsCommand = ReactiveCommand.CreateFromTask(CMDimportSettings);
            SaveSettingsCommand = ReactiveCommand.CreateFromTask(CMDsaveSettings);
            PickLocalFolderCommand = ReactiveCommand.CreateFromTask(CMDpickLocalFolder);
            PickBlueprintsFolderCommand = ReactiveCommand.CreateFromTask(CMDpickBlueprintsFolder);
            RefreshLocalSaveGameListCommand = ReactiveCommand.CreateFromTask(CMDrefreshLocalSaveGameList);
            RefreshRemoteSaveGameListCommand = ReactiveCommand.CreateFromTask(CMDrefreshRemoteSaveGameList);

            //Initialisations
            initUpdateTimer();
            _localSettings = new LocalSettings();
            initLog();
            _selectedIndex = 0; // Default to showing the first tab
            _localSaveGameList = new ObservableCollection<string>();
            _remoteSaveGameList = new ObservableCollection<string>();

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

            // try init savegame lists local and remote
            initGameListsOnStartup();
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
            GetLocalHeaderData();
            GetRemoteHeaderData();
            ParseLocalHeaderData();
            ParseRemoteHeaderData();
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
                    // Use User Home for default folder
                    string suggestedPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                    var suggestedFolder = await storageProvider.TryGetFolderFromPathAsync(suggestedPath);

                    var saveFileResult = await storageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
                    {
                        Title = "Save settings to file...",
                        FileTypeChoices = fileTypeList_XML,
                        SuggestedFileName = "SatfSyncSettings.xml",
                        SuggestedStartLocation = suggestedFolder
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
                    // Use User Home for default folder
                    string suggestedPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                    var suggestedFolder = await storageProvider.TryGetFolderFromPathAsync(suggestedPath);

                    var openFileResult = await storageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
                    {
                        Title = "Load settings from file...",
                        FileTypeFilter = fileTypeList_XML,
                        SuggestedStartLocation = suggestedFolder
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

        private async Task CMDpickLocalFolder()
        {
            var topLevel = GetMainWindow();

            if (topLevel != null)
            {
                var storageProvider = topLevel.StorageProvider;

                try
                {
                    IStorageFolder saveGameFolder = null;

                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    {
                        // Use %LOCALAPPDATA% on Windows
                        string localAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                        string saveGamePath = Path.Combine(localAppDataPath, "FactoryGame", "Saved", "SaveGames");

                        saveGameFolder = await storageProvider.TryGetFolderFromPathAsync(saveGamePath);
                    }
                    else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    {
                        // Use a different path on Linux
                        string homePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                        string saveGamePath = Path.Combine(homePath, ".config", "FactoryGame", "Saved", "SaveGames");

                        saveGameFolder = await storageProvider.TryGetFolderFromPathAsync(saveGamePath);
                    }
                    else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                    {
                        // Handle macOS... no reference to test... not implemented on osx for now.
                    }

                    // Open a folder picker dialog
                    var openFolderResult = await storageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
                    {
                        Title = "Select save games folder",
                        // Set the default start location to the user's home directory
                        SuggestedStartLocation = saveGameFolder
                    });

                    if (openFolderResult != null && openFolderResult.Count > 0)
                    {
                        // store selected folder path
                        FilePath = openFolderResult[0].TryGetLocalPath();

                        LogMessage("folder selection", $"successful: {openFolderResult[0].TryGetLocalPath()}", ColorGreen);
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions during the folder picking process
                    LogMessage("folder selection", $"failed: {ex.Message}", ColorRed);
                }
            }
        }

        private async Task CMDpickBlueprintsFolder()
        {
            var topLevel = GetMainWindow();

            if (topLevel != null)
            {
                var storageProvider = topLevel.StorageProvider;

                try
                {
                    IStorageFolder blueprintsFolder = null;

                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    {
                        // Use %LOCALAPPDATA% on Windows
                        string localAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                        string blueprintPath = Path.Combine(localAppDataPath, "FactoryGame", "Saved", "SaveGames", "blueprints");

                        blueprintsFolder = await storageProvider.TryGetFolderFromPathAsync(blueprintPath);
                    }
                    else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    {
                        // Use a different path on Linux
                        string homePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                        string blueprintPath = Path.Combine(homePath, ".config", "FactoryGame", "Saved", "SaveGames", "blueprints");

                        blueprintsFolder = await storageProvider.TryGetFolderFromPathAsync(blueprintPath);
                    }
                    else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                    {
                        // Handle macOS... no reference to test... not implemented on osx for now.
                    }

                    // Open a folder picker dialog
                    var openFolderResult = await storageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
                    {
                        Title = "Select blueprints folder",
                        // Set the default start location to the user's home directory
                        SuggestedStartLocation = blueprintsFolder
                    });

                    if (openFolderResult != null && openFolderResult.Count > 0)
                    {
                        // store selected folder path
                        BlueprintsPath = openFolderResult[0].TryGetLocalPath();

                        LogMessage("blueprints selection", $"successful: {openFolderResult[0].TryGetLocalPath()}", ColorGreen);
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions during the folder picking process
                    LogMessage("blueprints selection", $"failed: {ex.Message}", ColorRed);
                }
            }
        }

        // Task to load .sav files from the directory specified in FilePath
        private async Task CMDrefreshLocalSaveGameList()
        {
            _localSaveGameList.Clear(); // Clear existing items

            if (Directory.Exists(FilePath))
            {
                // Use Task.Run to perform I/O operation on a background thread
                var saveFiles = await Task.Run(() => Directory.GetFiles(FilePath, "*.sav"));

                foreach (var file in saveFiles)
                {
                    // Update the UI-bound collection on the UI thread safely
                    _localSaveGameList.Add(Path.GetFileName(file)); // Add file names to the collection
                }

                _localSaveGameList.Add(NewItem); // Add the "--- NEW ---" item once

                LogMessage("update local game list", "success", ColorGreen);
            }
            else
            {
                LogMessage("update local game list", "path not found! Check path to save games in settings.", ColorYellow);
            }
        }

        private async Task CMDrefreshRemoteSaveGameList()
        {
            _remoteSaveGameList.Clear(); // Clear existing items

            if (string.IsNullOrWhiteSpace(FtpAddress))
            {
                LogMessage("FTP credentials", "FTP address not set.", ColorYellow);
                return;
            }
            else if (string.IsNullOrWhiteSpace(FtpUser))
            {
                LogMessage("FTP credentials", "FTP user not set.", ColorYellow);
                return;
            }
            else if (string.IsNullOrWhiteSpace(FtpPassword))
            {
                LogMessage("FTP credentials", "FTP password not set", ColorYellow);
                return;
            }

            try
            {
                using (var client = new FtpClient(FtpAddress))
                {
                    client.Credentials = new NetworkCredential(FtpUser, FtpPassword);

                    await Task.Run(() => client.Connect());

                    // List all files in the FTP directory and filter for .sav files
                    var items = await Task.Run(() => client.GetListing("/"));

                    foreach (var item in items)
                    {
                        if (item.Type == FtpObjectType.File && item.Name.EndsWith(".sav", StringComparison.OrdinalIgnoreCase))
                        {
                            _remoteSaveGameList.Add(item.Name);
                        }
                    }

                    _remoteSaveGameList.Add(NewItem); // also Add the --- NEW --- item once.

                    LogMessage("update remote game list", $"success from {FtpAddress}", ColorGreen);
                }
            }
            catch (Exception ex)
            {
                LogMessage("FTP file list", $"Error: {ex.Message}", ColorRed);
            }
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

        private void initGameListsOnStartup()
        {
            if (_localSettings.ftpAddress != "" &&
                _localSettings.ftpUser != "" &&
                _localSettings.ftpPassword != "")
            {
                // try to update game list 
                // Call the async method without blocking the UI
                Task.Run(async () => await CMDrefreshRemoteSaveGameList()).Wait();
            }

            if (_localSettings.filePath != "")
            {
                // Call the async method without blocking the UI
                Task.Run(async () => await CMDrefreshLocalSaveGameList()).Wait();
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
            _localSettings.selectedFileLocal = SelectedFileLocal;
            _localSettings.selectedFileRemote = SelectedFileRemote;
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
            SelectedFileLocal = _localSettings.selectedFileLocal;
            SelectedFileRemote = _localSettings.selectedFileRemote;
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
            string _completeFilePath;

            // Ensure _selectedFileLocal is not null or empty
            if (!string.IsNullOrEmpty(_selectedFileLocal))
            {
                _completeFilePath = Path.Combine(FilePath, _selectedFileLocal);
            }
            else
            {
                // Handle the case where _selectedFileLocal is null or empty
                LogMessage("Selected local file", "empty", ColorYellow);
                _completeFilePath = null; // or set to a default value or raise an error
                return;
            }


            try
            {
                if (File.Exists(_completeFilePath))
                {
                    using (FileStream fileStream = new FileStream(_completeFilePath, FileMode.Open, FileAccess.Read))
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
            string fileName;

            // Ensure _selectedFileRemote is not null or empty or the new item
            if (!string.IsNullOrEmpty(_selectedFileRemote) && _selectedFileRemote != NewItem)
            {
                fileName = _selectedFileRemote;
            }
            else
            {
                // Handle the case where _selectedFileLocal is null or empty
                LogMessage("Selected remote file", "empty", ColorYellow);
                fileName = null; // or set to a default value or raise an error
                return;
            }

            string ftpPath = "/" + fileName; // Construct the FTP path

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

        private string ExtractSessionDefinition(byte[] headerData)
        {
            // Check if the header data is null or empty
            if (headerData == null || headerData.Length == 0)
            {
                return null; // Or handle this case as needed
            }

            // Convert the target word to a byte array
            byte[] targetWord = System.Text.Encoding.ASCII.GetBytes("SessionDefinition=");

            int position = IndexOfBytes(headerData, targetWord);

            // If the word is not found, return null
            if (position == -1)
            {
                return null; // the session definition was not found
            }

            // Start parsing the word after "SessionDefinition="
            int startIndex = position + targetWord.Length; // Move to the end of the found word
            List<byte> extractedData = new List<byte>();

            // Iterate until we hit a 0x00 byte
            while (startIndex < headerData.Length && headerData[startIndex] != 0x00)
            {
                extractedData.Add(headerData[startIndex]);
                startIndex++;
            }

            // Convert extracted ASCII bytes into a string
            return System.Text.Encoding.ASCII.GetString(extractedData.ToArray());
        }

        private string ExtractGameName(byte[] headerData, string _sessionDefinition)
        {
            // Check if the header data is null or empty
            if (headerData == null || headerData.Length == 0 || string.IsNullOrEmpty(_sessionDefinition))
            {
                return null; // Or handle this case as needed
            }

            // Convert the session definition to a byte array
            byte[] sessionDefinitionBytes = System.Text.Encoding.ASCII.GetBytes(_sessionDefinition);

            // Find the index of the session definition in the header data
            int position = IndexOfBytes(headerData, sessionDefinitionBytes);

            // If the session definition is not found, return null
            if (position == -1)
            {
                return null; // The session definition was not found
            }

            // Move the startIndex to the end of the session definition PLUS 2 to skip checksum
            // Now, we will skip any following 0x00 bytes
            int startIndex = position + sessionDefinitionBytes.Length + 2;

            // Skip over any 0x00 bytes
            while (startIndex < headerData.Length && headerData[startIndex] == 0x00)
            {
                startIndex++;
            }

            // Check if we've reached the end of the header data
            if (startIndex >= headerData.Length)
            {
                return null; // No Game Name found after session definition
            }

            List<byte> extractedGameName = new List<byte>();

            // Now iterate until we hit the next 0x00 byte
            while (startIndex < headerData.Length && headerData[startIndex] != 0x00)
            {
                extractedGameName.Add(headerData[startIndex]);
                startIndex++;
            }

            // Convert extracted ASCII bytes into a string
            return System.Text.Encoding.ASCII.GetString(extractedGameName.ToArray());
        }

        private TimeSpan? ExtractGameTime(byte[] headerData, string _gameName)
        {
            // Check if the header data is null or empty
            if (headerData == null || headerData.Length == 0 || string.IsNullOrEmpty(_gameName))
            {
                return null; // Or handle this case as needed
            }

            // Convert the game name to a byte array
            byte[] gameNameBytes = System.Text.Encoding.ASCII.GetBytes(_gameName);

            // Find the index of the game name in the header data
            int position = IndexOfBytes(headerData, gameNameBytes);

            // If the game name is not found, return null
            if (position == -1)
            {
                return null; // The game name was not found
            }

            // Move the startIndex to the end of the game name, skip 0x00 and checksum
            int startIndex = position + gameNameBytes.Length + 1; // +1 for 0x00 end byte of game name

            // Check if we've reached or exceeded the end of the header data
            if (startIndex + 4 > headerData.Length)
            {
                return null; // Not enough bytes for game time
            }

            // Extract the next 4 bytes for game time
            byte[] timeBytes = new byte[4];
            Array.Copy(headerData, startIndex, timeBytes, 0, 4); // Copy the next 4 bytes

            // Convert the byte array to an integer representing the time
            // Assuming the time is stored as a total seconds
            uint totalSeconds = BitConverter.ToUInt32(timeBytes, 0);

            // Construct a TimeSpan from total seconds
            TimeSpan timeSpan = TimeSpan.FromSeconds(totalSeconds);

            return timeSpan; // Return the DateTime representing the game time
        }
        private void ParseLocalHeaderData()
        {
            string sessionDefinition = null;
            string gameName = null;
            TimeSpan? gameTime;

            // Check if the selected local file is valid
            if (string.IsNullOrEmpty(_selectedFileLocal))
            {
                return; // Exit the method
            }
            else if (_selectedFileLocal == NewItem)
            {
                // If _selectedFileLocal is NewItem, log a warning and set fields empty.
                LocalGameName = string.Empty;
                LocalSessionDefinition = string.Empty;
                LocalPlaytime = string.Empty;
                LastPushName = string.Empty;
                LastPushDate = string.Empty;

                LogMessage("Warning: Selected local file is a new item. Parsing will be skipped.", string.Empty, ColorYellow);
                return; // Exit the method
            }

            // Proceed to extract values if the selected local file is valid
            sessionDefinition = ExtractSessionDefinition(LocalHeaderData);
            gameName = ExtractGameName(LocalHeaderData, sessionDefinition);
            gameTime = ExtractGameTime(LocalHeaderData, gameName);

            // Call the extracted method to retrieve the session definition
            if (sessionDefinition != null)
            {
                // Store or use the extracted session definition string
                LocalSessionDefinition = sessionDefinition;

                // Optionally, log or display the extracted string
                LogMessage("Extracted Session Definition:", sessionDefinition, ColorGreen);
            }
            else
            {
                // Handle the case where the word was not found
                LogMessage("SessionDefinition not found in LocalHeaderData", string.Empty, ColorRed);
            }

            if (gameName != null)
            {
                LocalGameName = gameName;
                LogMessage("Extracted Game Name:", gameName, ColorGreen);
            }
            else
            {
                LogMessage("Game Name not found after session definition", string.Empty, ColorRed);
            }

            if (gameTime.HasValue)
            {
                TimeSpan duration = gameTime.Value;
                string formattedDuration = $"{(int)duration.TotalHours:D2}H {duration.Minutes:D2}m {duration.Seconds:D2}s";
                LocalPlaytime = formattedDuration;
                LogMessage("Extracted Game Time:", formattedDuration, ColorGreen);
            }
            else
            {
                LogMessage("Game Time not found", string.Empty, ColorRed);
            }
        }

        private void ParseRemoteHeaderData()
        {
            string sessionDefinition = null;
            string gameName = null;
            TimeSpan? gameTime;

            // Check if the selected remote file is valid
            if (string.IsNullOrEmpty(_selectedFileRemote))
            {
                return; // Exit the method
            }
            else if (_selectedFileRemote == NewItem)
            {
                // If _selectedFileRemote is NewItem, log a warning and set fields empty.
                RemoteGameName = string.Empty;
                RemoteSessionDefinition = string.Empty;
                RemotePlaytime = string.Empty;
                RemoteLastPushName = string.Empty;
                RemoteLastPushDate = string.Empty;

                LogMessage("Warning: Selected remote file is a new item. Parsing will be skipped.", string.Empty, ColorYellow);
                return; // Exit the method
            }

            // Proceed to extract values if the selected remote file is valid
            sessionDefinition = ExtractSessionDefinition(RemoteHeaderData);
            gameName = ExtractGameName(RemoteHeaderData, sessionDefinition);
            gameTime = ExtractGameTime(RemoteHeaderData, gameName);

            // Call the extracted method to retrieve the session definition
            if (sessionDefinition != null)
            {
                // Store or use the extracted session definition string
                RemoteSessionDefinition = sessionDefinition; // Assuming you have this property
                LogMessage("Extracted Remote Session Definition:", sessionDefinition, ColorGreen);
            }
            else
            {
                // Handle the case where the word was not found
                LogMessage("Remote SessionDefinition not found in RemoteHeaderData", string.Empty, ColorRed);
            }

            if (gameName != null)
            {
                RemoteGameName = gameName; // Assuming you have this property for remote game name
                LogMessage("Extracted Remote Game Name:", gameName, ColorGreen);
            }
            else
            {
                LogMessage("Remote Game Name not found after remote session definition", string.Empty, ColorRed);
            }

            if (gameTime.HasValue)
            {
                TimeSpan duration = gameTime.Value;
                string formattedDuration = $"{(int)duration.TotalHours:D2}H {duration.Minutes:D2}m {duration.Seconds:D2}s";
                RemotePlaytime = formattedDuration; // Assuming you have this property for remote playtime
                LogMessage("Extracted Remote Game Time:", formattedDuration, ColorGreen);
            }
            else
            {
                LogMessage("Remote Game Time not found", string.Empty, ColorRed);
            }
        }

        int IndexOfBytes(byte[] haystack, byte[] needle)
        {
            // Randfallbehandlung: Falls das Suchmuster länger als das Byte-Array ist
            if (needle.Length == 0 || haystack.Length < needle.Length)
            {
                return -1;
            }

            // Schleife durch das Haupt-Array
            for (int i = 0; i <= haystack.Length - needle.Length; i++)
            {
                // Vergleich der Byte-Folge mit dem Suchmuster
                bool match = true;
                for (int j = 0; j < needle.Length; j++)
                {
                    if (haystack[i + j] != needle[j])
                    {
                        match = false;
                        break;
                    }
                }
                if (match)
                {
                    return i; // Rückgabe des Index des ersten Vorkommens
                }
            }

            return -1; // Wenn das Muster nicht gefunden wird
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
        public string selectedFileLocal { get; set; } = string.Empty;
        public string selectedFileRemote { get; set; } = string.Empty;
        public string blueprintsPath { get; set; } = string.Empty;
        public bool syncBlueprints { get; set; } = false;
    }

#pragma warning restore CA1822 // Mark members as static
}
