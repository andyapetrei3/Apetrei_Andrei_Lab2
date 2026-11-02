using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering; // Necesar pentru SelectList
using Microsoft.EntityFrameworkCore;
using Apetrei_Andrei_Lab2.Models;

namespace Apetrei_Andrei_Lab2.Pages.BorrowingPages;

public class EditModel : PageModel
{
    private readonly Apetrei_Andrei_Lab2Context _context;

    public EditModel(Apetrei_Andrei_Lab2Context context)
    {
        _context = context;
    }

    [BindProperty]
    public Borrowing Borrowing { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var borrowing = await _context.Borrowing.FirstOrDefaultAsync(m => m.ID == id);
        if (borrowing is null)
        {
            return NotFound();
        }

        Borrowing = borrowing;

        // AICI AM ADĂUGAT MODIFICAREA PENTRU LISTELE DERULANTE
        var bookList = _context.Book
            .Include(b => b.Author)
            .Select(x => new
            {
                x.ID,
                BookFullName = x.Title + " - " + (x.Author != null ? x.Author.LastName + " " + x.Author.FirstName : "Fără Autor")
            });

        ViewData["BookID"] = new SelectList(bookList, "ID", "BookFullName", borrowing.BookID);
        ViewData["MemberID"] = new SelectList(_context.Member, "ID", "FullName", borrowing.MemberID);

        return Page();
    }

    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            // Dacă validarea eșuează, trebuie să recreăm listele pentru a nu da eroare pagina
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

        _context.Attach(Borrowing).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!BorrowingExists(Borrowing.ID))
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

    private bool BorrowingExists(int id)
    {
        return _context.Borrowing.Any(e => e.ID == id);
    }
}