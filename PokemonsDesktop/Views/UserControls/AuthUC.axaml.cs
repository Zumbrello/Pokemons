using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using PokemonsDesktop.ViewModels;

namespace PokemonsDesktop.Views.UserControls;

public partial class AuthUC : UserControl
{
    public AuthUC()
    {
        InitializeComponent();
        DataContext = AuthVM.GetInstance();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}