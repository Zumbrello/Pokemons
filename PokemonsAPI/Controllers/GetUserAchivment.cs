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
public class GetAchivment : ControllerBase
{
    [Authorize]
    [HttpGet(Name = "GetAchivment")]
    public IActionResult Get(string login, int pokemonType, int correction)
    {
        PokemonsContext context = new PokemonsContext();
        
        try
        {
            //Выбор покемона с нужным типом из списка с отступом равным correction
            var pokemonURL = context.PokemonToTypes.Where(p => p.Type == pokemonType).Skip(correction).FirstOrDefault().Pokemon;
            
            //Получение покемона по нужному URL
            Pokemon pokemon = context.Pokemons.Where(p => p.Url == pokemonURL).FirstOrDefault();
            User user = context.Users.Where(u => u.Login == login).FirstOrDefault();
            
            //Изменение данных о пользователе в БД
            user.Achivment = pokemonURL;
            context.SaveChanges();
            
            return Ok(JsonSerializer.Serialize(pokemon, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles }));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}