using Azure;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages.Ingredients;

public class CreateModel : PageModel
{
    private readonly DAL.AppDbContext _context;

    public CreateModel(DAL.AppDbContext context)
    {
        _context = context;
    }

    public List<SelectListItem> Recipes { get; set; } = default!;
    public List<SelectListItem> Products { get; set; } = default!;
    [BindProperty(SupportsGet = true)] public Guid? RecipeId { get; set; }


    public IActionResult OnGet()
    {
        Recipes = _context.Recipes.Select(r =>
            r.Id == RecipeId
                ? new SelectListItem { Text = r.Name, Value = r.Id.ToString(), Selected = true }
                : new SelectListItem { Text = r.Name, Value = r.Id.ToString() }).ToList();


        Products = _context.Products.Select(p => new SelectListItem { Text = p.Name, Value = p.Id.ToString() })
            .ToList();
        return Page();
    }

    [BindProperty] public Ingredient Ingredient { get; set; } = default!;

    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _context.Ingredients.Add(Ingredient);
        await _context.SaveChangesAsync();
        if (RecipeId != null)
        {
            return RedirectToPage("/Recipes/Ingredients", new { RecipeId });
        }

        return RedirectToPage("./Index");
    }
}