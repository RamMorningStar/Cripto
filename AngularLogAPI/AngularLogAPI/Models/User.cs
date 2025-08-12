using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AngularLogAPI.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        public string username { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string token { get; set; }
    }
}
