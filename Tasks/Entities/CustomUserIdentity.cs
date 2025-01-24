using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Tasks.Entities
{
    public class CustomUserIdentity : IdentityUser
    {
        public bool PasswordExpired { get; set; } = false;
        [Required]
        public Profile Profile { get; set; }
    }
}
