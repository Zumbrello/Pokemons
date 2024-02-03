using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace PokemonsDesktop.ViewModels;

public partial class MenuUC : UserControl
{
    public MenuUC()
    {
        InitializeComponent();
        DataContext = MenuVM.GetInstance();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}