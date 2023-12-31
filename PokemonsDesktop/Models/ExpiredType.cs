﻿using System;
using System.Collections.Generic;

namespace PokemonsAPI.Models;

public partial class ExpiredType
{
    public string Name { get; set; } = null!;

    public int Id { get; set; }

    public virtual ICollection<Pokemon> Pokemons { get; set; } = new List<Pokemon>();
}
