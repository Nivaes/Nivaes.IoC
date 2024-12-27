using System;
using System.Collections.Frozen;
using Nivaes.IoC.Core;

namespace Nivaes.IoC
{
    public abstract class IoCServiceContainer : IIoCResolver, IDisposable
    {
        protected readonly IDictionary<Type, IInstanceResolver> Resolvers = new Dictionary<Type, IInstanceResolver>();

        protected readonly IDictionary<Type, IInstanceResolver> ScopedResolvers = new Dictionary<Type, IInstanceResolver>();

        protected readonly bool Scoped;

        private bool disposed = false;

        protected IoCServiceContainer()
        {
        }

        protected IoCServiceContainer(IDictionary<Type, IInstanceResolver> resolvers,
            IDictionary<Type, IInstanceResolver> scopedResolvers, bool scope = false)
        {
            Resolvers = resolvers;
            ScopedResolvers = scopedResolvers;
            Scoped = scope;
        }

        public virtual IIoCResolver CreateScope()
        {
            throw new NotImplementedException(nameof(CreateScope));
        }

        public virtual IIoCResolver Clone()
        {
            throw new NotImplementedException(nameof(CreateScope));
        }

        protected abstract void Bootstrap(IIoCServiceContainerBootstrapper bootstrapper);

        public object? Resolve(Type serviceType)
        {
            if (Resolvers.TryGetValue(serviceType, out var entry))
            {
                return entry.Resolve(this);
            }

            if (Scoped && ScopedResolvers.TryGetValue(serviceType, out entry))
            {
                return entry.Resolve(this);
            }

            if (ScopedResolvers.TryGetValue(serviceType, out entry))
            {
                ExceptionHelper.ScopedWithoutScopeException(serviceType.FullName ?? string.Empty);
            }

            ExceptionHelper.ServiceIsNotRegistered(serviceType.FullName ?? string.Empty);
            return null;
        }

        public object? Resolve(Type type, IOverrides overrides)
        {
            if (Resolvers.TryGetValue(type, out var entry))
            {
                return entry.Resolve(this, overrides);
            }

            if (Scoped && ScopedResolvers.TryGetValue(type, out entry))
            {
                return entry.Resolve(this, overrides);
            }

            if (ScopedResolvers.TryGetValue(type, out entry))
            {
                ExceptionHelper.ScopedWithoutScopeException(type.FullName ?? string.Empty);
            }

            ExceptionHelper.ServiceIsNotRegistered(type.FullName ?? string.Empty);
            return null;
        }

        public void AddDelegate(Func<IIoCResolver, object> resolver, Type interfaceType,
            Reuse reuse = Reuse.Transient)
        {
            switch (reuse)
            {
                case Reuse.Scoped:
                    ScopedResolvers.Add(interfaceType, new SingletonResolver(resolver));
                    break;
                case Reuse.Singleton:
                    Resolvers.Add(interfaceType, new SingletonResolver(resolver));
                    break;
                case Reuse.Transient:
                    Resolvers.Add(interfaceType, new TransientResolver(resolver));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(reuse), reuse, null);
            }
        }

        public void ReplaceDelegate(Func<IIoCResolver, object> resolver, Type interfaceType,
            Reuse reuse = Reuse.Transient)
        {
            switch (reuse)
            {
                case Reuse.Scoped:
                    ScopedResolvers[interfaceType] = new SingletonResolver(resolver);
                    break;
                case Reuse.Singleton:
                    Resolvers[interfaceType] = new SingletonResolver(resolver);
                    break;
                case Reuse.Transient:
                    Resolvers[interfaceType] = new TransientResolver(resolver);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(reuse), reuse, null);
            }
        }

        public void AddInstance<TValue>(TValue value)
        {
            Resolvers.Add(typeof(TValue), new SingletonResolver(o => value!));
        }

        public void ReplaceInstance<TValue>(TValue value)
        {
            Resolvers[typeof(TValue)] = new SingletonResolver(o => value!);
        }

        public void Merge(IoCServiceContainer container)
        {
            foreach (var resolver in container.Resolvers)
            {
                Resolvers.Add(resolver.Key, resolver.Value);
            }

            foreach (var resolver in container.ScopedResolvers)
            {
                ScopedResolvers.Add(resolver.Key, resolver.Value);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                if (!Scoped)
                {
                    foreach (var resolver in Resolvers.Values)
                    {
                        resolver.Dispose();
                    }
                }

                foreach (var resolver in ScopedResolvers.Values)
                {
                    resolver.Dispose();
                }
            }

            disposed = true;
        }
    }
}
