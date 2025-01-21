using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using Progress_Monitoring_System.Data;
using Progress_Monitoring_System.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Progress_Monitoring_System.Pages
{
    public class ApprovedStatusModel : PageModel
    {
        private readonly ApplicationDBContext _context;
        private readonly IEmailSender _emailSender;

        public ApprovedStatusModel(ApplicationDBContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }

        public IQueryable<UploadVersionModel> UATFATRecords { get; set; }

        public async Task OnGetAsync()
        {
           
            UATFATRecords = _context.UploadVersion;
        }
        [HttpPost]
        public async Task<IActionResult> OnPostApproveOrRejectAsync(int nID, string UploadedBy, string action)
        {
            var task = await _context.UploadVersion.FindAsync(nID);
            if (task == null)
            {
                return NotFound();
            }

          
            if (action == "Approve")
            {
                task.Status = 1;  
                task.ApprovedBy = HttpContext.Session.GetString("usermail");
                task.ApprovedDate = DateTime.Now;

               
                string subject = "Task Approved";
                string body = "<html><body>Your task has been approved!</body></html>";
                await _emailSender.SendEmailAsync(UploadedBy, subject, body);
            }
            else if (action == "Reject")
            {
                task.Status = 2; 
                task.ApprovedBy = HttpContext.Session.GetString("usermail");
                task.ApprovedDate = DateTime.Now;

                
                string subject = "Task Rejected";
                string body = "<html><body>Your task has been rejected!</body></html>";
                await _emailSender.SendEmailAsync(UploadedBy, subject, body);
            }

            await _context.SaveChangesAsync();

          
            return RedirectToPage();
        }

    }
}
