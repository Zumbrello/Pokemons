using Microsoft.AspNetCore.Mvc;
using PokemonsAPI.Context;
using PokemonsAPI.Models;
using PokemonsAPI.Models;

namespace PokemonsAPI.Controllers;
//Создание пользователя
[ApiController]
[Route("[controller]")]
public class CreateUserController : ControllerBase
{
    [HttpPost(Name = "CreateUser")]
    public IActionResult Post(string login, bool isAdmin, string password, string email = "")
    {
        PokemonsContext context = new PokemonsContext();
        
        try
        {
            //Проверка того, существует ли пользователь с таким логином
            User user = context.Users.Where(u => u.Login == login).FirstOrDefault();

            if (user != null)
            {
                return BadRequest("UserDuplicate");
            }

            //Заполнение информации о пользователе
            user = new User();
            user.Email = email;
            user.Login = login;
            user.Isadmin = isAdmin;
            user.Password = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
            context.Users.Add(user);
            
            //Запись в БД
            context.SaveChanges();
            return Ok("true");
        }catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}