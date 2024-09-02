using System.ComponentModel.DataAnnotations;

namespace Domain;

public class Recipe : BaseEntity
{
    [MaxLength(128)] public string Name { get; set; } = default!;

    public string Description { get; set; } = default!;

    [Range(1, maximum: int.MaxValue)]
    public int Servings { get; set; }

    public int PrepTimeMinutes { get; set; }
    public int CookTimeMinutes { get; set; }

    public ICollection<Ingredient>? Ingredients { get; set; }
}