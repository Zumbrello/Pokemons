using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokemonsAPI.Context;
using PokemonsAPI.Models;
using PokemonsAPI.Models;

namespace PokemonsAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class GetUserController : ControllerBase
{
    [Authorize]
    [HttpGet(Name = "GetUser")]
    public IActionResult Get(string login = "")
    {
        try
        {
            PokemonsContext context = new PokemonsContext();

            List<User> users = new List<User>();

            //Если логин не указан - вернуть всех пользователей, иначе 1
            if (login == "")
            {
                users = context.Users.ToList();
            }
            else
            {
                users = context.Users.Where(u => u.Login == login).ToList();
            }

            return Ok(JsonSerializer.Serialize(users));

        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}