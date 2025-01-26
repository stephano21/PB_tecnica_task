using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Commons
{
    public class TaskDTO
    {
        public List<ItemTaskDTO> Items { get; set; }= new List<ItemTaskDTO>();
    }
    public class ItemTaskDTO: CreateTask
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }
        [JsonPropertyName("status")]
        public Status status { get; set; }
        [JsonPropertyName("userId")]
        public string UserId { get; set; }
        [JsonPropertyName("createDate")]
        public DateTime CreateDate { get; set; }
        [JsonPropertyName("startDate")]
        public DateTime? StartDate { get; set; }
        [JsonPropertyName("endDate")]
        public DateTime? EndDate { get; set; }

    }
    public class CreateTask
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
    public enum Status
    {
        [Description("Nuevo")]
        Nuevo = 0,
        [Description("Pendiente")]
        Pendiente = 1,
        [Description("En Proceso")]
        EnProceso = 2,
        [Description("Terminado")]
        Terminado = 3

    }
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = field.GetCustomAttributes(typeof(DescriptionAttribute), false)
                                 .FirstOrDefault() as DescriptionAttribute;
            return attribute == null ? value.ToString() : attribute.Description;
        }
    }
}
