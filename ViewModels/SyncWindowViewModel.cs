using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SatisfatorySync.ViewModels
{
    public class SyncWindowViewModel : ViewModelBase
    {
        private string ColorRed { get; } = "#ff9696";
        private string ColorYellow { get; } = "#f5fa64";
        private string ColorGreen { get; } = "#74fc7d";

        private string _buttonVisible = "False";
        public string ButtonVisible
        {
            get => _buttonVisible;
            set => this.RaiseAndSetIfChanged(ref _buttonVisible, value);
        }

        public ObservableCollection<SyncLogItem> SyncLogList { get; set; }

        public SyncWindowViewModel()
        {
            // Constructor logic here
            SyncLogList = new ObservableCollection<SyncLogItem>
            {
                new SyncLogItem { Text = "Connecting...", Status = " Ok", StatusColor = ColorGreen},
                new SyncLogItem { Text = "Receiving...", Status = " Warn", StatusColor = ColorYellow},
                new SyncLogItem { Text = "Change bla bla?", Status = " Fail", StatusColor = ColorRed}

            };
        }

        public void AddSyncLogItem(string text, string status, string statusColor)
        {
            SyncLogList.Add(new SyncLogItem { Text = text, Status = status, StatusColor = statusColor });
        }

        public void ClearSyncLog()
        {
            SyncLogList.Clear();
        }

        public void showButtons()
        {
            ButtonVisible = "True";
        }
        public void hideButtons()
        {
            ButtonVisible = "False";
        }
    }

    public class SyncLogItem
    {
        public string Text { get; set; }
        public string Status { get; set; }
        public string StatusColor { get; set; }
    }
}
