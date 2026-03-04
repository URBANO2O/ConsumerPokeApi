using System;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

public class Pokemon
{
    public int id { get; set; }
    public string name { get; set; }
    public int height { get; set; }
    public int weight { get; set; }
}

[JsonSerializable(typeof(Pokemon))]
internal partial class PokemonContext : JsonSerializerContext
{
}
class Program
{
    static async Task Main()
    {
        Console.Write("Digite o nome do Pokémon: ");
        string? nome = Console.ReadLine()?.ToLower();

        if (string.IsNullOrWhiteSpace(nome))
        {
            Console.WriteLine("Nome inválido.");
            return;
        }

        using HttpClient client = new();

        try
        {
            string url = $"https://pokeapi.co/api/v2/pokemon/{nome}";
            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("Pokémon não encontrado!");
                return;
            }

            string json = await response.Content.ReadAsStringAsync();

            Pokemon? pokemon = JsonSerializer.Deserialize(
                json,
                PokemonContext.Default.Pokemon);

            if (pokemon is null)
            {
                Console.WriteLine("Erro ao desserializar.");
                return;
            }

            Console.WriteLine("\n=== Dados do Pokémon ===");
            Console.WriteLine($"Id: {pokemon.id}");
            Console.WriteLine($"Nome: {pokemon.name}");
            Console.WriteLine($"Altura: {pokemon.height}");
            Console.WriteLine($"Peso: {pokemon.weight}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro: {ex.Message}");
        }
    }
}