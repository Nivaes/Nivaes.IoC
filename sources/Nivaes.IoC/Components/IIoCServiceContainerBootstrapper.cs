namespace Nivaes.IoC
{
    public interface IIoCServiceContainerBootstrapper
    {
        void AddTransient<TImplementation>();
        void AddTransient<TInterface, TImplementation>()
            where TImplementation : TInterface;


        void AddSingleton<TImplementation>();
        void AddSingleton<TInterface, TImplementation>()
            where TImplementation : TInterface;

        void AddScoped<TImplementation>();
        void AddScoped<TInterface, TImplementation>()
            where TImplementation : TInterface;
    }
}
