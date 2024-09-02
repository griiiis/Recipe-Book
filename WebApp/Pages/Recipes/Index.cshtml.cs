using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages.Recipes
{
    public class IndexModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public IndexModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        public List<Recipe> NewRecipes { get; set; } = new();
        public IList<Recipe> Recipes { get;set; } = default!;
        public IList<Product> Products { get;set; } = default!;
        [BindProperty(SupportsGet = true)]
        public string? RecipeName { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? RecipeDescription { get; set; }
        [BindProperty(SupportsGet = true)]
        public List<string>? SelectedIds { get; set; }

        public async Task OnGetAsync()
        {
            Recipes = await _context.Recipes.ToListAsync();
            
            Products = await _context.Products
                .ToListAsync();
            if (RecipeName != null || RecipeDescription != null || SelectedIds?.Count != 0)
            {
                var recipes = await _context.Recipes.Include(r => r.Ingredients)
                    .ThenInclude(i => i.Product).ToListAsync();
                if (!string.IsNullOrWhiteSpace(RecipeName))
                {
                    recipes = recipes.Where(r => r.Name.Contains(RecipeName)).ToList();
                }

                if (!string.IsNullOrWhiteSpace(RecipeDescription))
                {
                    recipes = recipes.Where(r => r.Description.Contains(RecipeDescription)).ToList();
                }

                if (SelectedIds?.Count != 0)
                {
                    recipes = recipes.Where(r => r.Ingredients.Any(i => SelectedIds!.Contains(i.ProductId.ToString()))).ToList();
                }
        
                NewRecipes = recipes.Take(6).ToList();   
            }
            
        }
        
        public async Task<IActionResult> OnPost(string? recipeName, string? recipeDescription, List<Guid>? selectedProductsIds)
        {
            return RedirectToPage("./Index", new {RecipeName = recipeName, recipeDescription,SelectedIds = selectedProductsIds});
        }
    }
}
