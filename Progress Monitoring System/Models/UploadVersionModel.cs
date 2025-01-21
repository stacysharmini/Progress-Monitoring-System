using System.ComponentModel.DataAnnotations;

namespace Progress_Monitoring_System.Models;

public class UploadVersionModel
{
    [Key]
    public int nID  {get; set;}
    public string VersionID { get; set; }
    public string UATFATID { get; set; } 

    public string ModuleName { get; set; }
    public string UploadedBy { get; set; }
    public DateTime UploadedDate { get; set; }
    public string ? ApprovedBy { get; set; }
    public DateTime ? ApprovedDate { get; set; }
    public string ? FilePath { get; set; }

    public int Status { get; set; }
}
