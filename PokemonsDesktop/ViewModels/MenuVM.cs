using System;
using System.Windows.Input;
using Avalonia;
using ReactiveUI;

namespace PokemonsDesktop.ViewModels;

public class MenuVM : ViewModelBase
{
    private static MenuVM Instance;
    //Команды для привязки к кнопкам меню
    public ICommand AccountBtn_OnClickCommand { get; }
    public ICommand EventsBtn_OnClickCommand { get; }
    public ICommand PokedexBtn_OnClickCommand { get; }
    public ICommand CreateUserBtn_OnClickCommand { get; }
    public ICommand ChangeUserBtn_OnClickCommand { get; }
    public ICommand CreatePokemonBtn_OnClickCommand { get; } 
    public ICommand ChangePokemonBtn_OnClickCommand { get; }
    public ICommand ExitBtn_OnClickCommand { get; }

    //Должны ли отображаться пункты только для админов
    private static bool _ShowForAdmin;

    public bool ShowForAdmin
    {
        get { return _ShowForAdmin; }
        set
        {
            _ShowForAdmin = value;
            this.RaiseAndSetIfChanged(ref _ShowForAdmin, value);
        }
    }
    private MenuVM()
    {
        //Привязка команд к функциям
        AccountBtn_OnClickCommand = ReactiveCommand.Create(AccountBtn_OnClick);
        EventsBtn_OnClickCommand = ReactiveCommand.Create(EventsBtn_OnClick);
        PokedexBtn_OnClickCommand = ReactiveCommand.Create(PokedexBtn_OnClick);
        CreatePokemonBtn_OnClickCommand = ReactiveCommand.Create(CreatePokemonBtn_OnClick);
        ChangePokemonBtn_OnClickCommand = ReactiveCommand.Create(ChangePokemonBtn_OnClick);
        CreateUserBtn_OnClickCommand = ReactiveCommand.Create(CreateUserBtn_OnClick);
        ChangeUserBtn_OnClickCommand = ReactiveCommand.Create(ChangeUserBtn_OnClick);
        ExitBtn_OnClickCommand = ReactiveCommand.Create(ExitBtn_OnClick);
    }

    public static MenuVM GetInstance()
    {
        if (Instance == null)
            Instance = new MenuVM();
        
        if(MainWindowVM.config.AppSettings.Settings["IsAdmin"] != null)
        {
            Instance.ShowForAdmin = Convert.ToBoolean(MainWindowVM.config.AppSettings.Settings["IsAdmin"].Value);
        } 
        return Instance;
    }

    //Открыть форму создания пользователя 
    private void CreateUserBtn_OnClick()
    {
        UsersCRUDVM vm = UsersCRUDVM.GetInstance();
        vm.SetFormMode(1);
        MainWindowVM.GetInstance().CurrentVM = vm;      
    }
    //Открыть форму изменения пользователя
    private void ChangeUserBtn_OnClick()
    {
        UsersCRUDVM vm = UsersCRUDVM.GetInstance();
        vm.SetFormMode(2);
        MainWindowVM.GetInstance().CurrentVM = vm;       
    }
    //Открыть форму создания покемона
    private void CreatePokemonBtn_OnClick()
    {
        PokemonsCRUDVM vm = PokemonsCRUDVM.GetInstance();
        vm.SetFormMode(1);
        MainWindowVM.GetInstance().CurrentVM = vm;      
    }
    //Открыть форму изменения покемона
    private void ChangePokemonBtn_OnClick()
    {
        PokemonsCRUDVM vm = PokemonsCRUDVM.GetInstance();
        vm.SetFormMode(2);
        MainWindowVM.GetInstance().CurrentVM = vm;       
    }
    //Открыть личный кабинет
    private void AccountBtn_OnClick()
    {
        MainWindowVM.GetInstance().CurrentVM = AccountVM.GetInstance();       
    }
    //Выйти из аккаунта
    private void ExitBtn_OnClick()
    {
        try
        {
            MainWindowVM.config.AppSettings.Settings["Token"].Value = null;
            MainWindowVM.config.AppSettings.Settings["RefreshToken"].Value = null;
            MainWindowVM.config.Save();
        }catch(Exception ex){}

        MainWindowVM.GetInstance().CurrentVM = AuthVM.GetInstance();
        AuthVM.GetInstance().GetRemember();
        MainWindowVM.GetInstance().MenuIsVisible = false;
    }
    //Открыть покемонов дня/недели/месяца
    private void EventsBtn_OnClick()
    {
        MainWindowVM.GetInstance().MenuIsVisible = true;
        MainWindowVM.GetInstance().CurrentVM = EventsVM.GetInstance();       
    }
    //Открыть покедекс
    private void PokedexBtn_OnClick()
    {
        MainWindowVM.GetInstance().MenuIsVisible = true;
        MainWindowVM.GetInstance().CurrentVM = null;
        MainWindowVM.GetInstance().CurrentVM = PokemonsListVM.GetInstance();
    }
}