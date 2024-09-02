using System.ComponentModel.DataAnnotations;

namespace Domain;

public class Product : BaseEntity
{
    [MaxLength(128)]
    public string Name { get; set; } = default!;

    public EUnit Unit { get; set; }
    
    public ICollection<Ingredient>? Ingredients { get; set; }
}