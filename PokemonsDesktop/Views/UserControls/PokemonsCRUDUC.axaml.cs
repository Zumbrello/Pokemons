using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using PokemonsDesktop.ViewModels;

namespace PokemonsDesktop.Views.UserControls;

public partial class PokemonsCRUDUC : UserControl
{
    public PokemonsCRUDUC()
    {
        InitializeComponent();
        DataContext = PokemonsCRUDVM.GetInstance();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}