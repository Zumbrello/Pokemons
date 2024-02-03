using System;
using System.Collections.Generic;
using PokemonsDesktop.Moderls;

namespace PokemonsDesktop.Models;

public partial class PokemonToType
{
    public int Id { get; set; }

    public string Pokemon { get; set; } = null!;

    public int Type { get; set; }

    public virtual Pokemon PokemonNavigation { get; set; } = null!;

    public virtual ElementType TypeNavigation { get; set; } = null!;
}
