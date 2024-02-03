using System;
using System.Collections.Generic;
using PokemonsAPI.Moderls;

namespace PokemonsAPI.Models;

public partial class LikedPokemon
{
    public int Id { get; set; }

    public int User { get; set; }

    public string Pokemon { get; set; } = null!;

    public virtual Pokemon PokemonNavigation { get; set; } = null!;

    public virtual User UserNavigation { get; set; } = null!;
}
