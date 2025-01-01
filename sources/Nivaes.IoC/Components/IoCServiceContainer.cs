namespace Nivaes.IoC
{
    public abstract class IoCServiceContainer : IIoCResolver, IDisposable
    {
        protected IoTCollection<IInstanceResolver> resolvers = new IoTCollection<IInstanceResolver>();
        protected IoTCollection<IInstanceResolver> scopedResolvers = new IoTCollection<IInstanceResolver>();

        protected readonly bool Scoped;

        protected IoCServiceContainer()
        {
        }

        protected IoCServiceContainer(IoTCollection<IInstanceResolver> resolvers,
            IoTCollection<IInstanceResolver> scopedResolvers, bool scope = false)
        {
            this.resolvers = resolvers;
            this.scopedResolvers = scopedResolvers;
            Scoped = scope;
        }

        //private void LoadData(IEnumerable)
        //{

        //}

        //protected IoCServiceContainer(IDictionary<int, IInstanceResolver> resolvers,
        //    IDictionary<int, IInstanceResolver> scopedResolvers, bool scope = false)
        //{
        //    this.resolvers = resolvers;
        //    this.scopedResolvers = scopedResolvers;
        //    Scoped = scope;

        //    //resolverSearcher = this.resolvers.ToDictionarySeeker();
        //    //scopedResolversSearcher = this.scopedResolvers.ToDictionarySeeker();
        //}

        public abstract IIoCResolver CreateScope();

        public abstract IIoCResolver Clone();

        //public void Frozen()
        //{
        //    //var aa = resolvers.Select(x => x.Key).ToArray();
        //    //resolvers = resolvers.ToFrozenDictionary();
        //    //var bb = resolvers.Select(x => x.Key).ToArray();
        //    //scopedResolvers = scopedResolvers.ToFrozenDictionary();

        //    resolverSearcher = resolvers.ToFrozenSeeker();
        //    scopedResolversSearcher = scopedResolvers.ToFrozenSeeker();
        //}

        //public void Optimize()
        //{
        //    resolverSearcher = resolvers.ToIoTSeeker();
        //    scopedResolversSearcher = scopedResolvers.ToIoTSeeker();
        //}

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
                    scopedResolvers.Replace(interfaceType, new SingletonResolver(resolver));
                    break;
                case Reuse.Singleton:
                    resolvers.Replace(interfaceType, new SingletonResolver(resolver));
                    break;
                case Reuse.Transient:
                    resolvers.Replace(interfaceType, new TransientResolver(resolver));
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
            resolvers.Replace(typeof(TValue), new SingletonResolver(o => value!));
        }

        public void Merge(IoCServiceContainer container)
        {
            resolvers.Merge(container.resolvers);
            scopedResolvers.Merge(container.scopedResolvers);

            //foreach (var resolver in container.resolvers)
            //{
            //    resolvers.Add(resolver.Key, resolver.Value);
            //}

            //foreach (var resolver in container.scopedResolvers)
            //{
            //    scopedResolvers.Add(resolver.Key, resolver.Value);
            //}
        }

        #region IDispose
        private bool disposed = false;

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
