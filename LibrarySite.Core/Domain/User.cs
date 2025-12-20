namespace LibrarySite.Core.Domain
{
    public enum UserRole
    {
        Member = 0,
        Admin = 1
    }

    public class User
    {
        public int UserId { get; set; }

        public string Email { get; set; } = string.Empty;

        
        public string Password { get; set; } = string.Empty;

        public UserRole Role { get; set; }
    }
}
