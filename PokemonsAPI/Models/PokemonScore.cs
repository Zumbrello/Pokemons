using System;
using System.Collections.Generic;
using PokemonsAPI.Moderls;

namespace PokemonsAPI.Models;

public partial class PokemonScore
{
    public int Id { get; set; }

    public int Score { get; set; }

    public string Pokemon { get; set; } = null!;

    public virtual Pokemon PokemonNavigation { get; set; } = null!;
}
