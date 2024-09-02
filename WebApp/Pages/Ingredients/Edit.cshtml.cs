using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApp.Pages_Ingredients
{
    public class EditModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public EditModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        public Recipe Recipe { get; set; } = default!;
        public Product Product { get; set; } = default!;
        [BindProperty]
        public Ingredient Ingredient { get; set; } = default!;
        [BindProperty(SupportsGet = true)] public Guid? RecipeId { get; set; }


        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ingredient =  await _context.Ingredients.FirstOrDefaultAsync(m => m.Id == id);
            if (ingredient == null)
            {
                return NotFound();
            }
            Ingredient = ingredient;
            Recipe = _context.Recipes.First(r => r.Ingredients.Any(i => i.Id == id));
            Console.WriteLine(Recipe.Name);
            Product = (await _context.Products.FirstOrDefaultAsync(r => r.Ingredients.Any(i => i.Id == id)))!;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Ingredient).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IngredientExists(Ingredient.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            if (RecipeId != null)
            {
                return RedirectToPage("/Recipes/Ingredients", new { RecipeId });
            } 

            return RedirectToPage("./Index");
        }

        private bool IngredientExists(Guid id)
        {
            return _context.Ingredients.Any(e => e.Id == id);
        }
    }
}
