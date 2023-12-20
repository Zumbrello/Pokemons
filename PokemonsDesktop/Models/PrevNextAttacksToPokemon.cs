using System;
using System.Collections.Generic;

namespace PokemonsAPI.Models;

public partial class PrevNextAttacksToPokemon
{
    public int Id { get; set; }

    public string Pokemon { get; set; } = null!;

    public int Attack { get; set; }

    public virtual PrevGenAttack AttackNavigation { get; set; } = null!;

    public virtual Pokemon PokemonNavigation { get; set; } = null!;
}
