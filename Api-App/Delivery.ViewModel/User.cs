using System.ComponentModel.DataAnnotations;

namespace Delivery.ViewModel
{
    public class LoginViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        public string Name { get; set; }


        public string UserName { get { return Email; } }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }




        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        // Additional fields as needed
    }

}
