namespace Nivaes.IoC;

public class ServiceIsNotRegisteredException
    : Exception
{
    internal ServiceIsNotRegisteredException()
        : base()
    {
    }

    internal ServiceIsNotRegisteredException(string message)
        : base(message)
    {
    }

    internal ServiceIsNotRegisteredException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }
}
