using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokemonsAPI.Context;
using PokemonsAPI.Models;

namespace PokemonsAPI.Controllers;

//Получение всех покемонов
[ApiController]
[Route("[controller]")]
public class GetAllPokemons : ControllerBase
{
    [Authorize]
    [HttpGet(Name = "GetAllPokemons")]
    public IActionResult Get()
    {
        try
        {
            return Ok(JsonSerializer.Serialize(new PokemonsContext().Pokemons.ToList())); 
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message); 
        }
    }
}