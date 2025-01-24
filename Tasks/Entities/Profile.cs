using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Tasks.Entities
{
    [Table("Profile", Schema = "AUTH")]
    public class Profile
    {
        [Key]
        [ForeignKey("User")]
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual CustomUserIdentity User { get; set; }
        public virtual ICollection<TaskNotes> Tasks { get; set; }
        public string FullName() => $"{this.FirstName} {this.LastName}";
    }
}
