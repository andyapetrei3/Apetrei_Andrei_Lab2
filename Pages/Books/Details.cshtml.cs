using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Apetrei_Andrei_Lab2.Models;

namespace Apetrei_Andrei_Lab2.Pages.BookPages;

public class DetailsModel : PageModel
{
    private readonly Apetrei_Andrei_Lab2Context _context;
    public DetailsModel(Apetrei_Andrei_Lab2Context context)
    {
        _context = context;
    }

    public Book Book { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var book = await _context.Book.FirstOrDefaultAsync(m => m.ID == id);
        if (book is null)
        {
            return NotFound();
        }
        else
        {
            Book = book;
        }

        return Page();
    }
}
