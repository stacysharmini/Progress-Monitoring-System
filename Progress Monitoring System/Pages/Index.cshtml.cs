using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Progress_Monitoring_System.Data;
using Progress_Monitoring_System.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Progress_Monitoring_System.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDBContext _context;

        public IndexModel(ApplicationDBContext context)
        {
            _context = context;
        }

        public int CompletedTasks { get; set; }
        public int TotalTasks { get; set; }
        public double CompletionPercentage { get; set; }
        public int ProjectCount { get; set; }
        public double[] MonthlyProgressData { get;  set; }
        public List<TaskTypeData> TaskTypeCounts { get; set; }
        public void OnGet()
        {
          
            var taskCounts = _context.UATFAT
                .GroupBy(task => 1)
                .Select(group => new
                {
                    CompletedTasks = group.Count(task => task.Status == 1),
                    TotalTasks = group.Count(),
                    ProjectCount = _context.UATFAT.Count() 
                })
                .FirstOrDefault();

            CompletedTasks = taskCounts?.CompletedTasks ?? 0;
            TotalTasks = taskCounts?.TotalTasks ?? 0;
            ProjectCount = taskCounts?.ProjectCount ?? 0;

          
            if (TotalTasks > 0)
            {
                CompletionPercentage = (double)CompletedTasks / TotalTasks * 100;
            }
            else
            {
                CompletionPercentage = 0;  
            }

            var monthlyProgress = _context.UATFAT
      .Where(task => task.Status == 1 && task.ApprovedDate.HasValue) // Ensure ApprovedDate is not null
      .GroupBy(task => task.ApprovedDate.Value.Month)  // Safely access Month using Value
      .Select(group => new
      {
          Month = group.Key,
          CompletedPercentage = (double)group.Count() / _context.UATFAT.Count() * 100
      })
      .OrderBy(month => month.Month)
      .ToList();


            var months = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            var progressData = new double[12];  

           
            foreach (var progress in monthlyProgress)
            {
              
                if (progress.Month >= 1 && progress.Month <= 12)
                {
                    progressData[progress.Month - 1] = progress.CompletedPercentage;
                }
            }

            MonthlyProgressData = progressData;

            //
            TaskTypeCounts = _context.UATFAT
        .GroupBy(task => task.UatFatType) 
        .Select(group => new TaskTypeData
        {
            TaskType = group.Key,  
            UAT = group.Count(task => task.UatFatType == "UAT"), 
            FAT = group.Count(task => task.UatFatType == "FAT"), 
            Requirement = group.Count(task => task.UatFatType == "Requirement") 
        })
        .ToList();

        }


    }

}
public class TaskTypeData
{
    public string TaskType { get; set; }
    public int UAT { get; set; }
    public int FAT { get; set; }
    public int Requirement { get; set; }
}