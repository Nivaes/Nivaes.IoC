using System.Diagnostics.CodeAnalysis;

namespace Nivaes.IoC;

public static class IoCServiceContainerExtensions
{
    extension(IIoCResolver container)
    {
        public TService? Resolve<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TService>()
        {
            return (TService?)container.Resolve(typeof(TService));
        }

        public bool TryResolve<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TService>(out TService? result)
        {
            if (container.TryResolve(typeof(TService), out var internalResult))
            {
                result = (TService?)internalResult;
                return true;
            }
            else
            {
                result = default(TService);
                return false;
            }
        }

        public TService? Resolve<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TService>(IOverrides overrides)
        {
            return (TService?)container.Resolve(typeof(TService), overrides);
        }

        public bool TryResolve<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TService>(IOverrides overrides, out TService? result)
        {
            if (container.TryResolve(typeof(TService), overrides, out var internalResult))
            {
                result = (TService?)internalResult;
                return true;
            }
            else
            {
                result = default(TService);
                return false;
            }
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
