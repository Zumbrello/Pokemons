using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokemonsAPI.Context;
using PokemonsAPI.Models;

namespace PokemonsAPI.Controllers;
//Получение количества всех покемонов
[ApiController]
[Route("[controller]")]
public class GetAllPokemonsCount : ControllerBase
{
    [Authorize]
    [HttpGet(Name = "GetAllPokemonsCount")]
    public IActionResult Get()
    {
        try
        {
            return Ok(new PokemonsContext().Pokemons.ToList().Count);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message); 
        }
    }
}