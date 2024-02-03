using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using PokemonsDesktop.ViewModels;

namespace PokemonsDesktop.Views.UserControls;

public partial class UsersCRUDUC : UserControl
{
    public UsersCRUDUC()
    {
        InitializeComponent();
        DataContext = UsersCRUDVM.GetInstance();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}