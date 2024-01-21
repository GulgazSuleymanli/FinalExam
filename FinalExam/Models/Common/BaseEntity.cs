using Microsoft.AspNetCore.Components.Web;

namespace FinalExam.Models.Common
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
    }
}
