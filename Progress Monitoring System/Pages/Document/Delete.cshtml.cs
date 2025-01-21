using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Progress_Monitoring_System.Data;
using Progress_Monitoring_System.Models;
using System.Linq;

namespace Progress_Monitoring_System.Pages.Document
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDBContext _context;

        public DeleteModel(ApplicationDBContext context)
        {
            _context = context;
        }

        [BindProperty]
        public UATFATModel UatFat { get; set; }

        public bool IsReferenced { get; set; } = false;  

        public IActionResult OnGet(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            UatFat = _context.UATFAT.Find(id);

            if (UatFat == null)
            {
                return NotFound();
            }

           
            IsReferenced = _context.UploadVersion.Any(u => u.UATFATID == id);

            return Page();
        }

        public IActionResult OnPost(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            UatFat = _context.UATFAT.Find(id);

            if (UatFat == null)
            {
                return NotFound();
            }

          
            IsReferenced = _context.UploadVersion.Any(u => u.UATFATID == id);

            if (IsReferenced)
            {
                ModelState.AddModelError(string.Empty, "This task cannot be deleted because it is referenced in the UploadVersion.");
                return Page(); 
            }

          
            _context.UATFAT.Remove(UatFat);
            _context.SaveChanges();

            return RedirectToPage("/Document/Index");
        }
    }
}
