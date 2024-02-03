using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokemonsAPI.Context;
using PokemonsAPI.Models;
using PokemonsAPI.Models;

namespace PokemonsAPI.Controllers;
//Получение избранных покемонов
[ApiController]
[Route("[controller]")]
public class GetLikedPokemons :ControllerBase
{
    [Authorize]
    [HttpGet(Name = "GetLikedPokemons")]
    public IActionResult Get(string login)
    {
        try
        {
            //Получение данных о пользователе и его избранных покемонах
            PokemonsContext context = new PokemonsContext();
            User usr = context.Users.Where(u => u.Login == login).FirstOrDefault();
            List<LikedPokemon> pokemonsList = context.LikedPokemons.Where(p => p.User == usr.Id).ToList();

            string pokemons = "";

            //Заполнение списка покемонов для ответа
            for (int i = 0; i < pokemonsList.Count; i++)
            {
               pokemons += i + 1 == pokemonsList.Count ? pokemonsList[i].Pokemon : pokemonsList[i].Pokemon + ",";
            }
        
            if (pokemonsList.Count != 0)
            {
                return Ok(pokemons); 
            }
            else
            {
                return NotFound("Pokemons not found"); 
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message); 
        }
    } 
}