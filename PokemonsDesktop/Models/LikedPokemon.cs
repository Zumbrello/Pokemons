using System;
using System.Collections.Generic;
using PokemonsDesktop.Moderls;

namespace PokemonsDesktop.Models;

public partial class LikedPokemon
{
    public int Id { get; set; }

    public int User { get; set; }

    public string Pokemon { get; set; } = null!;

    public virtual Pokemon PokemonNavigation { get; set; } = null!;

    public virtual User UserNavigation { get; set; } = null!;
}
