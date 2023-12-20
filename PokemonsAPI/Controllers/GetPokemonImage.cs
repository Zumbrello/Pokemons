using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PokemonsAPI.Context;
using PokemonsAPI.Models;

namespace PokemonsAPI.Controllers;
//Получить изображение покемона
[ApiController]
[Route("[controller]")]
public class GetPokemonImage : ControllerBase
{
    [Authorize]
    [HttpGet(Name = "GetPokemonImage")]
    public IActionResult Get(string pokemon)
    {
        PokemonsContext context = new PokemonsContext();

        try
        {
            string? img = context.Pokemons.Where(p => p.Url == pokemon).FirstOrDefault()?.Image;
            return Ok(img == null ? "null" : img);

        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message); 
        }
    }
}