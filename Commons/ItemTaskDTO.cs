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
        Nuevo = 0,
        Pendiente = 1,
        EnProceso = 2,
        Terminado = 3
    }
}
