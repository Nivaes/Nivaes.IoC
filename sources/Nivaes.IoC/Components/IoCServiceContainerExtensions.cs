namespace Nivaes.IoC
{
    public static class IoCServiceContainerExtensions
    {
        public static TService? Resolve<TService>(this IIoCResolver container)
        {
            return (TService?)container.Resolve(typeof(TService));
        }
        
        public static TService? Resolve<TService>(this IIoCResolver container, IOverrides overrides)
        {
            return (TService?)container.Resolve(typeof(TService), overrides);
        }

        public static void AddDelegate<TService>(this IoCServiceContainer container, Func<IIoCResolver, TService?> resolver, Reuse reuse = Reuse.Transient)
        {
            container.AddDelegate(r => resolver(r)!, typeof(TService), reuse);
        }
        
        public static void ReplaceDelegate<TService>(this IoCServiceContainer container, Func<IIoCResolver, TService?> resolver, Reuse reuse = Reuse.Transient)
        {
            container.ReplaceDelegate(r => resolver(r)!, typeof(TService), reuse);
        }
    }
}
