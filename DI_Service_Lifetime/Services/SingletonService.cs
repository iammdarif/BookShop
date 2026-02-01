namespace DI_Service_Lifetime.Services
{
    public class SingletonService : ISingletonService
    {
        private readonly Guid _guid;

        public SingletonService()
        {
            _guid = Guid.NewGuid();
        }
        public string GetGuid()
        {
            return _guid.ToString();
        }
    }
}
