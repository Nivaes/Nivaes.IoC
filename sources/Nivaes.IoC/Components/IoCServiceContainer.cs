using System;
using System.Collections.Frozen;
using Nivaes.IoC.Core;

namespace Nivaes.IoC
{
    public abstract class IoCServiceContainer : IIoCResolver, IDisposable
    {
        protected IDictionary<Type, IInstanceResolver> resolvers = new Dictionary<Type, IInstanceResolver>();

        protected IDictionary<Type, IInstanceResolver> scopedResolvers = new Dictionary<Type, IInstanceResolver>();

        protected readonly bool Scoped;

        private bool disposed = false;

        protected IoCServiceContainer()
        {
        }

        protected IoCServiceContainer(IDictionary<Type, IInstanceResolver> resolvers,
            IDictionary<Type, IInstanceResolver> scopedResolvers, bool scope = false)
        {
            this.resolvers = resolvers;
            this.scopedResolvers = scopedResolvers;
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

        public void Frozen()
        {
            resolvers = resolvers.ToFrozenDictionary();
            scopedResolvers = scopedResolvers.ToFrozenDictionary();
        }

        protected abstract void Bootstrap(IIoCServiceContainerBootstrapper bootstrapper);

        public object? Resolve(Type serviceType)
        {
            if (resolvers.TryGetValue(serviceType, out var entry))
            {
                return entry.Resolve(this);
            }

            if (Scoped && scopedResolvers.TryGetValue(serviceType, out entry))
            {
                return entry.Resolve(this);
            }

            if (scopedResolvers.TryGetValue(serviceType, out entry))
            {
                ExceptionHelper.ScopedWithoutScopeException(serviceType.FullName ?? string.Empty);
            }

            ExceptionHelper.ServiceIsNotRegistered(serviceType.FullName ?? string.Empty);
            return null;
        }

        public object? Resolve(Type type, IOverrides overrides)
        {
            if (resolvers.TryGetValue(type, out var entry))
            {
                return entry.Resolve(this, overrides);
            }

            if (Scoped && scopedResolvers.TryGetValue(type, out entry))
            {
                return entry.Resolve(this, overrides);
            }

            if (scopedResolvers.TryGetValue(type, out entry))
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
                    scopedResolvers.Add(interfaceType, new SingletonResolver(resolver));
                    break;
                case Reuse.Singleton:
                    resolvers.Add(interfaceType, new SingletonResolver(resolver));
                    break;
                case Reuse.Transient:
                    resolvers.Add(interfaceType, new TransientResolver(resolver));
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
                    scopedResolvers[interfaceType] = new SingletonResolver(resolver);
                    break;
                case Reuse.Singleton:
                    resolvers[interfaceType] = new SingletonResolver(resolver);
                    break;
                case Reuse.Transient:
                    resolvers[interfaceType] = new TransientResolver(resolver);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(reuse), reuse, null);
            }
        }

        public void AddInstance<TValue>(TValue value)
        {
            resolvers.Add(typeof(TValue), new SingletonResolver(o => value!));
        }

        public void ReplaceInstance<TValue>(TValue value)
        {
            resolvers[typeof(TValue)] = new SingletonResolver(o => value!);
        }

        public void Merge(IoCServiceContainer container)
        {
            foreach (var resolver in container.resolvers)
            {
                resolvers.Add(resolver.Key, resolver.Value);
            }

            foreach (var resolver in container.scopedResolvers)
            {
                scopedResolvers.Add(resolver.Key, resolver.Value);
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
                    foreach (var resolver in resolvers.Values)
                    {
                        resolver.Dispose();
                    }
                }

                foreach (var resolver in scopedResolvers.Values)
                {
                    resolver.Dispose();
                }
            }

            disposed = true;
        }
    }
}
