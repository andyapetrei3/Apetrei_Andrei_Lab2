using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Apetrei_Andrei_Lab2.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Apetrei_Andrei_Lab2.Pages.BorrowingPages;

public class IndexModel : PageModel
{
    private readonly Apetrei_Andrei_Lab2Context _context;

    public IndexModel(Apetrei_Andrei_Lab2Context context)
    {
        _context = context;
    }

    public IList<Borrowing> Borrowing { get; set; } = default!;

    public async Task OnGetAsync()
    {
        if (_context.Borrowing != null)
        {
            // AICI ESTE MODIFICAREA (Pasul 11)
            // Includem Book, apoi Author-ul cărții, și includem și Member-ul
            Borrowing = await _context.Borrowing
                .Include(b => b.Book)
                    .ThenInclude(b => b.Author)
                .Include(b => b.Member)
                .ToListAsync();
        }
    }
}