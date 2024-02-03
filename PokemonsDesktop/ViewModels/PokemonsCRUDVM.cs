using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using PokemonsDesktop.Models;
using PokemonsDesktop.Moderls;
using ReactiveUI;
namespace PokemonsDesktop.ViewModels;

public class PokemonsCRUDVM : ViewModelBase
{
    private static PokemonsCRUDVM Instance;
    private static string _TitleTB;
    private static string _PokemonURLTB;
    private static string _NumberTB;
    private static string _URLTB;
    private static string _HealthTB;
    private static string _ActionText;
    private static string _PageTitle;
    private static string _StatusText;
    private static string _AttackTB;
    private static string _ProtectionTB;
    private static string _SpecialAttackTB;
    private static string _SpecialProtectionTB;
    private static Pokemon CurrentPokemon;
    private static bool _FindButtonVisibility;
    private static int _Mode;
    private static Bitmap _PokemonImage;

    //Геттеры и сеттеры полей
    public Bitmap PokemonImage
    {
        get
        {
            return _PokemonImage;
        }
        set
        {
            this.RaiseAndSetIfChanged(ref _PokemonImage, value);
        }
    }
    
    public int Mode
    {
        get { return _Mode; }
        set
        {
            this.RaiseAndSetIfChanged(ref _Mode, value);
        }
    }
    public string TitleTB
    {
        get { return _TitleTB; }
        set
        {
            this.RaiseAndSetIfChanged(ref _TitleTB, value);
        }
    }
    public bool FindButtonVisibility
    {
        get { return _FindButtonVisibility; }
        set
        {
            this.RaiseAndSetIfChanged(ref _FindButtonVisibility, value);
        }
    }
    public string PokemonURLTB
    {
        get { return _PokemonURLTB; }
        set
        {
            this.RaiseAndSetIfChanged(ref _PokemonURLTB, value);
        }
    }
    public string NumberTB
    {
        get { return _NumberTB; }
        set
        {
            this.RaiseAndSetIfChanged(ref _NumberTB, value);
        }
    }
    public string URLTB
    {
        get { return _URLTB; }
        set
        {
            this.RaiseAndSetIfChanged(ref _URLTB, value);
            try
            {
                PokemonImage = new Bitmap(new MemoryStream(Program.wc.DownloadData(value)));
            }
            catch (Exception ex)
            {
                PokemonImage = new Bitmap(new MemoryStream(Program.wc.DownloadData(
                    "https://pokestop.cz/wp-content/uploads/2018/06/co-je-to-za-pokemona-01.jpg")));
            }
        }
    }
    public string HealthTB
    {
        get { return _HealthTB; }
        set
        {
            this.RaiseAndSetIfChanged(ref _HealthTB, value);
        }
    }
    public string ActionText
    {
        get { return _ActionText; }
        set
        {
            this.RaiseAndSetIfChanged(ref _ActionText, value);
        }
    }
    public string StatusText
    {
        get { return _StatusText; }
        set
        {
            this.RaiseAndSetIfChanged(ref _StatusText, value);
        }
    }
    public string AttackTB
    {
        get { return _AttackTB; }
        set
        {
            this.RaiseAndSetIfChanged(ref _AttackTB, value);
        }
    }
    public string SpecialAttackTB
    {
        get { return _SpecialAttackTB; }
        set
        {
            this.RaiseAndSetIfChanged(ref _SpecialAttackTB, value);
        }
    }
    public string ProtectionTB
    {
        get { return _ProtectionTB; }
        set
        {
            this.RaiseAndSetIfChanged(ref _ProtectionTB, value);
        }
    }
    public string SpecialProtectionTB
    {
        get { return _SpecialProtectionTB; }
        set
        {
            this.RaiseAndSetIfChanged(ref _SpecialProtectionTB, value);
        }
    }
    public string PageTitle
    {
        get { return _PageTitle; }
        set
        {
            this.RaiseAndSetIfChanged(ref _PageTitle, value);
        }
    }
    //Кнопка действия
    public ICommand ActionBtnClickCommand { get; }
    //Кнопка заполнения данных о покемоне
    public ICommand FindButtonOnClickCommand { get; }
    //Заполнение формы в зависимости от режима
    public void SetFormMode(int FormMode)
    {
        Mode = FormMode;
        if (FormMode == (int)PokemonMgtMode.CreatePokemon)
        {
            PageTitle = "Создание покемона";
            ActionText = "Создать";
            FindButtonVisibility = false;
        
        }else if (FormMode == (int)PokemonMgtMode.ChangePokemon)
        {
            PageTitle = "Изменение покемона";
            ActionText = "Изменить";
            FindButtonVisibility = true;
        }
    }
    private PokemonsCRUDVM()
    {
        ActionBtnClickCommand = ReactiveCommand.Create(ActionButtonOnClick);
        FindButtonOnClickCommand = ReactiveCommand.Create(FindButtonOnClick);
    }
    //Нажатие на поиск покемона и заполнение данных
    private void FindButtonOnClick()
    {
        string Parameters = "pokemons=" + PokemonURLTB;
        JsonDocument Response =
            JsonDocument.Parse(Program.wc.DownloadString(Program.HostAdress + "/GetPokemonsByUrl?" + Parameters));
            
        List<Pokemon> PokemonsLst = JsonSerializer.Deserialize<List<Pokemon>>(Response.RootElement);
        StatusText = "";
        TitleTB = "";
        NumberTB = "";
        URLTB = "";
        HealthTB = "";
        AttackTB = "";
        SpecialAttackTB = "";
        ProtectionTB = "";
        SpecialProtectionTB = "";
        if(PokemonsLst.Count == 0)
        {
            StatusText = "Такой покемон не обнаружен!";
            HideStatus();
            return;
        }
        else
        {
            StatusText = "Покемон успешно найден!";
            HideStatus();
        }
        
        CurrentPokemon = PokemonsLst[0];
        TitleTB = CurrentPokemon.Title;
        NumberTB = CurrentPokemon.Number.ToString();
        URLTB = CurrentPokemon.Image;
        HealthTB = CurrentPokemon.Health.ToString();
        AttackTB = CurrentPokemon.Attack.ToString();
        SpecialAttackTB = CurrentPokemon.SpecialAttack.ToString();
        ProtectionTB = CurrentPokemon.Protection.ToString();
        SpecialProtectionTB = CurrentPokemon.SpecialProtection.ToString();
    }
    //Нажатие кнопки действия
    private void ActionButtonOnClick()
    {
        if (Mode == (int)PokemonMgtMode.CreatePokemon)
        {
            if (!CheckFields())
            {
                return;
            }

            CurrentPokemon = new Pokemon();
            CurrentPokemon.Url = PokemonURLTB;
            CurrentPokemon.Title = TitleTB;
            CurrentPokemon.Number = Convert.ToInt32(NumberTB);
            CurrentPokemon.Image = URLTB;
            CurrentPokemon.Health = Convert.ToInt32(HealthTB);
            CurrentPokemon.Attack = Convert.ToInt32(AttackTB);
            CurrentPokemon.SpecialAttack = Convert.ToInt32(SpecialAttackTB);
            CurrentPokemon.Protection = Convert.ToInt32(ProtectionTB);
            CurrentPokemon.SpecialProtection = Convert.ToInt32(SpecialProtectionTB);
            
            string Parameters = "json=" + JsonSerializer.Serialize(CurrentPokemon) +
                                "&mode=1";

           try
           {
               Program.wc.UploadString(Program.HostAdress + "/PostChangePokemon?" + Parameters, "");
                StatusText = "Покемон успешно создан!";
            }catch(Exception ex)
            {
                StatusText = "Такой покемон уже существует!";
            }

            HideStatus();

        }else if (Mode == (int)PokemonMgtMode.ChangePokemon)
        {
            if (CurrentPokemon == null)
            {
                StatusText = "Вы не нашли покемона!";
                HideStatus();
                return;
            }
            
            if (!CheckFields())
            {
                return;
            }
            
            CurrentPokemon.Title = TitleTB;
            CurrentPokemon.Number = Convert.ToInt32(NumberTB);
            CurrentPokemon.Image = URLTB;
            CurrentPokemon.Health = Convert.ToInt32(HealthTB);
            CurrentPokemon.Attack = Convert.ToInt32(AttackTB);
            CurrentPokemon.SpecialAttack = Convert.ToInt32(SpecialAttackTB);
            CurrentPokemon.Protection = Convert.ToInt32(ProtectionTB);
            CurrentPokemon.SpecialProtection = Convert.ToInt32(SpecialProtectionTB);
            string Parameters = "json=" + JsonSerializer.Serialize(CurrentPokemon) +
                                "&mode=2";
            try
            {
                Program.wc.UploadString(Program.HostAdress + "/PostChangePokemon?" + Parameters, "");
                StatusText = "Покемон успешно изменён!";
            }catch(Exception ex)
            {
                StatusText = "Ошибка изменения пользователя";
            }
        }   
    }
    
