using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages.Recipes;

public class Ingredients : PageModel
{
    private readonly DAL.AppDbContext _context;
    
    public Ingredients(DAL.AppDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public int Servings { get; set; }
    public Recipe Recipe { get; set; } = default!;
    [BindProperty(SupportsGet = true)]
    public Guid? RecipeId { get; set; }

    [BindProperty(SupportsGet = true)]
    public decimal Times { get; set; }
    
    public async Task<IActionResult> OnGet()
    {
        if (RecipeId == null)
        {
            return NotFound();
            
        }
        var recipe = await _context.Recipes
            .Include(r => r.Ingredients)!
            .ThenInclude(p => p.Product)
            .FirstOrDefaultAsync(r => RecipeId == r.Id);
        
        if (recipe != null)
        {
            if (Times != 0)
            {
                foreach (var ingredient in recipe.Ingredients!)
                {
                    ingredient.Qty /= (double) Times;
                }
            }
            Recipe = recipe;
            return Page();
        }
        
        return NotFound();
    }

    public async Task<IActionResult> OnPost()
    {
        var recipe = await _context.Recipes
            .Include(r => r.Ingredients)!
            .ThenInclude(p => p.Product)
            .FirstOrDefaultAsync(r => RecipeId == r.Id);

        var times = (decimal) recipe!.Servings / Servings;
        return RedirectToPage("/Recipes/Ingredients", new {RecipeId, Times = times});
    }
}