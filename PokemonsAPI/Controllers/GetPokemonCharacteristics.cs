using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokemonsAPI.Context;
using PokemonsAPI.Models;
using PokemonsAPI.Models;
using PokemonsAPI.Moderls;

namespace PokemonsAPI.Controllers;
//Получение характеристик покемона
[ApiController]
[Route("[controller]")]
public class GetPokemonCharacteristics : ControllerBase
{
    [Authorize]
    [HttpGet(Name = "GetPokemonCharacteristics")]
    public IActionResult Get(string pokemon)
    {
        try
        {
            PokemonsContext context = new PokemonsContext();
            Pokemon pokemonObj = context.Pokemons.Where(p => p.Url == pokemon).FirstOrDefault();
            
            //Проверка существования покемона
            if (pokemonObj == null)
            {
                return NotFound("Bad pokemon"); 
            }

            List<Evolution[]> EvolutionsList = new List<Evolution[]>();
            
            //Поиск следующих эволюций
            Evolution[] evo = context.Evolutions.Where(e => e.NextPokemon == pokemon).ToArray();
            EvolutionsList.Add(evo != null ? evo : new Evolution[] { });

            //Поиск предыдущих эволюций
            evo = context.Evolutions.Where(e => e.PrevPokemon == pokemon).ToArray();
            EvolutionsList.Add(evo != null ? evo : new Evolution[] { });
            
            //Заполнение строк эволюций для передачи в приложение
            string EvolutionsPrev = "";
            string EvolutionsNext = "";
            for (int i = 0; i < EvolutionsList[0].Length; i++)
            {
                EvolutionsPrev += (i != EvolutionsList[0].Length - 1 ? EvolutionsList[0][i].PrevPokemon + "," : EvolutionsList[0][i].PrevPokemon);
            }
            
            for (int i = 0; i < EvolutionsList[1].Length; i++)
            {
                EvolutionsNext += (i != EvolutionsList[1].Length - 1 ? EvolutionsList[1][i].NextPokemon + "," : EvolutionsList[1][i].NextPokemon);
            }
            
            //Подготовка данных для ответа
            string result = "{\"Health\":\"" + pokemonObj.Health + "\"" + 
                            ", \"Protection\":\"" + pokemonObj.Protection + "\"" + 
                            ", \"Attack\":\"" + pokemonObj.Attack + "\"" +  
                            ", \"SpecialProtection\":\"" + pokemonObj.SpecialProtection + "\"" +  
                            ", \"SpecialAttack\":\"" + pokemonObj.SpecialAttack + "\"" + 
                            ", \"Speed\":\"" + pokemonObj.Speed + "\"" + 
                            ", \"EvolutionsListPrev\":\"" + EvolutionsPrev + "\"" +
                            ", \"EvolutionsListNext\":\"" + EvolutionsNext + "\"" +
                            "}";

            return Ok(result);

        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message); 
        }
    }
}