using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokemonsAPI.Context;
using PokemonsAPI.Models;

namespace PokemonsAPI.Controllers;
//Изменение данных о пользователе или удаление
[ApiController]
[Route("[controller]")]
public class ChangeUserController : ControllerBase
{
    [Authorize]
    [HttpPost(Name = "ChangeUser")]
    public IActionResult Post(string login, bool isAdmin, string password, int mode, string email = "")
    {
        //1 - Обновить User
        //2 - Удалить User
        PokemonsContext context = new PokemonsContext();
        
        try
        {
            User user = context.Users.Where(u => u.Login == login).FirstOrDefault();
        
            //Обновление данных
            if (mode == 1)
            {
                if (user == null)
                {
                    return NotFound("UserNotFound");
                } 
                
                //Изменеие данных пользователя
                user.Email = email;
                user.Login = login;
                user.Isadmin = isAdmin;
                user.Password = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
                
                //Запись данных в БД
                context.SaveChanges();
                return Ok();
            
            }
            //Удаление пользователя
            else if (mode == 2)
            {
                if (user == null)
                {
                    return NotFound("UserNotFound");
                }

                context.Users.Remove(user);
                //Запись данных в БД
                context.SaveChanges();
                return Ok();
            }
        
            return BadRequest("UnexpectedMode");
        }catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}