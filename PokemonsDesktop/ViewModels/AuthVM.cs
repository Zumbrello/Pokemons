using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Text.Json;
using System.Windows.Input;
using Avalonia.Controls;
using PokemonsDesktop.Views;
using PokemonsDesktop.Models;
using ReactiveUI;

namespace PokemonsDesktop.ViewModels;

public class AuthVM : ViewModelBase
{ 
    private static AuthVM instance;
    private static string _PasswordText;
    private static string _LoginText;
    private static string _StatusText;
    private static bool _RememberMe;
    private static bool _ShowForAdmin;
    
    //Сеттеры закрытых полей, к которым происходит Binding
    public string PasswordText
    {
        get { return _PasswordText; }
        set { this.RaiseAndSetIfChanged(ref _PasswordText, value); }
    }

    public string LoginText
    {
        get { return _LoginText; }
        set { this.RaiseAndSetIfChanged(ref _LoginText, value); }
    }

    public string StatusText
    {
        get { return _StatusText; }
        set { this.RaiseAndSetIfChanged(ref _StatusText, value); }
    }
    
    public bool RememberMe
    {
        get { return _RememberMe;}
        set { this.RaiseAndSetIfChanged(ref _RememberMe, value); }
    }
    
    //Получение объекта-одиночки
    public static AuthVM GetInstance()
    {
        if (instance == null)
            instance = new AuthVM();
        instance.StatusText = "";
        return instance;
    }

    private AuthVM()
    {}

    //Установить сохранённые данные авторизации
    private void SetRemember()
    {
        string RememberMeValue = RememberMe.ToString();
        string LoginTextValue = LoginText;
        string PasswordTextValue = PasswordText;

        if (!RememberMe)
        { 
            RememberMeValue = "false";
            LoginTextValue = "";
            PasswordTextValue = "";
        }
        
        if (MainWindowVM.config.AppSettings.Settings["RememberMe"]?.Value == null)
        {
            MainWindowVM.config.AppSettings.Settings.Add("RememberMe", RememberMeValue); 
            MainWindowVM.config.Save();
        }
        else
        {
            MainWindowVM.config.AppSettings.Settings.Remove("RememberMe");
            MainWindowVM.config.AppSettings.Settings.Add("RememberMe", RememberMeValue); 
            MainWindowVM.config.Save();
        }
            
        if (MainWindowVM.config.AppSettings.Settings["Login"]?.Value == null)
        {
            MainWindowVM.config.AppSettings.Settings.Add("Login", LoginTextValue); 
            MainWindowVM.config.Save();
        }
        else
        {
            MainWindowVM.config.AppSettings.Settings.Remove("Login");
            MainWindowVM.config.AppSettings.Settings.Add("Login", LoginTextValue); 
            MainWindowVM.config.Save();
        }
            
        if (MainWindowVM.config.AppSettings.Settings["Password"]?.Value == null)
        {
            MainWindowVM.config.AppSettings.Settings.Add("Password", PasswordTextValue); 
            MainWindowVM.config.Save();
        }
        else
        {
            MainWindowVM.config.AppSettings.Settings.Remove("Password");
            MainWindowVM.config.AppSettings.Settings.Add("Password", PasswordTextValue); 
            MainWindowVM.config.Save();
        }
    }
    
    //Получить сохранённые данные авторизации
    public void GetRemember()
    {
        if (MainWindowVM.config.AppSettings.Settings["RememberMe"]?.Value != null)
        { 
            RememberMe = Convert.ToBoolean(MainWindowVM.config.AppSettings.Settings["RememberMe"].Value);
            if (!RememberMe)
            {
                return;
            }
        }
         
        if (MainWindowVM.config.AppSettings.Settings["Login"]?.Value != null)
        {
            LoginText = MainWindowVM.config.AppSettings.Settings["Login"].Value;
        }
        
        if (MainWindowVM.config.AppSettings.Settings["Password"]?.Value != null)
        {
            PasswordText = MainWindowVM.config.AppSettings.Settings["Password"].Value;
        }
    }
    
    //Команда входа и её функция
    public ICommand EnterBtn()
    {
        string Token;
        string url = Program.HostAdress + "/Login/Login?login=" + LoginText + "&password=" + PasswordText;

        JsonDocument request;
        try
        {
            request = JsonDocument.Parse(Program.wc.UploadString(url, "POST", ""));
        }
        catch (Exception ex)
        {
            StatusText = ex.Message;
            return null;
        }

        Token = Convert.ToString(request.RootElement.GetProperty("refreshToken").ToString());
        Program.wc.Headers.Clear();
        Program.wc.Headers.Add("Authorization", "Bearer " + Token);
        User CurrentUser = JsonSerializer.Deserialize<List<User>>(JsonDocument
            .Parse(Program.wc.DownloadString(Program.HostAdress + "/GetUser?" + "login=" + LoginText)).RootElement)[0];
        
        //Проверка сохранённых данных входа
        if (MainWindowVM.config.AppSettings.Settings["Login"]?.Value == null)
        {
            MainWindowVM.config.AppSettings.Settings.Add("Token", request.RootElement.GetProperty("token").ToString()); 
            MainWindowVM.config.AppSettings.Settings.Add("RefreshToken", request.RootElement.GetProperty("refreshToken").ToString());
            MainWindowVM.config.AppSettings.Settings.Add("Login", LoginText); 
            MainWindowVM.config.AppSettings.Settings.Add("IsAdmin", CurrentUser.Isadmin.ToString());
            MainWindowVM.config.Save();
        }
        else
        {
            MainWindowVM.config.AppSettings.Settings.Remove("Token");
            MainWindowVM.config.AppSettings.Settings.Remove("RefreshToken");
            MainWindowVM.config.AppSettings.Settings.Remove("Login");
            MainWindowVM.config.AppSettings.Settings.Remove("IsAdmin");
            MainWindowVM.config.AppSettings.Settings.Add("Token", request.RootElement.GetProperty("token").ToString());
            MainWindowVM.config.AppSettings.Settings.Add("RefreshToken", request.RootElement.GetProperty("refreshToken").ToString());
            MainWindowVM.config.AppSettings.Settings.Add("Login", LoginText);
            MainWindowVM.config.AppSettings.Settings.Add("IsAdmin", CurrentUser.Isadmin.ToString());
            MainWindowVM.config.Save();
        }
        
        SetRemember();
        
        Program.timer.Start();
        MainWindowVM.GetInstance().MenuIsVisible = true;
        MainWindowVM.GetInstance().CurrentMenuVM = null;
        MainWindowVM.GetInstance().CurrentMenuVM = MenuVM.GetInstance();
        MainWindowVM.GetInstance().CurrentVM = PokemonsListVM.GetInstance();
        
        if (!RememberMe)
        {
            LoginText = "";
            PasswordText = "";
        }

        return null;
    }
}