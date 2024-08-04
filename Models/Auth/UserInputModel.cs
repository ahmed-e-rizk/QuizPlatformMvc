using System.ComponentModel.DataAnnotations;

namespace QuizPlatform.Models.Auth
{
    public class UserInputModel
    {
        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 8)]
        public string Password { get; set; }

        [Required]
        public string Mobile { get; set; }
    }
}
