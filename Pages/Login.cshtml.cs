using Microsoft.Win32;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN_Project.Models;
using PRN_Project.Utils;
using System.ComponentModel.DataAnnotations;

namespace PRN_Project.Pages
{
    public class LoginModel : PageModel
    {
        public readonly FinalProjectContext _context;
        public LoginModel(FinalProjectContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Credential Credential { get; set; } = null!;

        public bool isAuthenticationFailed = false;
        public async Task<IActionResult> OnPost()
        {
            var accout = await _context.Accounts.Include(x => x.Users).FirstOrDefaultAsync(x => x.Credential.Equals(Credential.Username));
            if (accout == null)
            {
                isAuthenticationFailed = true;
                return Page();
            }
            if (!PasswordEncoder.Match(Credential.Password, accout.Password))
            {
				isAuthenticationFailed = true;
				return Page();
			}
            isAuthenticationFailed = false;
            
            Response.Cookies.Append("token", $"{Hash.Compute(accout.Credential)}.{accout.AccountId}.{accout.Users.ToList()[0].UserId}");
            return RedirectToPage("/Posts/Index");
        }   
    }

    public class Credential
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
