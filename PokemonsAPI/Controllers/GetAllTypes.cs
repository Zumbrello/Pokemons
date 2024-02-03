using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokemonsAPI.Context;
using PokemonsAPI.Models;

namespace PokemonsAPI.Controllers;

//Получение всех типов покемонов
[ApiController]
[Route("[controller]")]
public class GetAllTypes : ControllerBase
{
    [Authorize]
    [HttpGet(Name = "GetAllTypes")]
    public IActionResult Get()
    {
        try
        {
            return Ok(JsonSerializer.Serialize(new PokemonsContext().ElementTypes.ToList()));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message); 
        }
    }
}