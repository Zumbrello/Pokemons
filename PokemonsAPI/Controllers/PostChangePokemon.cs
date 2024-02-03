using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokemonsAPI.Context;
using PokemonsAPI.Models;
using PokemonsAPI.Models;
using PokemonsAPI.Moderls;

namespace PokemonsAPI.Controllers;
//Контроллер изменения информации о покемоне
[ApiController]
[Route("[controller]")]
public class PostChangePokemon : ControllerBase
{
    [Authorize]
    [HttpPost(Name = "PostChangePokemon")]
    public IActionResult Get(string json, int mode)
    {
        PokemonsContext context = new PokemonsContext();

        //1 - Создать
        //2 - Обновить
        //3 - Удалить
        try
        {
            Pokemon pokemon = JsonSerializer.Deserialize<Pokemon>(json);
        
            //Обработка данных о покемоне в зависимости от выбранного режима
            //Создание покемона
            if(mode == 1)
            {
                try
                {
                    context.Pokemons.Add(pokemon);
                    context.SaveChanges(); 
                }
                catch (Exception ex)
                {
                    return BadRequest("DuplicatePokemon");
                }
            }
            //Изменение покемона
            else if(mode == 2)
            {
                Pokemon FindPokemon = context.Pokemons.Where(p => p.Url == pokemon.Url).FirstOrDefault();
                if (FindPokemon == null)
                { 
                    return NotFound("Pokemon not found");
                }

                //Заполнение найденных данных
                FindPokemon.Title = pokemon.Title;
                FindPokemon.Number = pokemon.Number; 
                FindPokemon.Image = pokemon.Image;
                FindPokemon.Health = pokemon.Health;
                FindPokemon.Attack = pokemon.Attack; 
                FindPokemon.Protection = pokemon.Protection;
                FindPokemon.SpecialAttack = pokemon.SpecialAttack;
                FindPokemon.SpecialProtection = pokemon.SpecialProtection;

                context.Pokemons.Update(FindPokemon);
                context.SaveChanges();
            }
            //Удаление покемона
            else if (mode == 3)
            {
                if (context.Pokemons.Where(p => p.Title == pokemon.Title).FirstOrDefault() == null)
                {
                    return NotFound("Pokemon not found");  
                }

                context.Pokemons.Remove(pokemon);
                context.SaveChanges();               
            }
            else
            {
                return BadRequest("BadMode");
            }

            context.SaveChanges();
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}