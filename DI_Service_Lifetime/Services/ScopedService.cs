namespace DI_Service_Lifetime.Services
{
    public class ScopedService : IScopedService
    {
        private readonly Guid _guid;

        public ScopedService()
        {
            _guid = Guid.NewGuid();
        }
        public string GetGuid()
        {
            return _guid.ToString();
        }
    }
}
