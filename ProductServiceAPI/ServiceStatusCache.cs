namespace ProductServiceAPI
{
    public class ServiceStatusCache
    {
        // Храним данные о статусах сервисов
        private Dictionary<int, bool> _serviceStatuses = new Dictionary<int, bool>();

        public void UpdateServiceStatus(int serviceId, bool isRunning)
        {
            _serviceStatuses[serviceId] = isRunning;
        }

        public bool? GetServiceStatus(int serviceId)
        {
            return _serviceStatuses.ContainsKey(serviceId) ? _serviceStatuses[serviceId] : (bool?)null;
        }

        public Dictionary<int, bool> GetAllStatuses()
        {
            return _serviceStatuses;
        }
    }
}
