using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Progress_Monitoring_System.Models
{
    public class UserModel
    {
        [Key]
        public int UserID { get; set; }
        public string Username { get; set; }
        public int RoleID { get; set; }
        public string UserFullName { get; set; }
        public string UserEmail { get; set; }
        public DateTime CreatedDateTime { get; set; }

        public string Password { get; set; }

    }
}
