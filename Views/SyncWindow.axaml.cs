using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SatisfatorySync.ViewModels;

namespace SatisfatorySync.Views;

public partial class SyncWindow : Window
{
    public SyncWindow(SyncWindowViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel; // Set the DataContext for data binding
    }
}