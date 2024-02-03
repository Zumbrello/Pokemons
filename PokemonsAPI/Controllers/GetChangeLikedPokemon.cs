using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokemonsAPI.Context;
using PokemonsAPI.Models;
using PokemonsAPI.Models;

namespace PokemonsAPI.Controllers;
//Добавление покемона в избранное
[ApiController]
[Route("[controller]")]
public class GetChangeLikedPokemon : ControllerBase
{
    [Authorize]
    [HttpGet(Name = "GetChangeLikedPokemon")]
    public IActionResult Get(string login, string pokemon)
    {
        try
        {
            PokemonsContext context = new PokemonsContext();
            
            User SelectedUser = context.Users.Where(u => u.Login == login).FirstOrDefault();
            
            LikedPokemon LikedRecord =
                context.LikedPokemons.Where(lp => lp.Pokemon == pokemon && lp.User == SelectedUser.Id).FirstOrDefault();
           
            //Покемон был избран ?
            if (LikedRecord == null)
            {
                LikedRecord = new LikedPokemon()
                {
                    Pokemon = pokemon,
                    User = SelectedUser.Id
                };
                context.LikedPokemons.Add(LikedRecord);
                //Запись данных в БД
                context.SaveChanges();
            }
            else
            {
                context.LikedPokemons.Remove(LikedRecord);
                //Запись данных в БД
                context.SaveChanges();     
            }
            return Ok(); 
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message); 
        }
        
    }
}