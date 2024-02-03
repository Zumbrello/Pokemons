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
public class GetPokemonsByURL : ControllerBase
{
    [Authorize]
    [HttpGet(Name = "GetPokemonsByUrl")]
    public IActionResult  Get(string pokemons)
    {
        try
        {
            List<String> Pokemons = pokemons.Split(",").ToList();
            List<HttpPokemonTile> HttpPokemons = new List<HttpPokemonTile>();

            for (int i = 0; i < Pokemons.Count; i++)
            {
                Pokemon Pokemon = new PokemonsContext().Pokemons.Where(p => p.Url == Pokemons[i]).FirstOrDefault();
                if (Pokemon == null)
                {
                    continue;
                }
                
                //Добавление найденного покемона в список-ответ
                HttpPokemons.Add(new HttpPokemonTile()
                {
                    Image = Pokemon.Image,
                    Number = Pokemon.Number,
                    Title = Pokemon.Title,
                    Url = Pokemon.Url,
                    Attack = Pokemon.Attack,
                    Health = Pokemon.Health,
                    Protection = Pokemon.Protection,
                    SpecialAttack = Pokemon.SpecialAttack,
                    SpecialProtection = Pokemon.SpecialProtection
                });
            }

            return Ok(JsonSerializer.Serialize(HttpPokemons));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}