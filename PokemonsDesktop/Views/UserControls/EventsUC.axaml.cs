using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using PokemonsDesktop.ViewModels;

namespace PokemonsDesktop.Views.UserControls;

public partial class EventsUC : UserControl
{
    public EventsUC()
    {
        InitializeComponent();
        DataContext = EventsVM.GetInstance();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}