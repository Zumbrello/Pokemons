using System;
using System.IO;
using System.Net;
using System.Windows.Input;
using Avalonia.Media.Imaging;
using PokemonsDesktop.Models;
using ReactiveUI;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text.Json;
using Avalonia;
using Avalonia.Controls;
using PokemonsDesktop.Moderls;

namespace PokemonsDesktop.ViewModels;

public class PokemonsListVM : ViewModelBase
{
    private static PokemonsListVM instance;
    //Команды кнопок покедекса
    public static ICommand LastCommand { get; set; }
    public static ICommand NextCommand { get; set; }
    public static ICommand PreviousCommand { get; set; }
    public static ICommand FirstCommand { get; set; }
    
    private static ObservableCollection<PokemonTile> _PokemonsListTiles;
    private static int _PageCounter = 0;
    private static int _PaginationValue = 1; 
    
    private static double _CardHeight;
    private static double _CardWidth;
    private static double _ControlHeight;
    private static double _ControlWidth;
    private static double _CardImgSize;

    //Геттеры и сеттеры полей 
    public int PaginationValue
    {
        get { return _PaginationValue;}
        set
        {
            _PaginationValue = value;
            this.RaiseAndSetIfChanged(ref _PaginationValue, value);
        }
    }
    public int PageCounter
    {
        get { return _PageCounter; }
        set
        {
            _PageCounter = value;
            PaginationValue = value + 1;
            this.RaiseAndSetIfChanged(ref _PageCounter, value);
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

    public ICommand ShowPokemonCommand(object parameter)
    {
        ShowPokemon(parameter as PokemonTile);
        return null;
    }

    //Заполнение списка покемонов
    public void FillPokemonsListTile(bool Ignore = true)
    {
        PokemonsListTiles = new ObservableCollection<PokemonTile>();
        
        int gridSize = 15;
        JsonDocument request = JsonDocument.Parse(Program.wc.DownloadString(
            Program.HostAdress + "/GetCountPokemons?offset=" + gridSize * PageCounter + "&count=" +
            gridSize));
        List<Pokemon> FullRequest = JsonSerializer.Deserialize<List<Pokemon>>(request.RootElement);

        for (int i = 0; i < FullRequest.Count && i < gridSize; i++)
        {
            try
            {
                PokemonTile tile = new PokemonTile
                {
                    Number = FullRequest[i].Number,
                    Title = FullRequest[i].Title,
                    Url = FullRequest[i].Url,
                    Image = new Avalonia.Media.Imaging.Bitmap(
                        new MemoryStream(Program.wc.DownloadData(FullRequest[i].Image)))
                };
                PokemonsListTiles.Add(tile);
            }
            catch (Exception ex)
            {
                if (ex is WebException && ((ex as WebException).Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
                {
                    PokemonTile tile_in = new PokemonTile
                    {
                        Number = FullRequest[i].Number,
                        Title = FullRequest[i].Title,
                        Url = FullRequest[i].Url,
                        Image = new Avalonia.Media.Imaging.Bitmap(
                            new MemoryStream(Program.wc.DownloadData("https://нашчат.рф/avatar/user502750_11512092.jpg")))
                    };
                    PokemonsListTiles.Add(tile_in);
                }
                
                PokemonTile tile = new PokemonTile
                {
                    Number = FullRequest[i].Number,
                    Title = FullRequest[i].Title,
                    Url = FullRequest[i].Url,
                    Image = new Avalonia.Media.Imaging.Bitmap(
                        new MemoryStream(Program.wc.DownloadData("https://pokestop.cz/wp-content/uploads/2018/06/co-je-to-za-pokemona-01.jpg")))
                };
                PokemonsListTiles.Add(tile);
                
                if (!Ignore)
                {
                    FillPokemonsListTile();   
                }
                 
            }
        }
    }
    public static PokemonsListVM GetInstance()
    {
        if (instance == null)
        {
            instance = new PokemonsListVM();
        }

        return instance;
    }
    
    private PokemonsListVM()
    { 
        FillPokemonsListTile();
    }

    public ICommand GoToFirstPage()
    {
        PageCounter = 0;
        FillPokemonsListTile();
        MainWindowVM.GetInstance().CurrentVM = null;
        MainWindowVM.GetInstance().CurrentVM = GetInstance();
        return null;
    }
    public ICommand GoToLastPage()
    {
        PageCounter = Convert.ToInt32(
                        JsonDocument.Parse(
                            Program.wc.DownloadString(
                                Program.HostAdress + "/GetAllPokemonsCount")
                            ).RootElement.ToString()) - 1;

        PageCounter = (int)Math.Truncate((decimal)PageCounter / 15);
        FillPokemonsListTile(false);
        MainWindowVM.GetInstance().CurrentVM = null;
        MainWindowVM.GetInstance().CurrentVM = GetInstance();
        return null;
    }
    public ICommand GoToNextPage()
    {
        int AllPagesCount = 0;
   
        AllPagesCount = Convert.ToInt32(JsonDocument
            .Parse(Program.wc.DownloadString(Program.HostAdress + "/GetAllPokemonsCount")).RootElement.ToString()) / 15;
        

        if (PageCounter == AllPagesCount)
        {
            return null;
        } 
        PageCounter++; 
        FillPokemonsListTile();
        MainWindowVM.GetInstance().CurrentVM = null;
        MainWindowVM.GetInstance().CurrentVM = GetInstance();
        return null;
    }
    public ICommand GoToPreviousPage()
    {
        if (PageCounter == 0)
        {
            return null;
        }
        PageCounter--;
        FillPokemonsListTile();
        MainWindowVM.GetInstance().CurrentVM = null;
        MainWindowVM.GetInstance().CurrentVM = GetInstance(); 
        return null;
    }
    private void ShowPokemon(PokemonTile Tile)
    {
        MainWindowVM.GetInstance().CurrentVM = PokemonVM.GetInstance(Tile);
    }
}