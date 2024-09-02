using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Pages.Recipes;

public class AddIngredients : PageModel
{
    
    
    private readonly DAL.AppDbContext _context;

    public AddIngredients(DAL.AppDbContext context)
    {
        _context = context;
    }
    
    
    public List<SelectListItem> Products { get; set; } = default!;
    public List<SelectListItem> Recipes { get; set; } = default!;
    
    
    public IActionResult OnGet()
    {
        Recipes = _context.Recipes.Select(r => new SelectListItem{Text = r.Name, Value = r.Id.ToString()}).ToList();
        Products = _context.Products.Select(p => new SelectListItem{Text = p.Name, Value = p.Id.ToString()}).ToList();
        return Page();
    }
    
    [BindProperty]
    public Ingredient Ingredient { get; set; } = default!;
    public async Task<IActionResult> OnPostAddIngredient()
    {
        var errors = ModelState.Values.SelectMany(v => v.Errors);
        
        Console.WriteLine(ModelState.Keys.ToList().ToString());
        if (!ModelState.IsValid)
        {
            Console.WriteLine("Mina");
            return Page();
        }
        
        _context.Ingredients.Add(Ingredient);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
    [BindProperty]
    public Product Product { get; set; } = default!;
    public async Task<IActionResult> OnPostAddProduct()
    {
        var errors = ModelState.Values.SelectMany(v => v.Errors);
        if (!ModelState.IsValid)
        {
            return Page();
        }
        
        _context.Products.Add(Product);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}