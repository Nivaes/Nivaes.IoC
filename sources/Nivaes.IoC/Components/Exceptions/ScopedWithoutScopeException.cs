namespace Nivaes.IoC
{
    public class ScopedWithoutScopeException
        : Exception
    {
        internal ScopedWithoutScopeException()
            : base()
        {
        }

        internal ScopedWithoutScopeException(string message)
            : base(message)
        {

        }

        internal ScopedWithoutScopeException(string? message, Exception? innerException)
            : base(message, innerException)
        {
        }
    }
}
