using System.ComponentModel.DataAnnotations;

namespace FinalExam.Areas.Manage.ViewModels.Account
{
    public class LoginVM
    {
        public string UsernameOrEmail { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
