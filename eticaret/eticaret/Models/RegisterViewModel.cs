

    using System.ComponentModel.DataAnnotations;

    namespace eticaret.Models
    {
        public class RegisterViewModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "Şifre eşleşmiyor.")]
            public string ConfirmPassword { get; set; }
        }
    }


