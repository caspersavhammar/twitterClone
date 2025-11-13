using CommunityToolkit.Mvvm.ComponentModel;
using tddd49.Models;

namespace tddd49.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty] private string _title;
    private NetworkManager NetworkManager { get; set; }
    public string Greeting { get; } = "Welcome to Avalonia!";

}