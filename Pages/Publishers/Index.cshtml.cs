using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Apetrei_Andrei_Lab2.Models;

namespace Apetrei_Andrei_Lab2.Pages.Publishers
{
    public class IndexModel : PageModel
    {
        private readonly Apetrei_Andrei_Lab2Context _context;

        public IndexModel(Apetrei_Andrei_Lab2Context context)
        {
            _context = context;
        }

        public IList<Publisher> Publisher { get; set; } = default!;

        public async Task OnGetAsync()
        {
            Publisher = await _context.Publisher
                .Include(p => p.Books)
                .ToListAsync();
        }
    }
}