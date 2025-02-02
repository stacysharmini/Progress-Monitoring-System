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
            UploadVersions = await FetchUploadVersionsAsync(id);

            foreach (var version in UploadVersions)
            {
                if (!string.IsNullOrEmpty(version.FilePath) && version.FilePath.StartsWith("wwwroot\\uploads\\"))
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
                if (ActionType == "Upload" && File != null && File.Length > 0)
                {
                    return await HandleFileUploadAsync(UATFATID, VersionID, File);
                }

                if (ActionType == "Delete" && !string.IsNullOrEmpty(VersionID))
                {
                    return await HandleFileDeletionAsync(VersionID, UATFATID);
                }

                return new JsonResult(new { success = false, message = "Invalid request." });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new JsonResult(new { success = false, message = "An error occurred while processing the request." });
            }
        }

        private async Task<List<UploadVersionModel>> FetchUploadVersionsAsync(string id)
        {
            return await _context.UploadVersion
                .Where(m => m.UATFATID == id)
                .ToListAsync();
        }

        private async Task<IActionResult> HandleFileUploadAsync(string uatFatID, string versionID, IFormFile file)
        {
            string filePath = SaveFile(file);
            string userMail = HttpContext.Session.GetString("usermail");
            string userFullName = HttpContext.Session.GetString("UserFullName");

            var uploadVersion = new UploadVersionModel
            {
                UATFATID = uatFatID,
                VersionID = versionID,
                ModuleName = ModuleName,
                UploadedBy = userMail,
                FilePath = filePath,
                UploadedDate = DateTime.Now,
                Status = 0
            };

            await SaveUploadVersionAsync(uploadVersion);

            return new JsonResult(new { success = true });
        }

        private async Task<IActionResult> HandleFileDeletionAsync(string versionID, string uatFatID)
        {
            var versionToDelete = await _context.UploadVersion
                                               .FirstOrDefaultAsync(v => v.VersionID == versionID);

            if (versionToDelete != null)
            {
                _context.UploadVersion.Remove(versionToDelete);
                await _context.SaveChangesAsync();
                return RedirectToPage("/UploadVersion/Index", new { id = uatFatID, success = "true" });
            }

            return new JsonResult(new { success = false, message = "Version not found." });
        }

        private string SaveFile(IFormFile file)
        {
            var filePath = Path.Combine("wwwroot", "uploads", file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return filePath;
        }

        private async Task SaveUploadVersionAsync(UploadVersionModel uploadVersion)
        {
            _context.UploadVersion.Add(uploadVersion);
            await _context.SaveChangesAsync();
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
