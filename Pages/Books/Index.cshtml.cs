using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Apetrei_Andrei_Lab2.Models;
using System.Linq;

namespace Apetrei_Andrei_Lab2.Pages.BookPages
{
    public class IndexModel : PageModel
    {
        private readonly Apetrei_Andrei_Lab2Context _context;

        public IndexModel(Apetrei_Andrei_Lab2Context context)
        {
            _context = context;
        }

        public IList<Book> Book { get; set; } = default!;
        public string TitleSort { get; set; }
        public string AuthorSort { get; set; }
        public string CurrentFilter { get; set; }

        public async Task OnGetAsync(string sortOrder, string searchString)
        {
            CurrentFilter = searchString;
            TitleSort = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            AuthorSort = sortOrder == "author" ? "author_desc" : "author";

            // 1. Preluăm datele (fără nicio filtrare sau sortare complexă aici)
            var booksQuery = _context.Book
                .Include(b => b.Publisher)
                .Include(b => b.Author)
                .Include(b => b.BookCategories)
                    .ThenInclude(b => b.Category)
                .AsNoTracking();

            // 2. Aducem toate datele în memorie
            var booksList = await booksQuery.ToListAsync();

            // 3. Aplicăm căutarea ÎN MEMORIE (C#), ca să nu mai dea eroare SQL
            if (!String.IsNullOrEmpty(searchString))
            {
                booksList = booksList.Where(s =>
                    (s.Title != null && s.Title.Contains(searchString, StringComparison.OrdinalIgnoreCase)) ||
                    (s.Author != null && s.Author.FullName != null && s.Author.FullName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                ).ToList();
            }

            // 4. Aplicăm sortarea ÎN MEMORIE (C#)
            switch (sortOrder)
            {
                case "title_desc":
                    booksList = booksList.OrderByDescending(s => s.Title).ToList();
                    break;
                case "author_desc":
                    booksList = booksList.OrderByDescending(s => s.Author != null ? s.Author.FullName : "").ToList();
                    break;
                case "author":
                    booksList = booksList.OrderBy(s => s.Author != null ? s.Author.FullName : "").ToList();
                    break;
                default:
                    booksList = booksList.OrderBy(s => s.Title).ToList();
                    break;
            }

            // 5. Salvăm rezultatul final
            Book = booksList;
        }
    }
}