using Microsoft.AspNetCore.Mvc.RazorPages;
using Progress_Monitoring_System.Data;
using Progress_Monitoring_System.Models;
using System.Collections.Generic;
using System.Linq;

namespace Progress_Monitoring_System.Pages
{
    public class UserListModel : PageModel
    {
        private readonly ApplicationDBContext _context;

        public UserListModel(ApplicationDBContext context)
        {
            _context = context;
        }

        public List<UserModel> UserList { get; set; }

        public void OnGet()
        {
            // Fetch all users from the Users table
            UserList = _context.Users.ToList();
        }
    }
}
