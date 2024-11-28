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
using SatisfatorySync.Views;
using static System.Collections.Specialized.BitVector32;
using Tmds.DBus.Protocol;
using static SatisfatorySync.ViewModels.MainWindowViewModel;
using FluentFTP.Helpers;
using System.Text.Json;
using System.Text.Json.Serialization;




namespace SatisfatorySync.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
#pragma warning disable CA1822 // Mark members as static
        private string ColorRed { get; } = "#ffb3b3";
        private string ColorYellow { get; } = "#fcffb3";
        private string ColorGreen { get; } = "#b3ffb8";
        private string NewItem { get; } = "--- NEW ---";

        private TimeSpan? RawRemotePlaytime { get; set; }
        private TimeSpan? RawLocalPlaytime { get; set; }

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

        private string _lastDownloadName = "Test User";
        public string LastDownloadName
        {
            get => _lastDownloadName;
            set => this.RaiseAndSetIfChanged(ref _lastDownloadName, value);
        }

        private string _lastDownloadDate = "2024-10-22";
        public string LastDownloadDate
        {
            get => _lastDownloadDate;
            set => this.RaiseAndSetIfChanged(ref _lastDownloadDate, value);
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

        private string _lastUploadName = "Test Remote User";
        public string LastUploadName
        {
            get => _lastUploadName;
            set => this.RaiseAndSetIfChanged(ref _lastUploadName, value);
        }

        private string _lastUploadDate = "2024-10-21";
        public string LastUploadDate
        {
            get => _lastUploadDate;
            set => this.RaiseAndSetIfChanged(ref _lastUploadDate, value);
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
                if (_selectedFileRemote != value && value != NewItem && value != null)
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

                if (_selectedFileLocal != value && value != NewItem && value != null)
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

        // Sync status
        public enum SyncStatusType
        {
            Upload,
            Download,
            Backup,
            Restore
        }

        // Reactive Commands
        public ReactiveCommand<Unit, Unit> ExportSettingsCommand { get; }
        public ReactiveCommand<Unit, Unit> ImportSettingsCommand { get; set; }
        public ReactiveCommand<Unit, Unit> SaveSettingsCommand { get; set; }
        public ReactiveCommand<Unit, Unit> PickLocalFolderCommand { get; set; }
        public ReactiveCommand<Unit, Unit> PickBlueprintsFolderCommand { get; set; }
        public ReactiveCommand<Unit, Unit> RefreshLocalSaveGameListCommand { get; set; }
        public ReactiveCommand<Unit, Unit> RefreshRemoteSaveGameListCommand { get; set; }
        public ReactiveCommand<Unit, Unit> StartUploadCommand { get; set; }

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
            StartUploadCommand = ReactiveCommand.Create(CMDstartUPLOAD);

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

                    client.Disconnect();
                }
            }
            catch (Exception ex)
            {
                LogMessage("FTP file list", $"Error: {ex.Message}", ColorRed);
            }
        }

        private async void CMDstartUPLOAD()
        {
            // If local file is not selected - exit and error
            if (string.IsNullOrEmpty(_selectedFileLocal))
            {
                LogMessage("Local file", "Not selected!", ColorRed);
                return;
            }

            var viewModel = new SyncWindowViewModel();
            var syncWindow = new SyncWindow(viewModel);
            syncWindow.Show(GetMainWindow());

            // Stop update Loop and wait
            StopUpdateTimer();
            await Task.Delay(2000);

            // Test FTP connection
            bool ftpConSuccess = await testFTPConnection(viewModel);
            if (!ftpConSuccess)
            {
                viewModel.CloseWindow();
                StartUpdateTimer();
                return;
            }

            // Do we upload a new file?
            if (SelectedFileRemote == NewItem)
            {
                if (await askCreateNewFile(viewModel))
                {
                    string _completeFilePath = Path.Combine(FilePath, _selectedFileLocal);

                    // Check if file exists on server if YES exit with error
                    if (FileExists("/" + SelectedFileLocal))
                    {
                        viewModel.AddSyncLogItem("File already exists on server", "", viewModel.visibleTrue, viewModel.ColorRed);
                        viewModel.AddSyncLogItem(" ", "Fail", viewModel.visibleTrue, viewModel.ColorRed);
                        exitWindowWithError(viewModel);
                        return;
                    }

                    // Create sync file
                    string localTempFilePath = Path.GetTempFileName();
                    viewModel.AddSyncLogItem("Create sync file...", " Ok", viewModel.visibleTrue, viewModel.ColorGreen);

                    viewModel.AddSyncLogItem("Upload save game...", " Ok", viewModel.visibleFalse, viewModel.ColorGreen);

                    // Upload file to server with same name as local file
                    if (UploadFile(_completeFilePath, "/" + SelectedFileLocal))
                    {
                        // Successfully uploaded the save game
                        appendToSyncLog(localTempFilePath, SyncStatusType.Upload, $"first creation of file.");
                        await Task.Delay(700);
                        viewModel.updateStatusLastItem(" Ok", viewModel.ColorGreen, viewModel.visibleTrue);

                        // Update Game List Remote
                        await CMDrefreshRemoteSaveGameList();
                    }
                    else
                    {
                        // Upload not successfull
                        appendToSyncLog(localTempFilePath, SyncStatusType.Upload, "Error on file Upload");
                        await Task.Delay(600);
                        viewModel.updateStatusLastItem(" Fail", viewModel.ColorRed, viewModel.visibleTrue);
                        exitWindowWithError(viewModel);
                        return;
                    }

                    // Extract filename for json file
                    string newFileName = buildSyncFileName(_selectedFileLocal);

                    // Upload sync file - overwrite if one exists, but shouldnt at this point
                    viewModel.AddSyncLogItem("Upload sync file...", " Ok", viewModel.visibleFalse, viewModel.ColorGreen);
                    if (UploadFile(localTempFilePath, newFileName, true))
                    {
                        await Task.Delay(700);
                        viewModel.updateStatusLastItem(" Ok", viewModel.ColorGreen, viewModel.visibleTrue);
                        LogMessage("Upload save game", $"success to {FtpAddress}", ColorGreen);

                        exitWindowNoError(viewModel);

                        // Set selected file for remote same
                        SelectedFileRemote = SelectedFileLocal;

                        // Save settings
                        await CMDsaveSettings();

                        // Clean up temporary file
                        File.Delete(localTempFilePath);

                        return;
                    }
                    else
                    {
                        // Upload not successfull
                        await Task.Delay(800);
                        viewModel.updateStatusLastItem(" Fail", viewModel.ColorRed, viewModel.visibleTrue);
                        exitWindowWithError(viewModel);

                        // Clean up temporary file
                        File.Delete(localTempFilePath);

                        return;
                    }
                }
                else
                {
                    LogMessage("User abort on", "Server File New", ColorRed);
                    exitWindowWithError(viewModel);
                    return;
                }
            }
            else // must be existing file; overwrite it after backup
            {
                // Download sync file
                // Create sync file temp
                string localTempFilePath = Path.GetTempFileName();
                viewModel.AddSyncLogItem("Download sync file...", " Ok", viewModel.visibleFalse, viewModel.ColorGreen);

                // create ftp path from selected file for sync file
                string ftpPath = "/" + buildSyncFileName(_selectedFileLocal);

                if (DownloadFile(ftpPath, localTempFilePath, true))
                {
                    // Download of file successfull
                    await Task.Delay(600);
                    viewModel.updateStatusLastItem(" Ok", viewModel.ColorGreen, viewModel.visibleTrue);
                }
                else
                {
                    // Error on download
                    await Task.Delay(800);
                    viewModel.updateStatusLastItem(" Fail", viewModel.ColorRed, viewModel.visibleTrue);
                    exitWindowWithError(viewModel);
                    return;
                }
                // Read last lines in array
                SyncStatus[] lastEntries = ReadLastSyncLogs(localTempFilePath, 1);

                // Last Item Download/Upload?
                if (lastEntries[0].SyncType == SyncStatusType.Download)
                {
                    viewModel.AddSyncLogItem("Last sync state DOWNLOAD", " Ok", viewModel.visibleFalse, viewModel.ColorGreen);
                    // If it was a Download, check if we where the last user of download
                    // If so, we can assume that no one downloaded the file after us.
                    // Go ahead.
                    if (lastEntries[0].User == MyName)
                    {
                        viewModel.AddSyncLogItem($"Last sync name '{MyName}'", " Ok", viewModel.visibleFalse, viewModel.ColorGreen);
                    }
                    else
                    {
                        // Tell the User that another person downloaded the game in the meantime
                        viewModel.AddSyncLogItem($"Last sync name '{lastEntries[0].User}'", " Fail", viewModel.visibleFalse, viewModel.ColorGreen);
                        viewModel.AddSyncLogItem($"User '{lastEntries[0].User}' downloaded file parallel to you.", "", viewModel.visibleFalse, viewModel.ColorGreen);
                        if (!await askContinueUpload(viewModel))
                        {
                            // User aborted! 
                            exitWindowWithError(viewModel);
                            return;
                        }
                    }

                    // Only reach this point if user want to upload or continue upload
                    // Check if Playtime LOCAL is BIGGER than on server - so update header Data
                    viewModel.AddSyncLogItem($"Update Header Data", " Done.", viewModel.visibleFalse, viewModel.ColorGreen);
                    GetLocalHeaderData();
                    GetRemoteHeaderData();
                    ParseLocalHeaderData();
                    ParseRemoteHeaderData();
                    viewModel.updateStatusLastItem(" Ok", viewModel.ColorGreen, viewModel.visibleTrue);

                    if (!(RawLocalPlaytime > RawRemotePlaytime))
                    {
                        // NO
                        // Tell the User that playtime on server is bigger than the one he wants to upload
                        viewModel.AddSyncLogItem($"Playtime missmatch!", "", viewModel.visibleFalse, viewModel.ColorGreen);
                        viewModel.AddSyncLogItem($"Local Playtime: {LocalPlaytime}", "", viewModel.visibleFalse, viewModel.ColorGreen);
                        viewModel.AddSyncLogItem($"Remote Playtime: {RemotePlaytime}", "", viewModel.visibleFalse, viewModel.ColorGreen);
                        if (!await askContinueUpload(viewModel))
                        {
                            // User aborted! 
                            exitWindowWithError(viewModel);
                            return;
                        }
                    }
                    // YES Path - only reach here if user wants to overwrite the file on server
                }
                else if (lastEntries[0].SyncType == SyncStatusType.Upload)
                {
                    // Tell the User that last item was upload by a specific user and if he wants to overwrite file?
                    viewModel.AddSyncLogItem($"User '{lastEntries[0].User}' uploaded file after your download!", "", viewModel.visibleFalse, viewModel.ColorGreen);
                    viewModel.AddSyncLogItem($"Last sync name '{lastEntries[0].User}'", "", viewModel.visibleFalse, viewModel.ColorGreen);
                    if (!await askContinueUpload(viewModel))
                    {
                        // User aborted! 
                        exitWindowWithError(viewModel);
                        return;
                    }

                }
                else
                {
                    // Should not happen! 
                    exitWindowWithError(viewModel);
                    return;
                }

                // Only reach here if user wants to overwrite the savegame
                // Overwrite it and give feedback now
                HandleExistingFileOverwrite(viewModel);
            }
        }

        private void HandleExistingFileOverwrite(SyncWindowViewModel viewModel)
        {
            string serverFilePath = "/" + SelectedFileLocal;
            string backupFilePath = "/backup_" + SelectedFileLocal; // Adjust naming convention as necessary
            string localTempFilePath = Path.GetTempFileName();

            viewModel.AddSyncLogItem("Backup of existing file downloaded...", " Ok", viewModel.visibleFalse, viewModel.ColorGreen);
            // Step 1: Download current server file to create a backup
            if (DownloadFile(serverFilePath, localTempFilePath, true))
            {
                viewModel.updateStatusLastItem(" Ok.", viewModel.ColorGreen, viewModel.visibleTrue);
            }
            else
            {
                viewModel.updateStatusLastItem(" Fail!", viewModel.ColorRed, viewModel.visibleTrue);
                exitWindowWithError(viewModel);
                return;
            }

            // Step 2: Upload the downloaded file as a backup
            viewModel.AddSyncLogItem("Backup upload...", " Ok", viewModel.visibleFalse, viewModel.ColorGreen);

            if (UploadFile(localTempFilePath, backupFilePath, true))
            {
                viewModel.updateStatusLastItem(" Ok.", viewModel.ColorGreen, viewModel.visibleTrue);
            }
            else
            {
                viewModel.updateStatusLastItem(" Fail!", viewModel.ColorRed, viewModel.visibleTrue);
                exitWindowWithError(viewModel);
                return;
            }

            // Step 3: Upload the new local file to overwrite the existing server file
            string completeLocalFilePath = Path.Combine(FilePath, _selectedFileLocal);
            viewModel.AddSyncLogItem("Uploading new file...", " In Progress", viewModel.visibleTrue, viewModel.ColorYellow); // Show in progress

            if (UploadFile(completeLocalFilePath, serverFilePath, true)) // Overwrite existing server file
            {
                viewModel.updateStatusLastItem(" Ok.", viewModel.ColorGreen, viewModel.visibleTrue);
                LogMessage("Upload save game", $"success to {FtpAddress}", ColorGreen);
            }
            else
            {
                viewModel.updateStatusLastItem(" Fail!", viewModel.ColorRed, viewModel.visibleTrue);
                exitWindowWithError(viewModel);
                return;
            }

            // Clean up temporary file
            File.Delete(localTempFilePath);

            // Additional feedback can be provided here if needed
            exitWindowNoError(viewModel);
        }

        private string buildSyncFileName(string localFileName)
        {
            string newFileName = localFileName.Substring(0, localFileName.Length - 4);
            return "/" + newFileName + ".json";
        }
        private async void exitWindowWithError(SyncWindowViewModel viewmodel)
        {
            viewmodel.AddSyncLogItem("Exiting...", "Now!", viewmodel.visibleTrue, viewmodel.ColorRed);
            await Task.Delay(2300);
            viewmodel.CloseWindow();
            StartUpdateTimer();
        }

        private async void exitWindowNoError(SyncWindowViewModel viewmodel)
        {
            viewmodel.AddSyncLogItem("Exiting...", "Now", viewmodel.visibleTrue, viewmodel.ColorGreen);
            await Task.Delay(1700);
            viewmodel.CloseWindow();
            StartUpdateTimer();
        }
        private void appendToSyncLog(string file, SyncStatusType syncstat, string message)
        {
            // Prepare new log entry
            var syncStatus = new SyncStatus
            {
                Timestamp = DateTime.UtcNow,
                User = _name,
                SyncType = syncstat,
                Message = message,
                Playtime = _localPlaytime
            };

            // Configure JsonSerializerOptions to include the JsonStringEnumConverter
            var options = new JsonSerializerOptions
            {
                WriteIndented = true // Optional: Makes the JSON output more readable
            };
            options.Converters.Add(new JsonStringEnumConverter());

            // Append the new log entry as JSON
            using (StreamWriter sw = new StreamWriter(file, true)) // True to append
            {
                string jsonLog = JsonSerializer.Serialize(syncStatus);
                sw.WriteLine(jsonLog);
            }
        }

        private SyncStatus[] ReadLastSyncLogs(string filePath, int numberOfLines = 4)
        {
            try
            {
                // Read all lines from the file
                var lines = File.ReadLines(filePath).Reverse().Take(numberOfLines).ToArray(); // Get last N lines

                // Prepare an array for the parsed SyncStatus objects
                SyncStatus[] syncStatuses = new SyncStatus[lines.Length];

                // Deserialize each line into a SyncStatus object
                for (int i = 0; i < lines.Length; i++)
                {
                    syncStatuses[i] = JsonSerializer.Deserialize<SyncStatus>(lines[i]);
                }

                return syncStatuses;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading log file: {ex.Message}");
                return Array.Empty<SyncStatus>(); // Return an empty array on error
            }
        }

        public bool FileExists(string filePath)
        {
            using (FtpClient client = new FtpClient(_ftpAddress))
            {
                client.Credentials = new System.Net.NetworkCredential(_ftpUser, _ftpPassword);
                client.Connect();

                return client.FileExists(filePath);
            }
        }

        public bool UploadFile(string localPath, string remotePath, bool overwrite = false)
        {
            using (FtpClient client = new FtpClient(_ftpAddress))
            {
                client.Credentials = new System.Net.NetworkCredential(_ftpUser, _ftpPassword);
                client.Connect();

                try
                {
                    client.UploadFile(localPath, remotePath, overwrite ? FtpRemoteExists.Overwrite : FtpRemoteExists.Skip);
                    client.Disconnect();
                    return true; // Successfully uploaded
                }
                catch (Exception ex)
                {
                    client.Disconnect();
                    LogMessage($"Error uploading file:", $"{ex.Message}", ColorRed);
                    return false; // Failed to upload
                }
            }
        }

        public bool DownloadFile(string remotePath, string localPath, bool overwrite = false)
        {
            using (FtpClient client = new FtpClient(_ftpAddress))
            {
                client.Credentials = new System.Net.NetworkCredential(_ftpUser, _ftpPassword);
                client.Connect();

                // Check if the file exists before downloading
                if (client.FileExists(remotePath))
                {
                    try
                    {
                        client.DownloadFile(localPath, remotePath);
                        return true; // Successfully downloaded
                    }
                    catch (Exception ex)
                    {
                        LogMessage("Error downloading file", $" {ex.Message}", ColorRed);
                        return false; // Failed to download
                    }
                }
                else
                {
                    // Create an empty file if it does not exist
                    try
                    {
                        File.Create(localPath).Dispose();
                        return true; // Successfully created an empty file
                    }
                    catch (Exception ex)
                    {
                        LogMessage("Error creating local file", $" {ex.Message}", ColorRed);
                        return false; // Failed to create an empty file
                    }
                }
            }
        }
        private async Task<bool> askCreateNewFile(SyncWindowViewModel viewmodel)
        {
            //Ask User if he wants to create new file
            await Task.Delay(1000);
            viewmodel.AddSyncLogItem("Upload new file!", "", viewmodel.visibleTrue, viewmodel.ColorYellow);
            viewmodel.AddSyncLogItem(" ", "Create New?", viewmodel.visibleTrue, viewmodel.ColorYellow);
            await Task.Delay(500);
            viewmodel.showButtons();
            // Wait for button click and than return
            bool clickResult = await WaitForButtonClick(viewmodel);
            viewmodel.hideButtons();

            return clickResult;
        }

        private async Task<bool> askContinueUpload(SyncWindowViewModel viewmodel)
        {
            //Ask User if he wants to create new file
            await Task.Delay(1000);
            viewmodel.AddSyncLogItem("Overwrite on server!", "", viewmodel.visibleTrue, viewmodel.ColorYellow);
            viewmodel.AddSyncLogItem(" ", "Continue Upload?", viewmodel.visibleTrue, viewmodel.ColorYellow);
            await Task.Delay(500);
            viewmodel.showButtons();
            // Wait for button click and than return
            bool clickResult = await WaitForButtonClick(viewmodel);
            viewmodel.hideButtons();

            return clickResult;
        }

        private async Task<bool> testFTPConnection(SyncWindowViewModel viewmodel)
        {
            try
            {
                var client = new FtpClient(FtpAddress, FtpUser, FtpPassword);
                client.Connect();
                viewmodel.updateStatusLastItem(" Ok", viewmodel.ColorGreen, viewmodel.visibleTrue);
                client.Disconnect();
                return true;
            }
            catch (Exception ex)
            {
                viewmodel.updateStatusLastItem(" Fail", viewmodel.ColorRed, viewmodel.visibleTrue);
                viewmodel.AddSyncLogItem($"Connection failed: {ex.Message}", "", viewmodel.visibleTrue, viewmodel.ColorRed);
                viewmodel.AddSyncLogItem($"Click any Button to close window...", "", viewmodel.visibleTrue, viewmodel.ColorRed);
                viewmodel.showButtons();

                // Wait for button click and than return
                bool clickResult = await WaitForButtonClick(viewmodel);
                LogMessage("FTP connection", $"Error: {ex.Message}", ColorRed);
                return false;

            }
        }
        // Method to control the timer
        private void StopUpdateTimer()
        {
            _timer?.Stop();
        }
        private void StartUpdateTimer()
        {
            _timer?.Start();
        }

        private async Task<bool> WaitForButtonClick(SyncWindowViewModel viewModel)
        {
            // Create an observable to return the button click result
            return await viewModel.YesCommand.Select(result => result).FirstAsync()
                   .Merge(viewModel.NoCommand.Select(result => result)).FirstAsync();
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
                        //LogMessage("Local header data read", "successful", ColorGreen);
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
                        //LogMessage("Remote header data read", "successful", ColorGreen);
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
                LastUploadName = string.Empty;
                LastUploadDate = string.Empty;

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
                //LogMessage("Extracted Session Definition:", sessionDefinition, ColorGreen);
            }
            else
            {
                // Handle the case where the word was not found
                LogMessage("SessionDefinition not found in LocalHeaderData", string.Empty, ColorRed);
            }

            if (gameName != null)
            {
                LocalGameName = gameName;
                //LogMessage("Extracted Game Name:", gameName, ColorGreen);
            }
            else
            {
                LogMessage("Game Name not found after session definition", string.Empty, ColorRed);
            }

            if (gameTime.HasValue)
            {
                RawLocalPlaytime = gameTime.Value; // Save raw time for later
                TimeSpan duration = gameTime.Value;
                string formattedDuration = $"{(int)duration.TotalHours:D2}H {duration.Minutes:D2}m {duration.Seconds:D2}s";
                LocalPlaytime = formattedDuration;
                //LogMessage("Extracted Game Time:", formattedDuration, ColorGreen);
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
                LastUploadName = string.Empty;
                LastUploadDate = string.Empty;

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
                //LogMessage("Extracted Remote Session Definition:", sessionDefinition, ColorGreen);
            }
            else
            {
                // Handle the case where the word was not found
                LogMessage("Remote SessionDefinition not found in RemoteHeaderData", string.Empty, ColorRed);
            }

            if (gameName != null)
            {
                RemoteGameName = gameName; // Assuming you have this property for remote game name
                //LogMessage("Extracted Remote Game Name:", gameName, ColorGreen);
            }
            else
            {
                LogMessage("Remote Game Name not found after remote session definition", string.Empty, ColorRed);
            }

            if (gameTime.HasValue)
            {
                RawRemotePlaytime = gameTime.Value;
                TimeSpan duration = gameTime.Value;
                string formattedDuration = $"{(int)duration.TotalHours:D2}H {duration.Minutes:D2}m {duration.Seconds:D2}s";
                RemotePlaytime = formattedDuration; // Assuming you have this property for remote playtime
                //LogMessage("Extracted Remote Game Time:", formattedDuration, ColorGreen);
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

    public class SyncStatus
    {
        public DateTime Timestamp { get; set; }
        public string User { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))] // This attribute will format the enum as a string

        public SyncStatusType SyncType { get; set; }
        public string Playtime { get; set; }
        public string Message { get; set; }
    }

#pragma warning restore CA1822 // Mark members as static
}
