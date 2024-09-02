namespace Domain;

public class Ingredient : BaseEntity
{
    public Double Qty { get; set; }
    public string? Comment { get; set; }
    public Guid RecipeId { get; set; }
    public Recipe? Recipe { get; set; }

    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
}