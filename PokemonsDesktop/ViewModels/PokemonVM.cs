using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Text.Json;
using System.Windows.Input;
using Avalonia.Media;
using PokemonsDesktop.Models;
using PokemonsDesktop.Moderls;
using ReactiveUI;

namespace PokemonsDesktop.ViewModels;

public class PokemonVM : ViewModelBase
{
    private static PokemonVM instance;
    private PokemonTile _CurrentPokemonTile;
    private List<List<Pokemon>> EvolutionsLists = new List<List<Pokemon>>();
    private string _Title;
    private JsonDocument PokemonParameters;
    private JsonDocument PokemonCharactiristics;
    private string[] EvolutionsListPrev = new string[]{};
    private string[] EvolutionsListNext = new string[]{};
    private ObservableCollection<PokemonTile> _PokemonsListTiles1;
    private ObservableCollection<PokemonTile> _PokemonsListTiles2;
    private IBrush _LikedButtonColor;
    private string _LikedButtonText;

    public string Title
    {
        get { return "Название: " + CurrentPokemonTile.Title; }
    }

    public string LikedButtonText
    {
        get { return _LikedButtonText; }
        set
        {
            _LikedButtonText = value;
            this.RaiseAndSetIfChanged(ref _LikedButtonText, value);
        }

    }
    public IBrush LikedButtonColor
    {
        get { return _LikedButtonColor; }
        set
        {
            _LikedButtonColor = value;
            this.RaiseAndSetIfChanged(ref _LikedButtonColor, value);
        }

    }
    public ObservableCollection<PokemonTile> PokemonsListTiles1
    {
        get { return _PokemonsListTiles1; }
        set
        {
            _PokemonsListTiles1 = value;
            this.RaiseAndSetIfChanged(ref _PokemonsListTiles1, value);
            
        }
    }

    public ICommand LikedButtonOnClick(object parameter)
    {
        try
        {
            string Parameters = "login=" + MainWindowVM.config.AppSettings.Settings["Login"].Value +
                                "&pokemon=" + (parameter as PokemonVM).CurrentPokemonTile.Url;
            Program.wc.DownloadString(Program.HostAdress + "/GetChangeLikedPokemon?" + Parameters);

            MainWindowVM.GetInstance().CurrentVM = null;
            MainWindowVM.GetInstance().CurrentVM = GetInstance(CurrentPokemonTile);
            return null; 
        }
        catch (Exception e)
        {
            return null; 
        }
    }
    
    public ObservableCollection<PokemonTile> PokemonsListTiles2
    {
        get { return _PokemonsListTiles2; }
        set
        {
            _PokemonsListTiles2 = value;
            this.RaiseAndSetIfChanged(ref _PokemonsListTiles2, value);
        }
    }
    
    public PokemonTile CurrentPokemonTile
    {
        get { return _CurrentPokemonTile; }
        set
        {
            _CurrentPokemonTile = value;
            this.RaiseAndSetIfChanged(ref _CurrentPokemonTile, value);
        }
    }
    public string Number
    {
        get { return "Номер: " + CurrentPokemonTile.Number.ToString(); }
    }
    
    public string Health
    {
        get
        {
            return "Здоровье: " +  PokemonCharactiristics.RootElement.GetProperty("Health").ToString();
        }
    }
    
    public string Attack
    {
        get
        {
            return "Атака: " + PokemonCharactiristics.RootElement.GetProperty("Attack").ToString();
        }
    }
    
    public string SpecialAttack
    {
        get
        {
            return "Спец.атака: " + PokemonCharactiristics.RootElement.GetProperty("SpecialAttack").ToString();
        }
    }
    
    public string Protection
    {
        get
        {
            return "Защита: " + PokemonCharactiristics.RootElement.GetProperty("Protection").ToString();
        }
    }
    
    public string SpecialProtection
    {
        get
        {
            return "Спец.защита: " + PokemonCharactiristics.RootElement.GetProperty("SpecialProtection").ToString();
        }
    }

    private PokemonVM(PokemonTile? Tile)
    {
        if (Tile != null)
        {
            FillPokemonInfo(Tile);
        }
    }

