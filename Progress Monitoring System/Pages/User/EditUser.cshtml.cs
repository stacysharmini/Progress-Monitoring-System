using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Progress_Monitoring_System.Data;
using Progress_Monitoring_System.Models; 

namespace Progress_Monitoring_System.Pages.User
{
    public class EditUserModel : PageModel
    {
        private readonly ApplicationDBContext _context;

        public EditUserModel(ApplicationDBContext context)
        {
            _context = context;
        }

        [BindProperty]
        public UserModel User { get; set; }
        public List<SelectListItem> Roles { get; set; }
        public int UserID { get; set; }
        public IActionResult OnGet(int id)
        {
           
            User = _context.Users.FirstOrDefault(u => u.UserID == id);

            Roles = new List<SelectListItem>
        {
            new SelectListItem { Value = "1", Text = "Admin" },
            new SelectListItem { Value = "2", Text = "Project Manager" },
            new SelectListItem { Value = "3", Text = "System Analyst" }
        };
            if (User == null)
            {
                return RedirectToPage("/UserList");
            }
            UserID = User.UserID;
            return Page(); 
        }


        public IActionResult OnPost(int id)
        {
           

           
            var userToUpdate = _context.Users.FirstOrDefault(u => u.UserID == id);

            if (userToUpdate != null)
            {
              
                userToUpdate.UserFullName = User.UserFullName;
                userToUpdate.UserEmail = User.UserEmail;
                userToUpdate.RoleID = User.RoleID;

                
                _context.SaveChanges();

               
                TempData["SuccessMessage"] = "User details updated successfully!";
            }
            else
            {
               
                ModelState.AddModelError("", "User not found.");
                return Page();
            }

           
            return RedirectToPage("/UserList");
        }

    }
}
