using Microsoft.AspNetCore.Identity;

namespace Workhub.Domain.Entities
{
    public class GlobalUser : IdentityUser
    {
        public string Password { get; set; } = string.Empty;
    }
}
