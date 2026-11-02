using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Apetrei_Andrei_Lab2.Models;

namespace Apetrei_Andrei_Lab2.Pages.MemberPages
{
    public class DetailsModel : PageModel
    {
        private readonly Apetrei_Andrei_Lab2Context _context;

        public DetailsModel(Apetrei_Andrei_Lab2Context context)
        {
            _context = context;
        }

        public Member Member { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Member == null)
            {
                return NotFound();
            }

            // AICI AM ADĂUGAT INCLUDE-URILE PENTRU ÎMPRUMUTURI
            var member = await _context.Member
                .Include(m => m.Borrowings)
                    .ThenInclude(b => b.Book)
                        .ThenInclude(b => b.Author)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (member == null)
            {
                return NotFound();
            }
            else
            {
                Member = member;
            }

            return Page();
        }
    }
}