using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
using ReactiveUI;
using System.Configuration;
using System.Runtime.InteropServices;
using Avalonia.Controls;
using Microsoft.CodeAnalysis;

namespace PokemonsDesktop.ViewModels;

public class MainWindowVM : ViewModelBase
{
    public static Configuration config = ConfigurationManager.OpenExeConfiguration(AppDomain.CurrentDomain.BaseDirectory + "PokemonsDesktop");

    private static MenuVM _CurrentMenuVM;
    private static MainWindowVM instance;

    private static ViewModelBase _CurrentVM;

    private static bool _MenuIsVisible;

    //Геттеры и сеттеры полей ViewModel-и
    //Текущий пользовательский контент
    public MenuVM CurrentMenuVM
    {
        get { return _CurrentMenuVM; }
        set { this.RaiseAndSetIfChanged(ref _CurrentMenuVM, value); }
    }
    
    public bool MenuIsVisible
    {
        get { return _MenuIsVisible; }
        set { this.RaiseAndSetIfChanged(ref _MenuIsVisible, value); }
    }

    public ViewModelBase CurrentVM
    {
        get { return _CurrentVM; }
        set { this.RaiseAndSetIfChanged(ref _CurrentVM, value); }
    }

    public static MainWindowVM GetInstance()
    {
        if (instance == null)
        {
            instance = new MainWindowVM();
        }

        return instance;
    }

    private MainWindowVM()
    {
        if(config.AppSettings.Settings["RefreshToken"]?.Value != null)
        {
            try
            {
                Program.UpdateTokens();
                MenuIsVisible = true;
                CurrentVM = PokemonsListVM.GetInstance();
                CurrentMenuVM = null;
                CurrentMenuVM = MenuVM.GetInstance();
            }
            catch (Exception ex)
            {
                MenuIsVisible = false;
                AuthVM.GetInstance().GetRemember();
                CurrentVM = AuthVM.GetInstance();
                CurrentMenuVM = null;
                CurrentMenuVM = MenuVM.GetInstance();
            }  
        }
        else
        {
            MenuIsVisible = false;
            AuthVM.GetInstance().GetRemember();
            CurrentVM = AuthVM.GetInstance();
            CurrentMenuVM = null;
            CurrentMenuVM = MenuVM.GetInstance();
        }
        
        
    }

}