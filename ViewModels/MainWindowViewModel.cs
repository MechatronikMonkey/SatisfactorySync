using System;
using System.Collections.ObjectModel;

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


        public MainWindowViewModel() // Constructor
        {
            
        }

#pragma warning restore CA1822 // Mark members as static
    }
}
