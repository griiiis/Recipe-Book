using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApp.Pages_Ingredients
{
    public class IndexModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public IndexModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        public List<Ingredient> NewIngredients { get; set; } = new();
        public IList<Ingredient> Ingredients { get;set; } = default!;
        public IList<Product> Products { get;set; } = default!;
        [BindProperty(SupportsGet = true)]
        public string? RecipeName { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? ProductName { get; set; }
        [BindProperty(SupportsGet = true)]
        public List<string>? SelectedIds { get; set; }

        public async Task OnGetAsync()
        {
            Ingredients = await _context.Ingredients
                .Include(i => i.Recipe)
                .Include(p => p.Product)
                .ToListAsync();

            Products = await _context.Products.ToListAsync();
            
            if (RecipeName != null || ProductName != null || SelectedIds?.Count != 0)
            {
                var ingredients = await _context.Ingredients.Include(r => r.Recipe)
                    .Include(i => i.Product).ToListAsync();
                if (!string.IsNullOrWhiteSpace(RecipeName))
                {
                    ingredients = ingredients.Where(r => r.Recipe!.Name.Contains(RecipeName)).ToList();
                }

                if (!string.IsNullOrWhiteSpace(ProductName))
                {
                    ingredients = ingredients.Where(r => r.Product!.Name.Contains(ProductName)).ToList();
                }

                if (SelectedIds?.Count != 0)
                {
                    ingredients = ingredients.Where(i => SelectedIds!.Contains(i.ProductId.ToString())).ToList();
                }
        
                NewIngredients = ingredients.Take(6).ToList();   
            }
        }
        public async Task<IActionResult> OnPost(string? recipeName, string? productName, List<Guid>? selectedProductsIds)
        {
            return RedirectToPage("./Index", new {RecipeName = recipeName, productName,SelectedIds = selectedProductsIds});
        }
    }
}
