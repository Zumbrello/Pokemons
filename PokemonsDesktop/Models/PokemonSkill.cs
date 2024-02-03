using System;
using System.Collections.Generic;

namespace PokemonsDesktop.Models;

public partial class PokemonSkill
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual ICollection<PokemonToSkill> PokemonToSkills { get; set; } = new List<PokemonToSkill>();
}
