﻿using System;
using System.Collections.Generic;

namespace PokemonsAPI.Models;

public partial class PokemonToSkill
{
    public int Id { get; set; }

    public string Pokemon { get; set; } = null!;

    public int Skill { get; set; }

    public virtual Pokemon PokemonNavigation { get; set; } = null!;

    public virtual PokemonSkill SkillNavigation { get; set; } = null!;
}
