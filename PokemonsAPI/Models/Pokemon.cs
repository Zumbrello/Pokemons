﻿using System;
using System.Collections.Generic;

namespace PokemonsAPI.Models;

public partial class Pokemon
{
    public string Url { get; set; } = null!;

    public string Title { get; set; } = null!;

    public int Number { get; set; }

    public string Image { get; set; } = null!;

    public decimal Height { get; set; }

    public decimal Weight { get; set; }

    public int? ExpGroup { get; set; }

    public int Health { get; set; }

    public int Attack { get; set; }

    public int Protection { get; set; }

    public int SpecialAttack { get; set; }

    public int SpecialProtection { get; set; }

    public int Speed { get; set; }

    public int Summary { get; set; }

    public decimal Male { get; set; }

    public decimal Female { get; set; }

    public int? HatchingPeriod { get; set; }

    public double Rate { get; set; }

    public DateOnly? Expired { get; set; }

    public int? ExpiredType { get; set; }

    public virtual ICollection<EggsAttackToPokemon> EggsAttackToPokemons { get; set; } = new List<EggsAttackToPokemon>();

    public virtual ICollection<Evolution> EvolutionNextPokemonNavigations { get; set; } = new List<Evolution>();

    public virtual ICollection<Evolution> EvolutionPrevPokemonNavigations { get; set; } = new List<Evolution>();

    public virtual ExperienceGroup? ExpGroupNavigation { get; set; }

    public virtual ExpiredType? ExpiredTypeNavigation { get; set; }

    public virtual ICollection<LearnAttacksToPokemon> LearnAttacksToPokemons { get; set; } = new List<LearnAttacksToPokemon>();

    public virtual ICollection<LevelAttacksToPokemon> LevelAttacksToPokemons { get; set; } = new List<LevelAttacksToPokemon>();

    public virtual ICollection<LikedPokemon> LikedPokemons { get; set; } = new List<LikedPokemon>();

    public virtual ICollection<NextGenAttacksToPokemon> NextGenAttacksToPokemons { get; set; } = new List<NextGenAttacksToPokemon>();

    public virtual ICollection<PokemonScore> PokemonScores { get; set; } = new List<PokemonScore>();

    public virtual ICollection<PokemonToSkill> PokemonToSkills { get; set; } = new List<PokemonToSkill>();

    public virtual ICollection<PokemonToType> PokemonToTypes { get; set; } = new List<PokemonToType>();

    public virtual ICollection<PrevNextAttacksToPokemon> PrevNextAttacksToPokemons { get; set; } = new List<PrevNextAttacksToPokemon>();

    public virtual ICollection<TrainingMachineAttacksToPokemon> TrainingMachineAttacksToPokemons { get; set; } = new List<TrainingMachineAttacksToPokemon>();

    public virtual ICollection<UserMobileAccount> UserMobileAccounts { get; set; } = new List<UserMobileAccount>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
