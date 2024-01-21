using Microsoft.Build.Execution;
using System.ComponentModel.DataAnnotations;

namespace FinalExam.Areas.Manage.ViewModels.Account
{
    public class RegisterVM
    {
        [MinLength(3)]
        [MaxLength(32)]
        public string Name { get; set; }
        [MinLength(3)]
        [MaxLength(64)]
        public string Surname { get; set; }
        [MaxLength(100)]
        public string Username { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password), Compare("Password")]
        public string ComfirmPassword { get; set; }
    }
}
