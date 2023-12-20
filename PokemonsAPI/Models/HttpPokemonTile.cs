namespace PokemonsAPI.Controllers;

public class HttpPokemonTile
{
    public string Title { get; set; } = null!;
    public int Number { get; set; }
    public string Image { get; set; }
    public string Url { get; set; }
    public int Health { get; set; }
    public int Attack { get; set; }
    public int Protection { get; set; }
    public int SpecialAttack { get; set; }
    public int SpecialProtection { get; set; }
}