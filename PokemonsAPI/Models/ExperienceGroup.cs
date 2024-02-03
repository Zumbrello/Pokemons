using System;
using System.Collections.Generic;
using PokemonsAPI.Moderls;

namespace PokemonsAPI.Models;

public partial class ExperienceGroup
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Pokemon> Pokemons { get; set; } = new List<Pokemon>();
}
