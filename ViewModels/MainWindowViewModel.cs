using Avalonia.Data.Converters;
using Avalonia.Media;
using Avalonia.Threading;
using Microsoft.VisualBasic;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Globalization;

namespace SatisfatorySync.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
#pragma warning disable CA1822 // Mark members as static

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

        public MainWindowViewModel() // Constructor
        {
            // For testing add 4 dummy LogEntries - shall be removed later
            LogEntries = new ObservableCollection<LogEntry>
            {
                new LogEntry { TimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Action = "Initial Action", Result = "Completed", RowColor = "#b3ffb8"},
                new LogEntry { TimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Action = "Initial Action", Result = "Warning", RowColor = "#fcffb3"},
                new LogEntry { TimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Action = "Initial Action", Result = "Failure", RowColor = "#ffb3b3"},
                new LogEntry { TimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Action = "Initial Action", Result = "Completed", RowColor = "#b3ffb8" }
            };

            initUpdateTimer();
        }

        private void initUpdateTimer()
        {
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(2)
            };

            _timer.Tick += UpdateCallback;

            _timer.Start();
        }

        private void UpdateCallback(object sender, EventArgs e)
        {
            // This will be called every 2 seconds

            
            
        }

    }
    public class LogEntry
    {
        public string TimeStamp { get; set; }
        public string Action { get; set; }
        public string Result { get; set; }
        public string RowColor { get; set; }

    }
    
    #pragma warning restore CA1822 // Mark members as static
}
