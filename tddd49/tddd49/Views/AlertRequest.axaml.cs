using System.Diagnostics;
using Avalonia.Controls;
using tddd49.ViewModels;
using tddd49.Models;

namespace tddd49.Views;

public partial class AlertRequest : Window
{
    public AlertRequest(NetworkManager nm)
    {
        InitializeComponent();
        DataContext =  new AlertRequestViewModel(nm);
    }
}