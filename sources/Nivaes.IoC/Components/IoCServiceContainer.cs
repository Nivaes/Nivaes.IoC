using System;
using System.Collections;
using System.Collections.Frozen;
using Nivaes.IoC.Components;
using Nivaes.IoC.Components.Collections;
using Nivaes.IoC.Core;

namespace Nivaes.IoC
{
    public abstract class IoCServiceContainer : IIoCResolver, IDisposable
    {
        protected IDictionary<int, IInstanceResolver> resolvers = new Dictionary<int, IInstanceResolver>();
        protected IDictionary<int, IInstanceResolver> scopedResolvers = new Dictionary<int, IInstanceResolver>();

        protected ISearcher<IInstanceResolver> resolverSearcher;
        protected ISearcher<IInstanceResolver> scopedResolversSearcher;

        protected readonly bool Scoped;

        private bool disposed = false;

        protected IoCServiceContainer()
        {
            resolverSearcher = resolvers.ToDictionarySeeker();
            scopedResolversSearcher = scopedResolvers.ToDictionarySeeker();
        }

        protected IoCServiceContainer(IDictionary<int, IInstanceResolver> resolvers,
            IDictionary<int, IInstanceResolver> scopedResolvers, bool scope = false)
        {
            this.resolvers = resolvers;
            this.scopedResolvers = scopedResolvers;
            Scoped = scope;

            resolverSearcher = this.resolvers.ToDictionarySeeker();
            scopedResolversSearcher = this.scopedResolvers.ToDictionarySeeker();
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
            //var aa = resolvers.Select(x => x.Key).ToArray();
            //resolvers = resolvers.ToFrozenDictionary();
            //var bb = resolvers.Select(x => x.Key).ToArray();
            //scopedResolvers = scopedResolvers.ToFrozenDictionary();

            resolverSearcher = resolvers.ToFrozenSeeker();
            scopedResolversSearcher = scopedResolvers.ToFrozenSeeker();
        }

        public void Optimize()
        {
            resolverSearcher = resolvers.ToIoTSeeker();
            scopedResolversSearcher = scopedResolvers.ToIoTSeeker();
        }

        protected abstract void Bootstrap(IIoCServiceContainerBootstrapper bootstrapper);

        public object? Resolve(Type serviceType)
        {
            var serviceTypeHashCode = serviceType.GetHashCode();

            if (resolverSearcher.TryGetValue(serviceTypeHashCode, out var entry))
            {
                return entry.Resolve(this);
            }

            if (Scoped && scopedResolversSearcher.TryGetValue(serviceTypeHashCode, out entry))
            {
                return entry.Resolve(this);
            }

            if (scopedResolversSearcher.TryGetValue(serviceTypeHashCode, out entry))
            {
                ExceptionHelper.ScopedWithoutScopeException(serviceType.FullName ?? string.Empty);
            }

            ExceptionHelper.ServiceIsNotRegistered(serviceType.FullName ?? string.Empty);
            return null;
        }

        public object? Resolve(Type type, IOverrides overrides)
        {
            var typeHashCode = type.GetHashCode();

            if (resolverSearcher.TryGetValue(typeHashCode, out var entry))
            {
                return entry.Resolve(this, overrides);
            }

            if (Scoped && scopedResolversSearcher.TryGetValue(typeHashCode, out entry))
            {
                return entry.Resolve(this, overrides);
            }

            if (scopedResolversSearcher.TryGetValue(typeHashCode, out entry))
            {
                ExceptionHelper.ScopedWithoutScopeException(type.FullName ?? string.Empty);
            }

            ExceptionHelper.ServiceIsNotRegistered(type.FullName ?? string.Empty);
            return null;
        }

        public void AddDelegate(Func<IIoCResolver, object> resolver, Type interfaceType,
            Reuse reuse = Reuse.Transient)
        {
            var interfaceTypeHashCode = interfaceType.GetHashCode();

            switch (reuse)
            {
                case Reuse.Scoped:
                    scopedResolvers.Add(interfaceTypeHashCode, new SingletonResolver(resolver));
                    break;
                case Reuse.Singleton:
                    resolvers.Add(interfaceTypeHashCode, new SingletonResolver(resolver));
                    break;
                case Reuse.Transient:
                    resolvers.Add(interfaceTypeHashCode, new TransientResolver(resolver));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(reuse), reuse, null);
            }
        }

        public void ReplaceDelegate(Func<IIoCResolver, object> resolver, Type interfaceType,
            Reuse reuse = Reuse.Transient)
        {
            var interfaceTypeHashCode = interfaceType.GetHashCode();

            switch (reuse)
            {
                case Reuse.Scoped:
                    scopedResolvers[interfaceTypeHashCode] = new SingletonResolver(resolver);
                    break;
                case Reuse.Singleton:
                    resolvers[interfaceTypeHashCode] = new SingletonResolver(resolver);
                    break;
                case Reuse.Transient:
                    resolvers[interfaceTypeHashCode] = new TransientResolver(resolver);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(reuse), reuse, null);
            }
        }

        public void AddInstance<TValue>(TValue value)
        {
            resolvers.Add(typeof(TValue).GetHashCode(), new SingletonResolver(o => value!));
        }

        public void ReplaceInstance<TValue>(TValue value)
        {
            resolvers[typeof(TValue).GetHashCode()] = new SingletonResolver(o => value!);
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

        #region IDispose
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
        #endregion
    }
}
