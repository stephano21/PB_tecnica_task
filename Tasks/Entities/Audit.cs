namespace Tasks.Entities
{
    public class Audit
    {
        public bool Active { get; set; } = true;
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public string CreateUser { get; set; } = null;
        public string CreateIP { get; set; } = null;
        public DateTime? UpdateAt { get; set; }
        public string UpdateUser { get; set; } = null;
        public string UpdateIP { get; set; } = null;
        public DateTime? DeleteAt { get; set; }
        public string DeleteUser { get; set; } = null;
        public string DeleteIP { get; set; } = null;
        public void Register(string username, string ip)
        {
            Active = true;
            CreateAt = DateTime.UtcNow;
            CreateUser = username;
            CreateIP = ip;
        }
        public void Inactive(string username, string ip)
        {
            Active = false;
            DeleteAt = DateTime.UtcNow;
            DeleteUser = username;
            DeleteIP = ip;
        }
        public void Upgrade(string username, string ip)
        {
            Active = true;
            UpdateAt = DateTime.UtcNow;
            UpdateUser = username;
            UpdateIP = ip;
        }
    }
}
