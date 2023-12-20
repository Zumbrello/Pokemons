using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Windows.Input;
using ReactiveUI;

namespace PokemonsDesktop.ViewModels;

public class RegisterVM : ViewModelBase
{
    private static RegisterVM instance;
    private static string _EmailText;
    private static string _LoginText;
    private static string _PasswordText;
    private static string _RepeatPasswordText;
    private static string _StatusText;
    
    //Геттеры и сеттеры полей
    public string EmailText
    {
        get { return _EmailText; }
        set { this.RaiseAndSetIfChanged(ref _EmailText, value); }
    }
    
    public string LoginText
    {
        get { return _LoginText; }
        set { this.RaiseAndSetIfChanged(ref _LoginText, value); }
    }
    
    public string PasswordText
    {
        get { return _PasswordText; }
        set { this.RaiseAndSetIfChanged(ref _PasswordText, value); }
    }
    
    public string RepeatPasswordText
    {
        get { return _RepeatPasswordText; }
        set { this.RaiseAndSetIfChanged(ref _RepeatPasswordText, value); }
    }
    
    public string StatusText
    {
        get { return _StatusText; }
        set { this.RaiseAndSetIfChanged(ref _StatusText, value); }
    }
    
    public static RegisterVM GetInstance()
    {
        if (instance == null)
            instance = new RegisterVM();
        return instance;
    }

    public ICommand RegisterBtn_OnClickCommand { get; }

    //Нажатие на кнопку регистрации
    private void RegisterBtn_OnClick()
    {
        if (PasswordText != RepeatPasswordText)
        {
            StatusText = "Пароли не совпадают !";
            return;
        }

        if (PasswordText.Length < 8)
        {
            StatusText = "Пароль должен быть не менее, чем из 8 символов";
            return;
        }
        
        if (EmailText.Length  < 5 || !EmailText.Contains("@"))
        {
            StatusText = "Введённая почта не корректна";
            return;
        }
        
        if (LoginText.Length  < 5)
        {
            StatusText = "Логин должен быть не менее, чем из 5 символов";
            return;
        }
        
        string RequestURL =
            string.Format(Program.HostAdress + "/CreateUser?login={0}&isAdmin=false&password={1}&email={2}", 
                LoginText, PasswordText, EmailText);
        
        JsonDocument Response = JsonDocument.Parse(Program.wc.UploadString(RequestURL, ""));

        if (Response.RootElement.GetProperty("error").ToString() != "null")
        {
            StatusText = "Данный логин уже занят";
            return;    
        }

        MainWindowVM.GetInstance().MenuIsVisible = false;
        AuthVM.GetInstance().GetRemember();
        MainWindowVM.GetInstance().CurrentVM = null;
        MainWindowVM.GetInstance().CurrentVM = AuthVM.GetInstance();
    }
    
    //Возврат на окно входа
    public ICommand BackBtn_OnClickCommand { get; }

    private void BackBtn_OnClick()
    {
        AuthVM.GetInstance().GetRemember();
        MainWindowVM.GetInstance().CurrentVM = AuthVM.GetInstance();
    }
    
    //Выход из приложения
    public ICommand ExitBtn_OnClickCommand { get; }

    private void ExitBtn_OnClick()
    {
        Environment.Exit(0);   
    }

    private RegisterVM()
    {
        RegisterBtn_OnClickCommand = ReactiveCommand.Create(RegisterBtn_OnClick);
        BackBtn_OnClickCommand = ReactiveCommand.Create(BackBtn_OnClick);
        ExitBtn_OnClickCommand = ReactiveCommand.Create(ExitBtn_OnClick);
    }
    
    
}