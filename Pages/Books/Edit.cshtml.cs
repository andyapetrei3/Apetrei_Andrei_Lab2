using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Apetrei_Andrei_Lab2.Models;

namespace Apetrei_Andrei_Lab2.Pages.BookPages;

public class EditModel : PageModel
{
    private readonly Apetrei_Andrei_Lab2Context _context;

    public EditModel(Apetrei_Andrei_Lab2Context context)
    {
        _context = context;
    }

    [BindProperty]
    public Book Book { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var book = await _context.Book
            .Include(b => b.Publisher)
            .FirstOrDefaultAsync(m => m.ID == id);

        if (book is null)
        {
            return NotFound();
        }

        Book = book;
        ViewData["PublisherID"] = new SelectList(_context.Set<Publisher>(), "ID", "PublisherName", Book.PublisherID);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            ViewData["PublisherID"] = new SelectList(_context.Set<Publisher>(), "ID", "PublisherName", Book.PublisherID);
            return Page();
        }

        _context.Attach(Book).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!BookExists(Book.ID))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return RedirectToPage("./Index");
    }

    private bool BookExists(int id)
    {
        return _context.Book.Any(e => e.ID == id);
    }
}