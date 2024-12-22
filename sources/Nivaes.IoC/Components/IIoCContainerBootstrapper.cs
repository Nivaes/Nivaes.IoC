namespace Nivaes.IoC
{
    public interface IIoCContainerBootstrapper
    {
        void AddTransient<TImplementation>();
        void AddTransient<TInterface, TImplementation>();
        void AddSingleton<TImplementation>();
        void AddSingleton<TInterface, TImplementation>();
        void AddScoped<TImplementation>();
        void AddScoped<TInterface, TImplementation>();
    }
}
