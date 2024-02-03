using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using PokemonsDesktop.Models;
using PokemonsDesktop.Moderls;
using ReactiveUI;
namespace PokemonsDesktop.ViewModels;

public class UsersCRUDVM : ViewModelBase
{
    private static UsersCRUDVM Instance;
    private static string _LoginTB;
    private static string _EmailTB;
    private static string _LastEnterTB;
    private static string _PageTitle;
    private static string _ActionText;
    private static string _PasswordTB;
    private static string _StatusText;
    private static User CurrentUser;
    private static bool _FindButtonVisibility;
    private static bool _AdminChanged;
    private static int _Mode;
    private static ObservableCollection<PokemonTile> _PokemonsListTiles;
    private static bool _ShowLikedPokemons;

    //Геттеры и сеттеры полей
    public bool ShowLikedPokemons
    {
        get { return _ShowLikedPokemons; }
        set
        {
            this.RaiseAndSetIfChanged(ref _ShowLikedPokemons, value);
        }
    }
    public ObservableCollection<PokemonTile> PokemonsListTiles
    {
        get { return _PokemonsListTiles; }
        set
        {
            this.RaiseAndSetIfChanged(ref _PokemonsListTiles, value);
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
    public string StatusText
    {
        get { return _StatusText; }
        set
        {
            this.RaiseAndSetIfChanged(ref _StatusText, value);
        }
    }
    public string PasswordTB
    {
        get { return _PasswordTB; }
        set
        {
            this.RaiseAndSetIfChanged(ref _PasswordTB, value);
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
    public string PageTitle
    {
        get { return _PageTitle; }
        set
        {
            this.RaiseAndSetIfChanged(ref _PageTitle, value);
        }
    }
    public bool AdminChanged
    {
        get { return _AdminChanged; }
        set
        {
            this.RaiseAndSetIfChanged(ref _AdminChanged, value);
        }
    }
    public string LoginTB
    {
        get { return _LoginTB; }
        set
        {
            this.RaiseAndSetIfChanged(ref _LoginTB, value);
        }
    }
    public string EmailTB
    {
        get { return _EmailTB; }
        set
        {
            this.RaiseAndSetIfChanged(ref _EmailTB, value);
        }
    }
    public string LastEnterTB
    {
        get { return _LastEnterTB; }
        set
        {
            this.RaiseAndSetIfChanged(ref _LastEnterTB, value);
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
    
    //Кнопка действия
    public ICommand ActionBtnClickCommand { get; }
   
    //Кнопка заполнения данных о пользователе
    public ICommand FindButtonOnClickCommand { get; }
    
    //Заполнение формы в зависимости от режима
    public void SetFormMode(int FormMode)
    {
        Mode = FormMode;
        if (FormMode == (int)UserMgtMode.CreateUser)
        {
            PageTitle = "Создание пользователя";
            ActionText = "Создать";
            FindButtonVisibility = false;
            ShowLikedPokemons = false;
        
        }else if (FormMode == (int)UserMgtMode.ChangeUser)
        {
            PageTitle = "Изменение пользователя";
            ActionText = "Изменить";
            FindButtonVisibility = true;
            ShowLikedPokemons = true;
        }
    }
    private UsersCRUDVM()
    {
    }
    
    //Нажатие кнопки поиска пользователя
    public ICommand FindButtonOnClick()
    {
        string Parameters = "login=" + LoginTB;
        JsonDocument Response =
            JsonDocument.Parse(Program.wc.DownloadString(Program.HostAdress + "/GetUser?" + Parameters));
            
        List<User> UsersLst = JsonSerializer.Deserialize<List<User>>(Response.RootElement);
        StatusText = "";
        EmailTB = "";
        LastEnterTB = "";
        AdminChanged = false;
        PasswordTB = "";
        if(UsersLst.Count == 0)
        {
            StatusText = "Пользователь с таким логином не обнаружен!";
            HideStatus();
            return null;
        }
        else
        {
            StatusText = "Пользователь успешно найден!";
            FillLikedList(LoginTB);
            HideStatus();
        }
        
        CurrentUser = UsersLst[0];
        LoginTB = CurrentUser.Login;
        EmailTB = CurrentUser.Email;
        LastEnterTB = CurrentUser.Lastlogin;
        AdminChanged = CurrentUser.Isadmin;
        PasswordTB = System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(CurrentUser.Password));
        return null;
    }
    
    //Нажатие кнопки действия
    public ICommand ActionButtonOnClick()
    {
        if (Mode == (int)UserMgtMode.CreateUser)
        {
            if (!CheckFields())
            {
                return null;
            }

            CurrentUser = new User();
            CurrentUser.Login = LoginTB;
            CurrentUser.Email = EmailTB;
            CurrentUser.Isadmin = AdminChanged;
            CurrentUser.Password = PasswordTB;
            
            string Parameters = "login=" + LoginTB +
                                "&isAdmin=" + AdminChanged +
                                "&password=" + PasswordTB +
                                "&email=" + EmailTB;
            
            try
            {
                Program.wc.UploadString(Program.HostAdress + "/CreateUser?" + Parameters, "");
                StatusText = "Пользователь успешно создан!";
            }catch(Exception ex)
            {
                StatusText = "Такой пользователь уже существует!";
            }

            HideStatus();

        }else if (Mode == (int)UserMgtMode.ChangeUser)
        {
            if (CurrentUser == null)
            {
                StatusText = "Вы не нашли пользователя!";
                HideStatus();
                return null;
            }
            CheckFields();
            CurrentUser.Login = LoginTB;
            CurrentUser.Email = EmailTB;
            CurrentUser.Isadmin = AdminChanged;
            CurrentUser.Password = PasswordTB;
            string Parameters = "login=" + LoginTB +
                                "&isAdmin=" + AdminChanged.ToString() +
                                "&password=" + PasswordTB +
                                "&mode=1" +
                                "&email=" + EmailTB;
            
            try
            {
                Program.wc.UploadString(Program.HostAdress + "/ChangeUser?" + Parameters, "");
                StatusText = "Пользователь успешно изменён!";
            }catch(Exception ex)
            {
                StatusText = "Ошибка изменения пользователя";
            }
        }

        return null;
    }
    
    //Проверка заполнения полей
    private bool CheckFields()
    {
        try
        {
            if (LoginTB == "" || EmailTB == "" || PasswordTB == "")
            {
                StatusText = "Заполните все поля!";
                HideStatus();
                return false;
            }

            if (!EmailTB.Contains("@") || EmailTB.Length < 10)
            {
                StatusText = "Почта заполнена некорректно!";
                HideStatus();
                return false;
            }

            if (LoginTB.Length < 10)
            {
                StatusText = "Минимальная длинна логина 10 символов!";
                HideStatus();
                return false;
            }

            if (PasswordTB.Length < 8)
            {
                StatusText = "Минимальная длинна пароля 8 символов!";
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

    public static UsersCRUDVM GetInstance()
    {
        if (Instance == null)
            Instance = new UsersCRUDVM();
        Instance.StatusText = "";
        Instance.EmailTB = "";
        Instance.LoginTB = "";
        Instance.LastEnterTB = "";
        Instance.AdminChanged = false;
        Instance.PasswordTB = "";
        Instance.PokemonsListTiles = new ObservableCollection<PokemonTile>();
        return Instance;
    }
    enum UserMgtMode
    {
        CreateUser = 1,
        ChangeUser
    }
    
    //Заполнение списка избранных покемонов
    private void FillLikedList(string login)
    {
        PokemonsListTiles = new ObservableCollection<PokemonTile>();
        string Parameters = "login=" + login;

        string LikedPokemonsStr;

        try
        {
            LikedPokemonsStr = Program.wc.DownloadString(Program.HostAdress + "/GetLikedPokemons?" + Parameters);
        }
        catch (Exception ex)
        {
            LikedPokemonsStr = "";
        }

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
}