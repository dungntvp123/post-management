using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PRN_Project.Models;
using PRN_Project.Utils;

namespace PRN_Project.Pages.Accounts
{
    public class RegisterModel : PageModel
    {
        private readonly PRN_Project.Models.FinalProjectContext _context;

        public RegisterModel(PRN_Project.Models.FinalProjectContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public RegisterForm RegisterForm { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            Account account = new Account()
            {
                Credential = RegisterForm.Username,
                Password = PasswordEncoder.Encode(RegisterForm.Password),
            };


            User user = new User()
            {
                FirstName = RegisterForm.FirstName,
                LastName = RegisterForm.LastName,
                Account = account,
                CreateDate = DateTime.Now,
                NickName = string.IsNullOrEmpty(RegisterForm.NickName) ? $"{RegisterForm.FirstName} {RegisterForm.LastName}" : RegisterForm.NickName,
                
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            var accout = await _context.Accounts.Include(x => x.Users).FirstOrDefaultAsync(x => x.Credential.Equals(RegisterForm.Username));
            Response.Cookies.Append("token", $"{Hash.Compute(accout.Credential)}.{accout.AccountId}.{accout.Users.ToList()[0].UserId}");
            return RedirectToPage("/Posts/Index");
        }
    }

    public class RegisterForm
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string NickName { get; set; } 
    }
}
