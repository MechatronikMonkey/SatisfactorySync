using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive;

namespace SatisfatorySync.ViewModels
{
    public class SyncWindowViewModel : ViewModelBase
    {
        public string ColorRed { get; } = "#ff9696";
        public string ColorYellow { get; } = "#f5fa64";
        public string ColorGreen { get; } = "#74fc7d";
        public string visibleTrue { get; } = "True";
        public string visibleFalse { get; } = "False";

        private string _buttonVisible = "False";
        public string ButtonVisible
        {
            get => _buttonVisible;
            set => this.RaiseAndSetIfChanged(ref _buttonVisible, value);
        }

        public ObservableCollection<SyncLogItem> SyncLogList { get; set; }

        public event EventHandler CloseRequested;

        public ReactiveCommand<Unit, bool> YesCommand { get; }
        public ReactiveCommand<Unit, bool> NoCommand { get; }

        public SyncWindowViewModel() // Constructor
        {
            // Initialize SyncLog
            initSyncLog();

            // Initialize commands
            YesCommand = ReactiveCommand.Create(() => true);
            NoCommand = ReactiveCommand.Create(() => false);
        }

        public void CloseWindow()
        {
            // Logic to signal that the window should close
            // You might need to invoke an event or similar to notify the View
            CloseRequested?.Invoke(this, EventArgs.Empty);
        }

        private void initSyncLog()
        {
            SyncLogList = new ObservableCollection<SyncLogItem>
            {
                new SyncLogItem { Text = "Connecting...", Status = " Ok", StatusVisible="False", StatusColor = ColorGreen},
            };
        }
        public void AddSyncLogItem(string text, string status, string visible, string statusColor)
        {
            SyncLogList.Add(new SyncLogItem { Text = text, Status = status, StatusColor = statusColor, StatusVisible = visible });
        }

        public void updateStatusLastItem(string status, string statuscolor, string statvisible)
        {
            if (SyncLogList.Count > 0)
            {
                // Get the last added item
                var lastItem = SyncLogList[SyncLogList.Count - 1];

                // Update the StatusVisible property
                lastItem.StatusVisible = statvisible;
                lastItem.Status = status;
                lastItem.StatusColor = statuscolor;
            }
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

    public class SyncLogItem : INotifyPropertyChanged
    {
        private string _text;
        private string _status;
        private string _statusColor;
        private string _statusVisible;

        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                OnPropertyChanged(nameof(Text));
            }
        }

        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged(nameof(Status));
            }
        }

        public string StatusColor
        {
            get => _statusColor;
            set
            {
                _statusColor = value;
                OnPropertyChanged(nameof(StatusColor));
            }
        }

        public string StatusVisible
        {
            get => _statusVisible;
            set
            {
                _statusVisible = value;
                OnPropertyChanged(nameof(StatusVisible));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
