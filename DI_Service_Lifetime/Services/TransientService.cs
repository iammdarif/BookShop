namespace DI_Service_Lifetime.Services
{
    public class TransientService : ITransientService
    {
        private readonly Guid _guid;

        public TransientService()
        {
            _guid = Guid.NewGuid();
        }
        public string GetGuid()
        {
            return _guid.ToString();
        }
    }
}
