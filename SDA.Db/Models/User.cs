using Microsoft.AspNetCore.Identity;

namespace SDA.Db.Models
{
    public class User: IdentityUser
    {
        public DateTime RegistrationDateTime { get; set; }
    }
}
