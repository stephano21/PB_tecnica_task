using static System.Runtime.InteropServices.JavaScript.JSType;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Commons;

namespace Tasks.Entities
{
    [Table("TaskNotes", Schema = "CAT")]
    public class TaskNotes : Audit
    {
        [Key]
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        [ForeignKey("Profile")]
        public string UserId { get; set; }
        public Status Status { get; set; } = Status.Nuevo;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        //[JsonIgnore]
        public virtual Profile Profile { get; set; }

        public void UpdateTask(UpdateTask data)
        {
            Title = data.Title;
            Description = data.Description;
        }
    }
   
}
