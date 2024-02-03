using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using PokemonsDesktop.ViewModels;

namespace PokemonsDesktop.Views.UserControls;

public partial class AccountUC : UserControl
{
    public AccountUC()
    {
        InitializeComponent();
        DataContext = AccountVM.GetInstance();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}