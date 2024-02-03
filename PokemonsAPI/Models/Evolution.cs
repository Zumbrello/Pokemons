using System;
using System.Collections.Generic;
using PokemonsAPI.Moderls;

namespace PokemonsAPI.Models;

public partial class Evolution
{
    public int Id { get; set; }

    public string PrevPokemon { get; set; } = null!;

    public string NextPokemon { get; set; } = null!;

    public string? Requirement { get; set; }

    public virtual Pokemon NextPokemonNavigation { get; set; } = null!;

    public virtual Pokemon PrevPokemonNavigation { get; set; } = null!;
}
