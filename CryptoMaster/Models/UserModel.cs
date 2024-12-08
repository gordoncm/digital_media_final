using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoMaster.Models
{
    [Table("Users")]
    public class UserModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public bool IsLoggedIn { get; set; }

        public string BSCAddress { get; set; }
    }
}
