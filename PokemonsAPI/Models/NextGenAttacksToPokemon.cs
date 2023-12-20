using System;
using System.Collections.Generic;

namespace PokemonsAPI.Models;

public partial class NextGenAttacksToPokemon
{
    public int Id { get; set; }

    public string Pokemon { get; set; } = null!;

    public int Attack { get; set; }

    public virtual NextGenAttack AttackNavigation { get; set; } = null!;

    public virtual Pokemon PokemonNavigation { get; set; } = null!;
}
