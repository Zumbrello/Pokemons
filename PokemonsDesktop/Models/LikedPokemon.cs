using System;
using System.Collections.Generic;
using PokemonsDesktop.Models;
using PokemonsAPI.Models;

namespace PokemonsAPI.Models;

public partial class LikedPokemon
{
    public int Id { get; set; }

    public string Pokemon { get; set; } = null!;

    public string User { get; set; }

    public virtual Pokemon PokemonNavigation { get; set; } = null!;

    public virtual User UserNavigation { get; set; } = null!;
}
