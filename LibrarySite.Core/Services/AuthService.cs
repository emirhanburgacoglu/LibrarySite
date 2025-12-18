using System.Collections.Generic;
using System.Linq;
using LibrarySite.Core.Domain;

namespace LibrarySite.Core.Services
{
    public class AuthService
    {
        // Şimdilik demo kullanıcılar. Sonra DB'den gelecek.
        private readonly List<User> _users = new()
        {
            new User { UserId = 1, Email = "member@uni.edu", Password = "Cemira.1901", Role = UserRole.Member },
            new User { UserId = 2, Email = "admin@uni.edu",  Password = "1234", Role = UserRole.Admin }
        };

        public User? ValidateUser(string email, string password)
        {
            return _users.FirstOrDefault(u =>
                u.Email.ToLower() == email.ToLower() &&
                u.Password == password);
        }
    }
}
