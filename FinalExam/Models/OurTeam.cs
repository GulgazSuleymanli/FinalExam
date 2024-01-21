using FinalExam.Models.Common;

namespace FinalExam.Models
{
    public class OurTeam:BaseEntity
    {
        public string FullName { get; set; }
        public string Position { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string? Facebook { get; set; }
        public string? Instagram { get; set; }
        public string? Twitter { get; set; }
        public string? Linkedin { get; set; }
    }
}
