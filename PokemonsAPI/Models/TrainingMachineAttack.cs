using System;
using System.Collections.Generic;

namespace PokemonsAPI.Models;

public partial class TrainingMachineAttack
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int Type { get; set; }

    public int? Power { get; set; }

    public int? Accuracy { get; set; }

    public int? MovePoints { get; set; }

    public string Description { get; set; } = null!;

    public virtual ICollection<TrainingMachineAttacksToPokemon> TrainingMachineAttacksToPokemons { get; set; } = new List<TrainingMachineAttacksToPokemon>();
}
