using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PokemonsAPI.Context;
using PokemonsAPI.Models;
using PokemonsAPI.Models;
using PokemonsAPI.Moderls;

namespace PokemonsAPI.Controllers;
//Получение определённого количества покемонов
[ApiController]
[Route("[controller]")]
public class GetCountPokemons : ControllerBase
{
    [Authorize]
    [HttpGet(Name = "GetCountPokemons")]
    public IActionResult Get(int offset, int count)
    {
        try
        {
            PokemonsContext context = new PokemonsContext();
            List<Pokemon> pokemonsList = context.Pokemons.OrderBy(p => p.Number).Skip(offset).Take(count).ToList();
        
            if (pokemonsList.Count > 0)
            {
                return Ok(JsonSerializer.Serialize(pokemonsList)); 
            }
            else
            {
                return NotFound("Pokemon not found"); 
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message); 
        }
    }
}