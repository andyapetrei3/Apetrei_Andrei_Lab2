using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Apetrei_Andrei_Lab2.Models;

namespace Apetrei_Andrei_Lab2.Pages.BorrowingPages;

public class CreateModel : PageModel
{
    private readonly Apetrei_Andrei_Lab2Context _context;

    public CreateModel(Apetrei_Andrei_Lab2Context context)
    {
        _context = context;
    }

    public IActionResult OnGet()
    {
        // PASUL 10: Modificăm listele pentru a afișa date lizibile (Titlu - Autor, Nume Membru)
        var bookList = _context.Book
            .Include(b => b.Author)
            .Select(x => new
            {
                x.ID,
                // Verificăm dacă autorul există pentru a evita erori (NullReferenceException)
                BookFullName = x.Title + " - " + (x.Author != null ? x.Author.LastName + " " + x.Author.FirstName : "Fără Autor")
            });

        ViewData["BookID"] = new SelectList(bookList, "ID", "BookFullName");
        ViewData["MemberID"] = new SelectList(_context.Member, "ID", "FullName");

        return Page();
    }

    [BindProperty]
    public Borrowing Borrowing { get; set; } = default!;

    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            // Dacă validarea eșuează, trebuie să recreăm listele derulante pentru a nu da eroare pagina
            var bookList = _context.Book
                .Include(b => b.Author)
                .Select(x => new
                {
                    x.ID,
                    BookFullName = x.Title + " - " + (x.Author != null ? x.Author.LastName + " " + x.Author.FirstName : "Fără Autor")
                });

            ViewData["BookID"] = new SelectList(bookList, "ID", "BookFullName", Borrowing.BookID);
            ViewData["MemberID"] = new SelectList(_context.Member, "ID", "FullName", Borrowing.MemberID);

            return Page();
        }

        _context.Borrowing.Add(Borrowing);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}