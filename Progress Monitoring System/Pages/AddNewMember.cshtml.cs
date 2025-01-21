using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity; 
using Microsoft.EntityFrameworkCore;
using Progress_Monitoring_System.Data;
using Progress_Monitoring_System.Models;

namespace Progress_Monitoring_System.Pages
{
    public class AddNewMemberModel : PageModel
    {
        private readonly ApplicationDBContext _context;
        private readonly PasswordHasher<UserModel> _passwordHasher;

        public AddNewMemberModel(ApplicationDBContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<UserModel>();
        }

        [BindProperty]
        public List<UserRoleModel> UserRoles { get; set; }

        [BindProperty]
        public UserModel NewMember { get; set; }

        public async Task OnGetAsync()
        {
            
            UserRoles = await _context.Role.ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
           
           
            var hashedPassword = _passwordHasher.HashPassword(NewMember, NewMember.Password);
            NewMember.Password = hashedPassword;

            NewMember.CreatedDateTime = DateTime.Now;
            NewMember.UserEmail = NewMember.UserEmail ; 
            NewMember.UserFullName = NewMember.UserFullName ; 
            NewMember.RoleID = NewMember.RoleID ; 

     
            _context.Users.Add(NewMember);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Index");
        }
    }
}
