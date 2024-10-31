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




namespace SatisfatorySync.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
#pragma warning disable CA1822 // Mark members as static
        private string ColorRed { get; } = "#ffb3b3";
        private string ColorYellow { get; } = "#fcffb3";
        private string ColorGreen { get; } = "#b3ffb8";

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

        public ObservableCollection<LogEntry> LogEntries { get; set; }

        private DispatcherTimer _timer;
        public LocalSettings _localSettings { get; }
        List<FilePickerFileType> fileTypeList_XML { get; set; }

        // Reactive Commands
        public ReactiveCommand<Unit, Unit> ExportSettingsCommand { get; }

        public MainWindowViewModel() // Constructor
        {
            // For testing add 4 dummy LogEntries - shall be removed later
            LogEntries = new ObservableCollection<LogEntry>
            {
                new LogEntry { TimeStamp = DateTime.Now.ToString("o"), Action = "Initial Action", Result = "Completed", RowColor = "#b3ffb8"},
                new LogEntry { TimeStamp = DateTime.Now.ToString("o"), Action = "Initial Action", Result = "Warning", RowColor = "#fcffb3"},
                new LogEntry { TimeStamp = DateTime.Now.ToString("o"), Action = "Initial Action", Result = "Failure", RowColor = "#ffb3b3"},
            };

            //Button Bindings
            ExportSettingsCommand = ReactiveCommand.CreateFromTask(exportSettings);

            //Initialisations
            initUpdateTimer();
            _localSettings = new LocalSettings();

            //Filetypes
            // Create a list of FilePickerFileType instances
            fileTypeList_XML = new List<FilePickerFileType>
            {
                new FilePickerFileType("XML files")
                {
                Patterns = new List<string> { "*.xml" }.AsReadOnly(),
                MimeTypes = new List<string> { "application/xml" }.AsReadOnly()
                }
                 // Add more FilePickerFileType instances as needed
            };
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

        }

        // export settings
        private async Task exportSettings()
        {
            var topLevel = GetMainWindow();

            if (topLevel != null)
            {
                // Use the StorageProvider API to show the save dialog
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
                        // Serialize the LocalSettings instance to XML
                        var serializer = new XmlSerializer(typeof(LocalSettings));
                        await using var stream = await saveFileResult.OpenWriteAsync();
                        using (var writer = new StreamWriter(stream))
                        {
                            serializer.Serialize(writer, _localSettings);
                        }

                        // Log successful export
                        LogEntries.Add(new LogEntry { TimeStamp = DateTime.Now.ToString("o"), Action = "settings export", Result = "successful", RowColor = ColorGreen });
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions, e.g., log the error
                    LogEntries.Add(new LogEntry { TimeStamp = DateTime.Now.ToString("o"), Action = "settings export", Result = $"failed: {ex.Message}", RowColor = ColorRed });
                }
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
    }
    public class LogEntry
    {
        public string TimeStamp { get; set; }
        public string Action { get; set; }
        public string Result { get; set; }
        public string RowColor { get; set; }

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
