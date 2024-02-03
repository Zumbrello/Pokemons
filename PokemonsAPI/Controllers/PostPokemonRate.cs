using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokemonsAPI.Context;
using PokemonsAPI.Models;
using PokemonsAPI.Models;

namespace PokemonsAPI.Controllers;
//Установить оценку покемону
[ApiController]
[Route("[controller]")]
public class PostPokemonRate : ControllerBase
{
    [Authorize]
    [HttpPost(Name = "PostPokemonRate")]
    public IActionResult Post(string pokemon, int rate)
    {
        PokemonsContext context = new PokemonsContext();

        try
        {
            //Создание записи в БД
            PokemonScore score = new PokemonScore();
            score.Pokemon = pokemon;
            score.Score = rate;
            context.PokemonScores.Add(score);
            context.SaveChanges();

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}