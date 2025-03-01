using System.Text.Json.Serialization;

namespace LearnAPI.Model
{
    public class UserModel
    {
        public int Id { get; set; } // Primary Key
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
       // public string PasswordHash { get; set; } = string.Empty;
        [JsonPropertyName("password")]
        public string PasswordHash { get; set; } // Ensure correct mapping
        public string Role { get; set; } = "Customer"; // Default role
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true; // Active by default
    }
}
