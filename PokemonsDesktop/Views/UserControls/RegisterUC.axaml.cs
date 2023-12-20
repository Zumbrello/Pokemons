using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using PokemonsDesktop.ViewModels;

namespace PokemonsDesktop.Views.UserControls;

public partial class RegisterUC : UserControl
{
    public RegisterUC()
    {
        InitializeComponent();
        DataContext = RegisterVM.GetInstance();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}