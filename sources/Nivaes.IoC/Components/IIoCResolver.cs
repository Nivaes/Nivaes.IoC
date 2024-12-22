namespace Nivaes.IoC
{
    public interface IIoCResolver : IDisposable
    {
        IIoCResolver CreateScope();

        object? Resolve(Type serviceType);
        object? Resolve(Type type, IOverrides overrides);
    }
}
