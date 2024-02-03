using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokemonsAPI.Context;
using PokemonsAPI.Models;
using PokemonsAPI.Models;
using PokemonsAPI.Moderls;

namespace PokemonsAPI.Controllers;
//Получить номер эволюции покемона
[ApiController]
[Route("[controller]")]
public class GetPokemonEvolution : ControllerBase
{
    [Authorize]
    [HttpGet(Name = "GetPokemonEvolution")]
    public IActionResult Get(string pokemon)
    {
        try
        {
            PokemonsContext context = new PokemonsContext();
            Pokemon pokemonObj = context.Pokemons.Where(p => p.Url == pokemon).FirstOrDefault();
            string tmpPokemon = "temp";
            int counter = 1;

            //Проверка существования покемона
            if (pokemonObj == null)
            {
                return NotFound("Pokemon not found"); 
            }
            
            while (tmpPokemon != null)
            {
                tmpPokemon = context.Evolutions.Where(p => p.NextPokemon == pokemon).FirstOrDefault()?.PrevPokemon;
                if (tmpPokemon != null)
                {
                    counter++;
                    pokemon = tmpPokemon;
                }
            } 
        
            return Ok(counter.ToString()); 
            
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message); 
        }
    }
}