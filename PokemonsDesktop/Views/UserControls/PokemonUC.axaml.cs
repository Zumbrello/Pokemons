using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using PokemonsDesktop.ViewModels;

namespace PokemonsDesktop.Views.UserControls;

public partial class PokemonUC : UserControl
{
    public PokemonUC()
    {
        InitializeComponent();
        DataContext = PokemonVM.GetInstance(null);
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}