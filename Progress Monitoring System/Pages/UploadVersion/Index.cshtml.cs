using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Progress_Monitoring_System.Data;
using Progress_Monitoring_System.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Progress_Monitoring_System.Pages.UploadVersions
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDBContext _context;

        public IndexModel(ApplicationDBContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string UatFatID { get; set; }

        [BindProperty]
        public string VersionID { get; set; }

        [BindProperty]
        public string ModuleName { get; set; }

        [BindProperty]
        public string UploadedBy { get; set; }

        [BindProperty]
        public IFormFile File { get; set; }

        public string usermail { get; set; }
        public string UserFullName { get; set; }
        public List<UploadVersionModel> UploadVersions { get; set; }
        public string UATFATID { get; set; }

     
        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            UATFATID = id;
            UploadVersions = await _context.UploadVersion
                .Where(m => m.UATFATID == id)
                .ToListAsync();

            foreach (var version in UploadVersions)
            {
                if (version.FilePath != null && version.FilePath.StartsWith("wwwroot\\uploads\\"))
                {
                    version.FilePath = version.FilePath.Substring("wwwroot\\uploads\\".Length);
                }
            }

            return Page();
        }


        public async Task<IActionResult> OnPostAsync(string VersionID, string UATFATID, IFormFile File, string ActionType)
        {
            try
            {
                // Handle Upload Version
                if (ActionType == "Upload" && File != null && File.Length > 0)
                {
                    var filePath = Path.Combine("wwwroot", "uploads", File.FileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await File.CopyToAsync(stream);
                    }

                    string usermail = HttpContext.Session.GetString("usermail");
                    string UserFullName = HttpContext.Session.GetString("UserFullName");

                    var uploadVersion = new UploadVersionModel
                    {
                        UATFATID = UATFATID,
                        VersionID = VersionID,
                        ModuleName = "ModuleName", 
                        UploadedBy = usermail,
                        FilePath = filePath,
                        UploadedDate = DateTime.Now,
                        Status = 0
                    };

                    _context.UploadVersion.Add(uploadVersion);
                    await _context.SaveChangesAsync();

                    return new JsonResult(new { success = true });
                }

                // Handle Delete Version
                if (ActionType == "Delete" && !string.IsNullOrEmpty(VersionID))
                {
                    var versionToDelete = await _context.UploadVersion
                                                         .Where(v => v.VersionID == VersionID)
                                                         .FirstOrDefaultAsync();

                    if (versionToDelete != null)
                    {
                        _context.UploadVersion.Remove(versionToDelete);
                        await _context.SaveChangesAsync();
                        return RedirectToPage("/UploadVersion/Index", new { id = UATFATID, success = "true" });
                    }
                    else
                    {
                        return new JsonResult(new { success = false, message = "Version not found." });
                    }
                }

                return new JsonResult(new { success = false, message = "Invalid request." });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new JsonResult(new { success = false, message = "An error occurred while processing the request." });
            }
        }

    }
}

public class UploadVersionForm
{
    public string UatFatID { get; set; }
    public string VersionID { get; set; }
    public string ModuleName { get; set; }
    public string UploadedBy { get; set; }
    public IFormFile File { get; set; }
}
