using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PokemonsAPI.Models;
using PokemonsAPI.Moderls;

namespace PokemonsAPI.Context;

public partial class PokemonsContext : DbContext
{
    public PokemonsContext()
    {
    }

    public PokemonsContext(DbContextOptions<PokemonsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ElementType> ElementTypes { get; set; }

    public virtual DbSet<Evolution> Evolutions { get; set; }

    public virtual DbSet<ExperienceGroup> ExperienceGroups { get; set; }

    public virtual DbSet<ExpiredType> ExpiredTypes { get; set; }

    public virtual DbSet<LikedPokemon> LikedPokemons { get; set; }

    public virtual DbSet<Pokemon> Pokemons { get; set; }

    public virtual DbSet<PokemonScore> PokemonScores { get; set; }

    public virtual DbSet<PokemonSkill> PokemonSkills { get; set; }

    public virtual DbSet<PokemonToSkill> PokemonToSkills { get; set; }

    public virtual DbSet<PokemonToType> PokemonToTypes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserJwt> UserJwts { get; set; }

    public virtual DbSet<UserMobileAccount> UserMobileAccounts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=5.144.96.227;Port=2345;Database=pokemons;Username=postgres;password=8567");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ElementType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("element_types_pk");

            entity.ToTable("element_types");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
        });

        modelBuilder.Entity<Evolution>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("evolutions_pk");

            entity.ToTable("evolutions");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.NextPokemon)
                .HasColumnType("character varying")
                .HasColumnName("next_pokemon");
            entity.Property(e => e.PrevPokemon)
                .HasColumnType("character varying")
                .HasColumnName("prev_pokemon");
            entity.Property(e => e.Requirement)
                .HasColumnType("character varying")
                .HasColumnName("requirement");

            entity.HasOne(d => d.NextPokemonNavigation).WithMany(p => p.EvolutionNextPokemonNavigations)
                .HasForeignKey(d => d.NextPokemon)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("evolutions_fk_1");

            entity.HasOne(d => d.PrevPokemonNavigation).WithMany(p => p.EvolutionPrevPokemonNavigations)
                .HasForeignKey(d => d.PrevPokemon)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("evolutions_fk");
        });

        modelBuilder.Entity<ExperienceGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ecperience_groups_pk");

            entity.ToTable("experience_groups");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('ecperience_groups_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
        });

        modelBuilder.Entity<ExpiredType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("expired_types_pk");

            entity.ToTable("expired_types");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
        });

        modelBuilder.Entity<LikedPokemon>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("liked_pokemons_pk");

            entity.ToTable("liked_pokemons");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Pokemon)
                .HasColumnType("character varying")
                .HasColumnName("pokemon");
            entity.Property(e => e.User).HasColumnName("user");

            entity.HasOne(d => d.PokemonNavigation).WithMany(p => p.LikedPokemons)
                .HasForeignKey(d => d.Pokemon)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("liked_pokemons_fk_1");

            entity.HasOne(d => d.UserNavigation).WithMany(p => p.LikedPokemons)
                .HasForeignKey(d => d.User)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("liked_pokemons_fk");
        });

        modelBuilder.Entity<Pokemon>(entity =>
        {
            entity.HasKey(e => e.Url).HasName("pokemon_pk");

            entity.ToTable("pokemon");

            entity.Property(e => e.Url)
                .HasColumnType("character varying")
                .HasColumnName("url");
            entity.Property(e => e.Attack).HasColumnName("attack");
            entity.Property(e => e.ExpGroup).HasColumnName("exp_group");
            entity.Property(e => e.Expired).HasColumnName("expired");
            entity.Property(e => e.ExpiredType).HasColumnName("expired_type");
            entity.Property(e => e.Female).HasColumnName("female");
            entity.Property(e => e.HatchingPeriod).HasColumnName("hatching_period");
            entity.Property(e => e.Health).HasColumnName("health");
            entity.Property(e => e.Height).HasColumnName("height");
            entity.Property(e => e.Image)
                .HasColumnType("character varying")
                .HasColumnName("image");
            entity.Property(e => e.Male).HasColumnName("male");
            entity.Property(e => e.Number).HasColumnName("number");
            entity.Property(e => e.Protection).HasColumnName("protection");
            entity.Property(e => e.Rate).HasColumnName("rate");
            entity.Property(e => e.SpecialAttack).HasColumnName("special_attack");
            entity.Property(e => e.SpecialProtection).HasColumnName("special_protection");
            entity.Property(e => e.Speed).HasColumnName("speed");
            entity.Property(e => e.Summary).HasColumnName("summary");
            entity.Property(e => e.Title)
                .HasColumnType("character varying")
                .HasColumnName("title");
            entity.Property(e => e.Weight).HasColumnName("weight");

            entity.HasOne(d => d.ExpGroupNavigation).WithMany(p => p.Pokemons)
                .HasForeignKey(d => d.ExpGroup)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("pokemon_fk");

            entity.HasOne(d => d.ExpiredTypeNavigation).WithMany(p => p.Pokemons)
                .HasForeignKey(d => d.ExpiredType)
                .HasConstraintName("pokemon_expired_type");
        });

        modelBuilder.Entity<PokemonScore>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pokemon_score_pk");

            entity.ToTable("pokemon_score");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Pokemon)
                .HasColumnType("character varying")
                .HasColumnName("pokemon");
            entity.Property(e => e.Score).HasColumnName("score");

            entity.HasOne(d => d.PokemonNavigation).WithMany(p => p.PokemonScores)
                .HasForeignKey(d => d.Pokemon)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("pokemon_score_fk");
        });

        modelBuilder.Entity<PokemonSkill>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pokemon_skills_pk");

            entity.ToTable("pokemon_skills");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasColumnType("character varying")
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
        });

        modelBuilder.Entity<PokemonToSkill>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pokemon_to_skill_pk");

            entity.ToTable("pokemon_to_skill");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Pokemon)
                .HasColumnType("character varying")
                .HasColumnName("pokemon");
            entity.Property(e => e.Skill).HasColumnName("skill");

            entity.HasOne(d => d.PokemonNavigation).WithMany(p => p.PokemonToSkills)
                .HasForeignKey(d => d.Pokemon)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("pokemon_to_skill_fk");

            entity.HasOne(d => d.SkillNavigation).WithMany(p => p.PokemonToSkills)
                .HasForeignKey(d => d.Skill)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("pokemon_to_skill_fk_1");
        });

        modelBuilder.Entity<PokemonToType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pokemon_to_type_pk");

            entity.ToTable("pokemon_to_type");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Pokemon)
                .HasColumnType("character varying")
                .HasColumnName("pokemon");
            entity.Property(e => e.Type).HasColumnName("type");

            entity.HasOne(d => d.PokemonNavigation).WithMany(p => p.PokemonToTypes)
                .HasForeignKey(d => d.Pokemon)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("pokemon_to_type_fk");

            entity.HasOne(d => d.TypeNavigation).WithMany(p => p.PokemonToTypes)
                .HasForeignKey(d => d.Type)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("pokemon_to_type_fk_1");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("newtable_pk");

            entity.ToTable("users");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('newtable_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.Achivment)
                .HasColumnType("character varying")
                .HasColumnName("achivment");
            entity.Property(e => e.Email)
                .HasColumnType("character varying")
                .HasColumnName("email");
            entity.Property(e => e.Isadmin).HasColumnName("isadmin");
            entity.Property(e => e.Lastlogin)
                .HasColumnType("character varying")
                .HasColumnName("lastlogin");
            entity.Property(e => e.Login)
                .HasColumnType("character varying")
                .HasColumnName("login");
            entity.Property(e => e.Password)
                .HasColumnType("character varying")
                .HasColumnName("password");

            entity.HasOne(d => d.AchivmentNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.Achivment)
                .HasConstraintName("users_fk");
        });

        modelBuilder.Entity<UserJwt>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_jwt_pk");

            entity.ToTable("user_jwt");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.JwtAccess)
                .HasColumnType("character varying")
                .HasColumnName("jwt_access");
            entity.Property(e => e.JwtRefresh)
                .HasColumnType("character varying")
                .HasColumnName("jwt_refresh");
            entity.Property(e => e.User).HasColumnName("user");

            entity.HasOne(d => d.UserNavigation).WithMany(p => p.UserJwts)
                .HasForeignKey(d => d.User)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_jwt_fk");
        });

        modelBuilder.Entity<UserMobileAccount>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_mobile_account_pk");

            entity.ToTable("user_mobile_account");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Pokemon)
                .HasColumnType("character varying")
                .HasColumnName("pokemon");
            entity.Property(e => e.User).HasColumnName("user");

            entity.HasOne(d => d.PokemonNavigation).WithMany(p => p.UserMobileAccounts)
                .HasForeignKey(d => d.Pokemon)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_mobile_account_fk");

            entity.HasOne(d => d.UserNavigation).WithMany(p => p.UserMobileAccounts)
                .HasForeignKey(d => d.User)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_mobile_account_fk_1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
