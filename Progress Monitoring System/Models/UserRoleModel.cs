using System.ComponentModel.DataAnnotations;

namespace Progress_Monitoring_System.Models
{
    public class UserRoleModel
    {
        [Key]
        public int RoleID { get; set; }
        public string RoleName { get; set; }

    }
}
