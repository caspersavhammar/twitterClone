using System.Diagnostics;
using Avalonia.Controls;
using tddd49.ViewModels;
using tddd49.Models;

namespace tddd49.Views;

public partial class AlertResponse : Window
{
    public AlertResponse()
    {
        InitializeComponent();
        DataContext = new AlertResponseViewModel();
    }
}