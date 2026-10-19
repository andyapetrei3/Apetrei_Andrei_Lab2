using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Apetrei_Andrei_Lab2.Models;

namespace Apetrei_Andrei_Lab2.Pages.CategoryPages
{
    public class IndexModel : PageModel
    {
        private readonly Apetrei_Andrei_Lab2Context _context;

        public IndexModel(Apetrei_Andrei_Lab2Context context)
        {
            _context = context;
        }

        public IList<Category> Category { get; set; } = default!;

        public async Task OnGetAsync()
        {
            Category = await _context.Category
                .Include(c => c.BookCategories)
                    .ThenInclude(b => b.Book)
                .ToListAsync();
        }
    }
}