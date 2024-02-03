using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokemonsAPI.Context;
using PokemonsAPI.Models;
using PokemonsAPI.Models;
using PokemonsAPI.Moderls;

namespace PokemonsAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class GetPokemonParameters : ControllerBase
{
    [Authorize]
    [HttpGet(Name = "GetPokemonParameters")]
    public IActionResult Get(string pokemon)
    {
        PokemonsContext context = new PokemonsContext();
        
        try
        {
            Pokemon pokemonObj = context.Pokemons.Where(p => p.Url == pokemon).FirstOrDefault();

            if (pokemonObj == null)
            {
                return NotFound("Pokemon not found"); 
            }
            else
            {
                string result = "{\"Height\":\"" + pokemonObj.Height + "\", \"Weight\":\"" + pokemonObj.Weight + "\", \"ExpGroup\":\"" + pokemonObj.ExpGroup + "\"}";
                return Ok(result); 
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message); 
        }

        
    }
}