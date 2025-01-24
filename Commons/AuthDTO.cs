using System.ComponentModel.DataAnnotations;

namespace Commons
{
    public class AuthDTO
    {
    }
    public class LoginDTO
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
    public class RegisterDTO:UserDTO
    {
        [Required]
        public string Password { get; set; }
    }
    public class UserDTO
    {
        [Required]
        public string UserName { get; set; }
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        public string Role { get; set; }="User";
        public ProfileDTO Profile { get; set; } = new ProfileDTO ();
    }
    public class ProfileDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }
    }
    public class LoggedUser
    {
        public TokensDTO Auth { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
        public DateTime Expiracion { get; set; }
        public string Env { get; set; }
    }
    public class TokensDTO
    {
        public string Access_Token { get; set; }
        public string Refresh_Token { get; set; }
    }
    public class ProfileViewDTO: ProfileDTO
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Couple { get; set; }
    }
}