    private void FillPokemonInfo(PokemonTile Tile)
    {
        PokemonsListTiles1 = new ObservableCollection<PokemonTile>();
        PokemonsListTiles2 = new ObservableCollection<PokemonTile>();
        
        CurrentPokemonTile = Tile;
        string Parameters = "&pokemon=" + CurrentPokemonTile.Url;
        PokemonParameters = JsonDocument.Parse(
            Program.wc.DownloadString(
                Program.HostAdress + "/GetPokemonParameters?" + Parameters));
        
        PokemonCharactiristics = JsonDocument.Parse(    
            Program.wc.DownloadString(
                Program.HostAdress + "/GetPokemonCharacteristics?" + Parameters));

        EvolutionsListPrev = PokemonCharactiristics.RootElement.GetProperty("EvolutionsListPrev").ToString().Split(",");
        EvolutionsListNext = PokemonCharactiristics.RootElement.GetProperty("EvolutionsListNext").ToString().Split(",");

        string PrevEvolutionsParameter = "";
        string NextEvolutionsParameter = "";
        
        for (int i = 0; i < EvolutionsListPrev.Length; i++)
        {
            PrevEvolutionsParameter += (i == EvolutionsListPrev.Length - 1 ? EvolutionsListPrev[i] : EvolutionsListPrev[i] + ",");
        }
        
        
        for (int i = 0; i < EvolutionsListPrev.Length; i++)
        {
            NextEvolutionsParameter += (i == EvolutionsListNext.Length - 1 ? EvolutionsListNext[i] : EvolutionsListNext[i] + ",");
        }
        
        Parameters = "pokemons=" + PrevEvolutionsParameter;
        if (EvolutionsListPrev[0] != "")
        {
            List<HttpPokemonTile> PrevEvolutionsTiles = JsonSerializer.Deserialize<List<HttpPokemonTile>>(JsonDocument
                .Parse(
                    Program.wc.DownloadString(Program.HostAdress + "/GetPokemonsByUrl?" + Parameters)).RootElement
                );

            for (int i = 0; i < PrevEvolutionsTiles.Count; i++)
            {
                PokemonsListTiles1.Add(PrevEvolutionsTiles[i].ConvertToPokemonTile());
            }
        }

        Parameters = "pokemons=" + NextEvolutionsParameter;

        
        if (EvolutionsListNext[0] != "")
        {
            List<HttpPokemonTile> NextEvolutionsTiles = JsonSerializer.Deserialize<List<HttpPokemonTile>>(JsonDocument
                .Parse(
                    Program.wc.DownloadString(Program.HostAdress + "/GetPokemonsByUrl?" + Parameters)).RootElement
                );

            for (int i = 0; i < NextEvolutionsTiles.Count; i++)
            {
                PokemonsListTiles2.Add(NextEvolutionsTiles[i].ConvertToPokemonTile());
            }
        }
        
        Parameters = "login=" + MainWindowVM.config.AppSettings.Settings["Login"].Value;
        String LikedPokemonsStr = "";
        try
        {
            LikedPokemonsStr = Program.wc.DownloadString(Program.HostAdress + "/GetLikedPokemons?" + Parameters);
        }
        catch (Exception ex)
        {}

        if (LikedPokemonsStr.Contains(Tile.Url))
        {
            LikedButtonColor = Brushes.Red;
            LikedButtonText = "Удалить из избранного";
        }
        else
        {
            LikedButtonColor = Brushes.LightGreen;
            LikedButtonText = "Добавить в избранное";
        }
    }
    
    public static PokemonVM GetInstance(PokemonTile? Tile)
    {
        if (instance == null)
            instance = new PokemonVM(Tile);
        if (Tile != null)
        {
            instance = new PokemonVM(Tile);
            instance.FillPokemonInfo(Tile);   
        }
        return instance;
    }
    
    public ICommand ShowPokemonCommand(object parameter)
    {
        ShowPokemon(parameter as PokemonTile);
        return null;
    }
    
    private void ShowPokemon(PokemonTile Tile)
    {
        MainWindowVM.GetInstance().CurrentVM = null;
        MainWindowVM.GetInstance().CurrentVM = PokemonVM.GetInstance(Tile);
    }
}