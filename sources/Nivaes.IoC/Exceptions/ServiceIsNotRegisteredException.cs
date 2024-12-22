namespace Nivaes.IoC
{
    public class ServiceIsNotRegisteredException : Exception
    {
        public ServiceIsNotRegisteredException(string message)
            : base(message)
        {

        }
    }
}
