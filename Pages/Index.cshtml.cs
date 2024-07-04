using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN_Project.Filters;
using PRN_Project.Models;

namespace PRN_Project.Pages
{
    [AuthenticationFilter]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly FinalProjectContext _context;

        public IndexModel(ILogger<IndexModel> logger, FinalProjectContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> OnGet()
        {
            return Page();
        }
    }
}
