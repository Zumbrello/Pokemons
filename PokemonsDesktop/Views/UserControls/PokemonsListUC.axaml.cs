using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.Json;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using PokemonsDesktop.ViewModels;
using PokemonsDesktop.Models;

namespace PokemonsDesktop.Views.UserControls;

public partial class PokemonsListUC : UserControl
{
    private int GridSize = 8;
    public PokemonsListUC()
    {
        InitializeComponent();
        DataContext = PokemonsListVM.GetInstance();
        
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
