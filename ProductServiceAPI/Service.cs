namespace ProductServiceAPI
{
    public class Service
    {
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string ServiceStatusName { get; set; }

        public bool IsServiceRunning { get; set; } 
    }
}
