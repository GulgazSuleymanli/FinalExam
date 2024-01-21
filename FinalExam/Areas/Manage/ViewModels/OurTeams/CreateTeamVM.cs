using System.ComponentModel.DataAnnotations;

namespace FinalExam.Areas.Manage.ViewModels.OurTeams
{
    public class CreateTeamVM
    {
        [MaxLength(64)]
        public string FullName { get; set; }
        [MaxLength(32)]
        public string Position { get; set; }
        [MaxLength(100)]
        public string Description { get; set; }
        public IFormFile Image { get; set; }
        public string? Facebook { get; set; }
        public string? Instagram { get; set; }
        public string? Twitter { get; set; }
        public string? Linkedin { get; set; }
    }
}
