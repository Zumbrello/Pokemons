using System;
using System.Collections.Generic;

namespace PokemonsAPI.Models;

public partial class ElementType
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<PokemonToType> PokemonToTypes { get; set; } = new List<PokemonToType>();
}
