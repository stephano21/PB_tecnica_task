namespace Commons
{
    public class ItemTaskDTO: CreateTask
    {
        public long Id { get; set; }
        public Status status { get; set; }
        public string UserId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

    }
    public class CreateTask
    {
        public string Title { get; set; }
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
