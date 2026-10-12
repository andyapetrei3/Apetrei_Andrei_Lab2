using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Apetrei_Andrei_Lab2.Models;

namespace Apetrei_Andrei_Lab2.Pages.BookPages;

public class CreateModel : PageModel
{
    private readonly Apetrei_Andrei_Lab2Context _context;

    public CreateModel(Apetrei_Andrei_Lab2Context context)
    {
        _context = context;
    }

    public IActionResult OnGet()
    {
        ViewData["PublisherID"] = new SelectList(_context.Set<Publisher>(), "ID", "PublisherName");
        ViewData["AuthorID"] = new SelectList(_context.Set<Author>(), "ID", "LastName");
        return Page();
    }

    [BindProperty]
    public Book Book { get; set; } = default!;

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            ViewData["PublisherID"] = new SelectList(_context.Set<Publisher>(), "ID", "PublisherName");
            ViewData["AuthorID"] = new SelectList(_context.Set<Author>(), "ID", "LastName");
            return Page();
        }

        _context.Book.Add(Book);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}