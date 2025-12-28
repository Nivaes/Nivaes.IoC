using System.Diagnostics.CodeAnalysis;

namespace Nivaes.IoC
{
    public static class IoCServiceContainerExtensions
    {
        extension(IIoCResolver container)
        {
            public TService? Resolve<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TService>()
            {
                return (TService?)container.Resolve(typeof(TService));
            }

            public TService? Resolve<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TService>(IOverrides overrides)
            {
                return (TService?)container.Resolve(typeof(TService), overrides);
            }
        }

        extension(IoCServiceContainer container)
        {
            public void AddDelegate<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TService>(Func<IIoCResolver, TService?> resolver, Reuse reuse = Reuse.Transient)
            {
                container.AddDelegate(r => resolver(r)!, typeof(TService), reuse);
            }

            public void ReplaceDelegate<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TService>(Func<IIoCResolver, TService?> resolver, Reuse reuse = Reuse.Transient)
            {
                container.ReplaceDelegate(r => resolver(r)!, typeof(TService), reuse);
            }
        }
    }
}
