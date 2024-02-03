using System;
using System.Collections.Generic;
using PokemonsDesktop.Moderls;

namespace PokemonsDesktop.Models;

public partial class PokemonScore
{
    public int Id { get; set; }

    public int Score { get; set; }

    public string Pokemon { get; set; } = null!;

    public virtual Pokemon PokemonNavigation { get; set; } = null!;
}
