using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokemonsAPI.Context;
using PokemonsAPI.Models;
using PokemonsAPI.Models;
using PokemonsAPI.Moderls;

namespace PokemonsAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class GetUserMobileAccount : ControllerBase
{
    [Authorize]
    [HttpGet(Name = "GetUserMobileAccount")]
    public IActionResult Get(string login)
    {
        PokemonsContext context = new PokemonsContext();

        try
        {
            UserMobileAccount account = new UserMobileAccount();
            //Заполнение данных об аккаунте пользователя
            var user = context.Users.Where(u => u.Login == login).FirstOrDefault();
	        account.User = user.Id;
            account.Email = user.Email;
            account.Login = user.Login;
            account.UserNavigation = user;

            //Если пользователь не найден
            if (account.User == null)
            {
               return NotFound("AccountNotFound");
            }

            //Установка данных о достижении, если оно есть
            if (user.Achivment != null)
            {
                Pokemon pokemon = context.Pokemons.Where(p => p.Url == user.Achivment).FirstOrDefault();
                account.Pokemon = pokemon.Url;
                account.PokemonNavigation = pokemon;
            }

            return Ok(JsonSerializer.Serialize(account, new JsonSerializerOptions() { 	ReferenceHandler = ReferenceHandler.IgnoreCycles }));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
