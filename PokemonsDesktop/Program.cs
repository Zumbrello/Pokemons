using Avalonia;
using Avalonia.ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Timers;
using PokemonsDesktop.Models;
using PokemonsDesktop.ViewModels;

namespace PokemonsDesktop;

class Program
{
    public static string HostAdress = "http://5.144.96.227:5555";
    //public static string HostAdress = "http://localhost:5269";
    public static WebClient wc = new WebClient();
    public static System.Timers.Timer timer = new System.Timers.Timer();
    public static HttpClient httpClient = new HttpClient();
    
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    /*public static void Main(string[] args) => BuildAvaloniaApp()
        .StartWithClassicDesktopLifetime(args);*/
    public static void Main(string[] args)
    {
        timer.Interval = 1000 * 60 * 50;
        timer.Elapsed += timer_Elapsed;
        timer.AutoReset = true;
        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    static void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
        UpdateTokens();
    }

    public static void UpdateTokens()
    {
        string url = HostAdress + "/Login/UpdateTokens";

        try
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("Authorization", "Bearer " + MainWindowVM.config.AppSettings.Settings["RefreshToken"].Value);
    
            var response = client.Send(request);
            response.EnsureSuccessStatusCode();
            JsonDocument jsonResponse = JsonDocument.Parse(new StreamReader(response.Content.ReadAsStream()).ReadToEnd());
            
            MainWindowVM.config.AppSettings.Settings.Remove("Token");
            MainWindowVM.config.AppSettings.Settings.Remove("RefreshToken");
            MainWindowVM.config.AppSettings.Settings.Add("Token",
                Convert.ToString(jsonResponse.RootElement.GetProperty("token").ToString()));
            MainWindowVM.config.AppSettings.Settings.Add("RefreshToken",
                Convert.ToString(jsonResponse.RootElement.GetProperty("refreshToken").ToString()));
            MainWindowVM.config.Save();
            wc.Headers.Clear();
            wc.Headers.Add("Authorization", "Bearer " + MainWindowVM.config.AppSettings.Settings["Token"].Value);
            
            url = HostAdress + "/GetUserById?id=" + jsonResponse.RootElement.GetProperty("id");
            request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("Authorization", "Bearer " + MainWindowVM.config.AppSettings.Settings["Token"].Value);
    
            response = client.Send(request);
            response.EnsureSuccessStatusCode();
            jsonResponse = JsonDocument.Parse(new StreamReader(response.Content.ReadAsStream()).ReadToEnd());

            User user = JsonSerializer.Deserialize<User>(jsonResponse.RootElement);
            MainWindowVM.config.AppSettings.Settings["IsAdmin"].Value = user.Isadmin.ToString();
            MenuVM.GetInstance().ShowForAdmin = Convert.ToBoolean(MainWindowVM.config.AppSettings.Settings["IsAdmin"].Value);
        }
        catch (Exception ex)
        {
            MainWindowVM.config.AppSettings.Settings.Remove("RefreshToken");
            MainWindowVM.config.AppSettings.Settings.Remove("Token");
            MainWindowVM.GetInstance().CurrentVM = AuthVM.GetInstance();
            MainWindowVM.GetInstance().MenuIsVisible = false;
        }
    }
    
    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace()
            .UseReactiveUI();
}