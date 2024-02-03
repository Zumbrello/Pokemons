using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Windows.Input;
using PokemonsDesktop.Models;
using PokemonsDesktop.Moderls;
using ReactiveUI;

namespace PokemonsDesktop.ViewModels;

public class EventsVM : ViewModelBase
{
    private static EventsVM Instance;
    private static ICommand ItemBtnClick { get; set; }
    private static ObservableCollection<DailyPokemon> _PokemonsListTiles;
    public ObservableCollection<DailyPokemon> PokemonsListTiles
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
        MainWindowVM.GetInstance().CurrentVM = PokemonVM.GetInstance(new PokemonTile()
        {
            Url = (parameter as DailyPokemon).Url,
            Image = (parameter as DailyPokemon).Image,
            Number = (parameter as DailyPokemon).Number,
            Title = (parameter as DailyPokemon).Title
        });
        return null;
    }
    
    //Заполнение списка карточек временных покемонов
    public void FillPokemonsListTile(bool Ignore = true)
    {
        PokemonsListTiles = new ObservableCollection<DailyPokemon>();
        string Daily = Program.wc.DownloadString(Program.HostAdress + "/GetTimePokemon?Mode=1");
        string Weekly = Program.wc.DownloadString(Program.HostAdress + "/GetTimePokemon?Mode=2");
        string Mounthly = Program.wc.DownloadString(Program.HostAdress + "/GetTimePokemon?Mode=3");

        
        string Parameters = "pokemons=" + Daily + "," + Weekly + "," + Mounthly;
        List<HttpPokemonTile> TimePokemons = JsonSerializer.Deserialize<List<HttpPokemonTile>>(JsonDocument
                .Parse(
                    Program.wc.DownloadString(Program.HostAdress + "/GetPokemonsByUrl?" + Parameters)).RootElement
                );
        
        for (int i = 0; i < 3; i++)
        {
            try
            {
                //Установка значений в карточке временного покемона
                DailyPokemon tile = new DailyPokemon
                {
                    Number = TimePokemons[i].Number,
                    Title = TimePokemons[i].Title,
                    Url = TimePokemons[i].Url,
                    Image = new Avalonia.Media.Imaging.Bitmap(
                        new MemoryStream(Program.wc.DownloadData(TimePokemons[i].Image)))
                };
                if (i == 0)
                {
                    tile.Period = "Покемон Дня";
                }
                if (i == 1)
                {
                    tile.Period = "Покемон Недели";
                }
                if (i == 2)
                {
                    tile.Period = "Покемон Месяца";
                }
                PokemonsListTiles.Add(tile);
            }
            catch (Exception ex)
            {
                if (ex is WebException && ((ex as WebException).Response as HttpWebResponse).StatusCode == HttpStatusCode.NotFound)
                {
                    //Обработка ошибки при отсутствии или некорректной ссылке на изображение покемона
                    DailyPokemon tile = new DailyPokemon
                    {
                        Number = TimePokemons[i].Number,
                        Title = TimePokemons[i].Title,
                        Url = TimePokemons[i].Url,
                        Image = new Avalonia.Media.Imaging.Bitmap(
                            new MemoryStream(Program.wc.DownloadData("https://нашчат.рф/avatar/user502750_11512092.jpg")))
                    };
                    if (i == 0)
                    {
                        tile.Period = "Покемон Дня";
                    }
                    if (i == 1)
                    {
                        tile.Period = "Покемон Недели";
                    }
                    if (i == 2)
                    {
                        tile.Period = "Покемон Месяца";
                    }
                    PokemonsListTiles.Add(tile);
                }
                if (!Ignore)
                {
                    FillPokemonsListTile();   
                }
                 
            }
        }
    }
    private EventsVM()
    { }

    public static EventsVM GetInstance()
    {
        if (Instance == null)
            Instance = new EventsVM();
        Instance.FillPokemonsListTile();
        return Instance;
    }
}