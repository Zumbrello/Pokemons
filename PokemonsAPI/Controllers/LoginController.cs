using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using PokemonsAPI.Context;
using PokemonsAPI.Models;
using PokemonsAPI.Models;

namespace PokemonsAPI.Controllers;
//Управление токенами
[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{
    private readonly JwtSettings jwtSettings;
    //Установка настроек токена
    public LoginController(JwtSettings jwtSettings) {
        this.jwtSettings = jwtSettings;
    }
    
    //Получение токенов
    [Route("Login")]
    [HttpPost]
    public IActionResult GetTokens(string login, string password)
    {
        try
        {
            PokemonsContext Context = new PokemonsContext();
            
            //Настройка токена
            var Token = new UserTokens();
            password = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
            var User = Context.Users.Where(x => x.Login == login && x.Password == password).FirstOrDefault();
            if (User != null) {
                Token = JwtHelpers.JwtHelpers.GenTokenkey(new UserTokens() {
                    UserName = User.Login
                }, jwtSettings);
            } else {
                return BadRequest($"wrong password");
            }

            //Запись данных в БД
            Context.UserJwts.Add(new UserJwt()
                { User = User.Id, JwtAccess = Token.Token, JwtRefresh = Token.RefreshToken });
            Context.SaveChanges();
            
            User.Lastlogin = DateTime.Now.ToString();
            Context.SaveChanges();
            
            return Ok(Token);
        } catch (Exception ex) {
            return BadRequest("Ошибка API");
        }
    }
    
    //Обновление токенов
    [Route("UpdateTokens")]
    [HttpGet]
    [Authorize]
    public IActionResult UpdateTokens()
    {
        PokemonsContext Context = new PokemonsContext();
        
        //Поиск старого токена в бвзе
        string token = Request.Headers.Authorization.ToString().Substring(7);
        var JwtRecord = Context.UserJwts.Where(x => x.JwtRefresh == token).FirstOrDefault();
        
        if (JwtRecord == null)
        {
            return NotFound("Токен отсутствует в базе"); 
        }
        
        User user = Context.Users.Where(u => u.Id == JwtRecord.User).FirstOrDefault();
        
        try
        {
            //Настройка токена
            var Token = new UserTokens();
            Token = JwtHelpers.JwtHelpers.GenTokenkey(new UserTokens() {
                UserName = user.Login,
            }, jwtSettings);
            
            //Изменение данных в БД
            Context.UserJwts.Remove(JwtRecord);
            Context.SaveChanges();
            
            Context.UserJwts.Add(new UserJwt()
                { User = user.Id, JwtAccess = Token.Token, JwtRefresh = Token.RefreshToken });
            user.Lastlogin = DateTime.Now.ToString();
            Context.SaveChanges();
            
            return Ok(Token);
        } catch (Exception ex) {
            return BadRequest("Ошибка API");
        }
    }
}