    //Проверка заполнения полей
    private bool CheckFields()
    {
        try
        {
            int OutInt;
            if (TitleTB.Length == 0 ||
                NumberTB.Length == 0 ||
                URLTB.Length == 0 ||
                HealthTB.Length == 0 ||
                AttackTB.Length == 0 ||
                SpecialAttackTB.Length == 0 ||
                ProtectionTB.Length == 0 ||
                SpecialProtectionTB.Length == 0)
            {
                StatusText = "Заполните все поля!";
                HideStatus();
                return false;
            }

            if (!int.TryParse(NumberTB, out OutInt))
            {
                StatusText = "Номер заполнен некорректно!";
                HideStatus();
                return false;
            }
            
            if (!int.TryParse(HealthTB, out OutInt))
            {
                StatusText = "Здоровье заполнено некорректно!";
                HideStatus();
                return false;
            }
            
            if (!int.TryParse(AttackTB, out OutInt))
            {
                StatusText = "Атака заполнена некорректно!";
                HideStatus();
                return false;
            }
            
            if (!int.TryParse(SpecialAttackTB, out OutInt))
            {
                StatusText = "Спец.Атака заполнена некорректно!";
                HideStatus();
                return false;
            }
            
            if (!int.TryParse(ProtectionTB, out OutInt))
            {
                StatusText = "Защита заполнена некорректно!";
                HideStatus();
                return false;
            }
            
            if (!int.TryParse(SpecialProtectionTB, out OutInt))
            {
                StatusText = "Спец.Защита заполнена некорректно!";
                HideStatus();
                return false;
            }

            if (!((URLTB.Contains("http://") || URLTB.Contains("https://")) && (URLTB.Contains(".jpg") || URLTB.Contains(".jpeg") || URLTB.Contains(".png")) && URLTB.Length > 14))
            {
                StatusText = "Ссылка на изображение должна быть в формате: https(http)://sitename.domen/imagename.jpg";
                HideStatus();
                return false;
            }

            if (PokemonURLTB.Length < 3)
            {
                StatusText = "Минимальная длинна id покемона 3 символа!";
                HideStatus();
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            StatusText = "Поля заполнены не корректно!";
            HideStatus();
            return false;
        }
    }
    private async void HideStatus()
    {
        await Task.Delay(5000);
        StatusText = "";
    }

    public static PokemonsCRUDVM GetInstance()
    {
        if (Instance == null)
            Instance = new PokemonsCRUDVM();
        Instance.StatusText = "";
        Instance.TitleTB = "";
        Instance.NumberTB = "";
        Instance.URLTB = "";
        Instance.HealthTB = "";
        Instance.AttackTB = "";
        Instance.SpecialAttackTB = "";
        Instance.ProtectionTB = "";
        Instance.SpecialProtectionTB = "";
        Instance.PokemonURLTB = "";
        return Instance;
    }
    enum PokemonMgtMode
    {
        CreatePokemon = 1,
        ChangePokemon
    }
}