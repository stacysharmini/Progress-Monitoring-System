using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Progress_Monitoring_System.Data;
using Progress_Monitoring_System.Models;
using System.Data;

namespace Progress_Monitoring_System.Pages.User
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDBContext _context;
        public IndexModel(ApplicationDBContext context)
        {
            _context = context;
        }

        public List<UserModel> Users { get; set; }
        public void OnGet()
        {
            Users = _context.Users.ToList();
        }
    }
}
