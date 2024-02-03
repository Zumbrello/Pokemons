using System;
using System.Collections.Generic;
using PokemonsAPI.Moderls;

namespace PokemonsAPI.Models;

public partial class User
{
    public int Id { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public bool Isadmin { get; set; }

    public string? Email { get; set; }

    public string? Lastlogin { get; set; }

    public string? Achivment { get; set; }

    public virtual Pokemon? AchivmentNavigation { get; set; }

    public virtual ICollection<LikedPokemon> LikedPokemons { get; set; } = new List<LikedPokemon>();

    public virtual ICollection<UserJwt> UserJwts { get; set; } = new List<UserJwt>();

    public virtual ICollection<UserMobileAccount> UserMobileAccounts { get; set; } = new List<UserMobileAccount>();
}
