using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Collections.ObjectModel;
using System.Globalization;

namespace SatisfatorySync.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        #pragma warning disable CA1822 // Mark members as static

        public string LocalGameName { get; set; } = "SatisfactoryTestGame";
        public string LocalSessionDefinition { get; set; } = "sess_steam";
        public string LocalPlaytime { get; set; } = "10h 30m";
        public string LastPushName { get; set; } = "Test User";
        public string LastPushDate { get; set; } = "2024-10-22";
        public string RemoteGameName { get; set; } = "Test Remote Game";
        public string RemoteSessionDefinition { get; set; } = "Test Remote Session";
        public string RemotePlaytime { get; set; } = "12h 45m";
        public string RemoteLastPushName { get; set; } = "Test Remote User";
        public string RemoteLastPushDate { get; set; } = "2024-10-21";
        public ObservableCollection<LogEntry> LogEntries { get; set; }

        public MainWindowViewModel() // Constructor
        {
            LogEntries = new ObservableCollection<LogEntry>
            {
                new LogEntry { TimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Action = "Initial Action", Result = "Completed", RowColor = "#b3ffb8"},
                new LogEntry { TimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Action = "Initial Action", Result = "Warning", RowColor = "#fcffb3"},
                new LogEntry { TimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Action = "Initial Action", Result = "Failure", RowColor = "#ffb3b3"},
                new LogEntry { TimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Action = "Initial Action", Result = "Completed", RowColor = "#b3ffb8" }
            };
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
