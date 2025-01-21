using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Progress_Monitoring_System.Models
{
    public class UATFATModel
    {
        [Key]
        public string UatFatID { get; set; }           
        public string UatFatName { get; set; }         
        public string UatFatDescription { get; set; }  
        public int Status { get; set; }         
        public string UatFatType { get; set; }        
        public string CreatedBy { get; set; }
       
        public DateTime ? CreatedDateTime { get; set; }
        public DateTime ?  ApprovedDate { get; set; }
        public string ? ApprovedBy { get; set; }

    }
}
