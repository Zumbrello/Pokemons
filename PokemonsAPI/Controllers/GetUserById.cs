using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PokemonsAPI.Context;
using PokemonsAPI.Models;
using PokemonsAPI.Models;

namespace PokemonsAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class GetUserById : ControllerBase
{
    [Authorize]
    [HttpGet(Name = "GetUserById")]
    public IActionResult Get(string id = "")
    {
        try
        {
            PokemonsContext context = new PokemonsContext();

            //Поиск пользователя по id
            User user = context.Users.Where(u => u.Id == Int32.Parse(id)).FirstOrDefault();

            if (user == null)
            {
                return NotFound("Пользователь не найден");
            }
            
            return Ok(JsonSerializer.Serialize(user));

        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}