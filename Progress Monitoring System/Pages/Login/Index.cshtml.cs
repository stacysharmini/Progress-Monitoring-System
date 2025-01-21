using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Progress_Monitoring_System.Data;
using Progress_Monitoring_System.Models;
using System.ComponentModel.DataAnnotations;

namespace Progress_Monitoring_System.Pages.Login
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDBContext _context;
        private readonly PasswordHasher<UserModel> _passwordHasher;

        public IndexModel(ApplicationDBContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<UserModel>();
        }

        [BindProperty]
        public required UserModel LoginModel { get; set; }

        public bool LoginFailed { get; set; } = false;
        public IActionResult OnGet()
        
        {
           
            HttpContext.Session.Clear();


            return Page();
        }
        public IActionResult OnPost()
        {
           
            var user = _context.Users.FirstOrDefault(u => u.Username == LoginModel.Username);

            if (user != null)
            {
               
                var result = _passwordHasher.VerifyHashedPassword(user, user.Password, LoginModel.Password);

                
                if (result == PasswordVerificationResult.Success)
                {
                    HttpContext.Session.SetString("UserID", user.Username);
                    HttpContext.Session.SetString("usermail", user.UserEmail);
                    HttpContext.Session.SetString("UserFullName", user.UserFullName);
                    HttpContext.Session.SetString("RoleID", user.RoleID.ToString());
                    return RedirectToPage("/Index");
                }
            }

           
            LoginFailed = true;
            return Page();
        }

    }
}
