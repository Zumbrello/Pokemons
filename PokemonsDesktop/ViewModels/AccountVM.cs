using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Text.Json;
using System.Windows.Input;
using PokemonsDesktop.Models;
using PokemonsDesktop.Models;
using PokemonsDesktop.Moderls;
using ReactiveUI;

namespace PokemonsDesktop.ViewModels;

public class AccountVM : ViewModelBase
{
    private static AccountVM Instance;
    private static string _LoginTB;
    private static bool _LoginEnabled;
    private static bool _LoginVisible;
    private static string _EmailTB;
    private static bool _EmailEnabled;
    private static bool _EmailVisible;
    private static string _LastEnterTB;
    private static bool _LastEnterEnabled;
    private static bool _LastEnterVisible;
    private static ObservableCollection<PokemonTile> _PokemonsListTiles;

    //Свойство логина
    public string LoginTB
    {
        get { return _LoginTB; }
        set
        {
            _LoginTB = value;
            this.RaiseAndSetIfChanged(ref _LoginTB, value);
        }
    }
    
    public bool LoginEnabled
    {
        get { return _LoginEnabled; }
        set
        {
            _LoginEnabled = value;
            this.RaiseAndSetIfChanged(ref _LoginEnabled, value);
        }
    }
    
    public bool LoginVisible
    {
        get { return _LoginVisible; }
        set
        {
            _LoginVisible = value;
            this.RaiseAndSetIfChanged(ref _LoginVisible, value);
        }
    }
    
    public string EmailTB
    {
        get { return _EmailTB; }
        set
        {
            _EmailTB = value;
            this.RaiseAndSetIfChanged(ref _EmailTB, value);
        }
    }
    public bool EmailEnabled
    {
        get { return _EmailEnabled; }
        set
        {
            _EmailEnabled = value;
            this.RaiseAndSetIfChanged(ref _EmailEnabled, value);
        }
    }
    
    public bool EmailVisible
    {
        get { return _EmailVisible; }
        set
        {
            _EmailVisible = value;
            this.RaiseAndSetIfChanged(ref _EmailVisible, value);
        }
    }
    
    public string LastEnterTB
    {
        get { return _LastEnterTB; }
        set
        {
            _LastEnterTB = value;
            this.RaiseAndSetIfChanged(ref _LastEnterTB, value);
        }
    }
    public bool LastEnterEnabled
    {
        get { return _LastEnterEnabled; }
        set
        {
            _LastEnterEnabled = value;
            this.RaiseAndSetIfChanged(ref _LastEnterEnabled, value);
        }
    }
    
    public bool LastEnterVisible
    {
        get { return _LastEnterVisible; }
        set
        {
            _LastEnterVisible = value;
            this.RaiseAndSetIfChanged(ref _LastEnterVisible, value);
        }
    }

    public ObservableCollection<PokemonTile> PokemonsListTiles
    {
        get { return _PokemonsListTiles; }
        set
        {
            _PokemonsListTiles = value;
            this.RaiseAndSetIfChanged(ref _PokemonsListTiles, value);
        }
    }

    private AccountVM()
    {
        FillLikedList();   
    }

    public static AccountVM GetInstance()
    {
        Instance = new AccountVM();
        string Parameters = "login=" + MainWindowVM.config.AppSettings.Settings["Login"].Value;
        
        User CurrentUser = JsonSerializer.Deserialize<List<User>>(JsonDocument
            .Parse(
                Program.wc.DownloadString(Program.HostAdress + "/GetUser?" + Parameters)).RootElement
            )[0];
        Instance.EmailTB = CurrentUser.Email;
        Instance.LoginTB = CurrentUser.Login;
        Instance.LastEnterTB = CurrentUser.Lastlogin;

        return Instance;
    }
    
    //Команды кнопок
    public ICommand ShowPokemonCommand(object parameter)
    {
        ShowPokemon(parameter as PokemonTile);
        return null;
    }
    
    private void ShowPokemon(PokemonTile Tile)
    {
        MainWindowVM.GetInstance().CurrentVM = PokemonVM.GetInstance(Tile);
    }
    
    public ICommand DeleteFromLiked(object parameter)
    {
        string Parameters = "login=" + MainWindowVM.config.AppSettings.Settings["Login"].Value +
                            "&pokemon=" + (parameter as PokemonTile).Url;

        Program.wc.DownloadString(Program.HostAdress + "/GetChangeLikedPokemon?" + Parameters);
     
        FillLikedList();
        MainWindowVM.GetInstance().CurrentVM = null;
        MainWindowVM.GetInstance().CurrentVM = Instance;
        return null;
    }

    //Заполнение списка избранных покемонов
    private void FillLikedList()
    {
        PokemonsListTiles = new ObservableCollection<PokemonTile>();

        string Parameters = "login=" + MainWindowVM.config.AppSettings.Settings["Login"].Value;

        try
        {
            string LikedPokemonsStr = Program.wc.DownloadString(Program.HostAdress + "/GetLikedPokemons?" + Parameters);

            if (LikedPokemonsStr != "")
            {
                Parameters = "pokemons=" + LikedPokemonsStr;
                List<HttpPokemonTile> LikedPokemons = JsonSerializer.Deserialize<List<HttpPokemonTile>>(JsonDocument
                    .Parse(
                        Program.wc.DownloadString(Program.HostAdress + "/GetPokemonsByUrl?" + Parameters)).RootElement
                );

                for (int i = 0; i < LikedPokemons.Count; i++)
                {
                    PokemonsListTiles.Add(LikedPokemons[i].ConvertToPokemonTile());
                }
            }
        }
        catch (Exception ex)
        {
            
        }
    }
}