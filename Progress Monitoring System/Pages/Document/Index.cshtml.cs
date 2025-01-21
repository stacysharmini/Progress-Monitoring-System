using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Progress_Monitoring_System.Data;
using Progress_Monitoring_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Progress_Monitoring_System.Pages.Document
{
    public class IndexModel(ApplicationDBContext context, IEmailSender emailSender) : PageModel
    {
        private readonly ApplicationDBContext _context = context;
        private readonly IEmailSender _emailSender = emailSender;
        public string UserID { get; set; }
        public string UserFullName { get; set; }

        public IList<UATFATModel> UATFATS { get; set; }

        [BindProperty]
        public string UatFatName { get; set; }

        [BindProperty]
        public string UatFatDescription { get; set; }

        [BindProperty]
        public string UatFatType { get; set; }

        public async Task OnGetAsync()
        {
            UATFATS = await _context.UATFAT.ToListAsync();
        }
        public IActionResult DownloadFile(string filePath)
        {
            var fileBytes = System.IO.File.ReadAllBytes(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files", filePath)); // Adjust the path accordingly
            var fileName = Path.GetFileName(filePath);

            return File(fileBytes, "application/octet-stream", fileName);
        }
        // creating a new task
        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(UatFatName) || string.IsNullOrEmpty(UatFatDescription) || string.IsNullOrEmpty(UatFatType))
            {
                return new JsonResult(new { success = false, message = "All fields are required." });
            }

            try
            {

                UserID = HttpContext.Session.GetString("UserID");
                UserFullName = HttpContext.Session.GetString("UserFullName");
                var newTask = new UATFATModel
                {
                    UatFatID = "F" + (await _context.UATFAT.CountAsync() + 1).ToString("D4"),  
                    UatFatName = UatFatName,
                    UatFatDescription = UatFatDescription,
                    UatFatType = UatFatType,
                    CreatedBy = HttpContext.Session.GetString("usermail"),
                    CreatedDateTime = DateTime.Now,
                    Status = 1 
                };
                var usersWithRole = await _context.Users
            .Where(u => u.RoleID == 2)
            .ToListAsync();

                _context.UATFAT.Add(newTask);
                await _context.SaveChangesAsync();
                string subject = "New Task or Module added successfully";
                string body = "<html><body>New Task has been Created!</body></html>";

                foreach (var user in usersWithRole)
                {
                    string toEmail = user.UserEmail;
                    await _emailSender.SendEmailAsync(toEmail, subject, body);
                }
                return new JsonResult(new { success = true, message = "Task created successfully!" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = $"Error creating task: {ex.Message}" });
            }
        }
    }

}
