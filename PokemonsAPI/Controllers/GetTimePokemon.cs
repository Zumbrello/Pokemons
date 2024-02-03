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
public class GetTimePokemon : ControllerBase
{
    [Authorize]
    [HttpGet(Name = "GetTimePokemon")]
    public IActionResult Get(int mode)
    {
        PokemonsContext context = new PokemonsContext();

        //1 - покемон дня
        //2 - покемон недели
        //3 - покемон месяца

        try
        {
            Pokemon oldPokemon = context.Pokemons.Where(p => p.ExpiredType == mode).FirstOrDefault();
            
            //Проверка необходимости смены покемона
            if (oldPokemon != null && oldPokemon.Expired > DateOnly.FromDateTime(DateTime.Now))
            {
                return Ok(oldPokemon.Url);
            }

            Pokemon newPokemon = context.Pokemons.ToList()[new Random().Next(0, context.Pokemons.ToList().Count) - 1];
            
            //Изменение параметров предыдущего покемона, если он раньше был выбран
            if (oldPokemon != null)
            {
                oldPokemon.ExpiredType = null;
                oldPokemon.Expired = null;
            }

            //Выбор покемона дня
            if (mode == 1)
            {
                newPokemon.ExpiredType = 1;
                newPokemon.Expired = DateOnly.FromDateTime(DateTime.Now).AddDays(1);
            }
            //Выбор покемона недели
            else if (mode == 2)
            {
                newPokemon.ExpiredType = 2;
                newPokemon.Expired = DateOnly.FromDateTime(DateTime.Now).AddDays(7);
            }
            //Выбор покемона месяца
            else if (mode == 3)
            {
                newPokemon.ExpiredType = 3;
                newPokemon.Expired = DateOnly.FromDateTime(DateTime.Now).AddDays(31);
            }
            else
            {
                return BadRequest("BadMode");
            }

            context.SaveChanges();
            return Ok(newPokemon.Url);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}