using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Apetrei_Andrei_Lab2.Models;
using Apetrei_Andrei_Lab2.Models.ViewModels; // Am adăugat acest using
using System.Linq;

namespace Apetrei_Andrei_Lab2.Pages.CategoryPages
{
    public class IndexModel : PageModel
    {
        private readonly Apetrei_Andrei_Lab2Context _context;

        public IndexModel(Apetrei_Andrei_Lab2Context context)
        {
            _context = context;
        }

        public CategoryIndexData CategoryData { get; set; }
        public int CategoryID { get; set; }

        public async Task OnGetAsync(int? id) // Am adăugat parametrul id
        {
            CategoryData = new CategoryIndexData();

            // Încărcăm categoriile cu tot cu cărți și autori
            CategoryData.Categories = await _context.Category
                .Include(c => c.BookCategories)
                    .ThenInclude(bc => bc.Book)
                        .ThenInclude(b => b.Author) // Avem nevoie de autor pentru a-i afișa numele
                .OrderBy(c => c.CategoryName)
                .ToListAsync();

            // Dacă s-a apăsat pe "Select"
            if (id != null)
            {
                CategoryID = id.Value;
                Category category = CategoryData.Categories
                    .Where(c => c.ID == id.Value)
                    .Single();

                // Extragem doar cărțile categoriei selectate
                CategoryData.Books = category.BookCategories
                    .Select(bc => bc.Book)
                    .ToList();
            }
        }
    }
}