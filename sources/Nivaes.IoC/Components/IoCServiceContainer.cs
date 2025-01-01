namespace Nivaes.IoC
{
    public abstract class IoCServiceContainer : IIoCResolver, IDisposable
    {
        protected IoTCollection<IInstanceResolver> mResolvers;
        protected IoTCollection<IInstanceResolver> mScopedResolvers;

        protected readonly bool Scoped;

        protected IoCServiceContainer()
        {
        }

        protected IoCServiceContainer(IoTCollection<IInstanceResolver> resolvers,
            IoTCollection<IInstanceResolver> scopedResolvers, bool scope = false)
        {
            mResolvers = resolvers;
            mScopedResolvers = scopedResolvers;
            Scoped = scope;
        }

        protected void LoadData(IEnumerable<KeyInstanceResolverValue> resolvers, IEnumerable<KeyInstanceResolverValue> scopedResolvers)
        {
            mResolvers = new IoTCollection<IInstanceResolver>(resolvers);
            mScopedResolvers = new IoTCollection<IInstanceResolver>(scopedResolvers);
        }

        //protected IoCServiceContainer(IInstanceResolver<IInstanceResolver> resolvers, IInstanceResolver<IInstanceResolver> scopedResolvers, bool scope = false)
        //{
        //    mResolvers = resolvers;
        //    mScopedResolvers = scopedResolvers;
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
            if (mResolvers.TryGetValue(serviceType, out var entry))
            {
                return entry.Resolve(this);
            }

            if (Scoped && mScopedResolvers.TryGetValue(serviceType, out entry))
            {
                return entry.Resolve(this);
            }

            if (mScopedResolvers.TryGetValue(serviceType, out entry))
            {
                ExceptionHelper.ScopedWithoutScopeException(serviceType.FullName ?? string.Empty);
            }

            ExceptionHelper.ServiceIsNotRegistered(serviceType.FullName ?? string.Empty);
            return null;
        }

        public object? Resolve(Type type, IOverrides overrides)
        {
            if (mResolvers.TryGetValue(type, out var entry))
            {
                return entry.Resolve(this, overrides);
            }

            if (Scoped && mScopedResolvers.TryGetValue(type, out entry))
            {
                return entry.Resolve(this, overrides);
            }

            if (mScopedResolvers.TryGetValue(type, out entry))
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
                    mScopedResolvers.Add(interfaceType, new SingletonResolver(resolver));
                    break;
                case Reuse.Singleton:
                    mResolvers.Add(interfaceType, new SingletonResolver(resolver));
                    break;
                case Reuse.Transient:
                    mResolvers.Add(interfaceType, new TransientResolver(resolver));
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
                    mScopedResolvers.Replace(interfaceType, new SingletonResolver(resolver));
                    break;
                case Reuse.Singleton:
                    mResolvers.Replace(interfaceType, new SingletonResolver(resolver));
                    break;
                case Reuse.Transient:
                    mResolvers.Replace(interfaceType, new TransientResolver(resolver));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(reuse), reuse, null);
            }
        }

        public void AddInstance<TValue>(TValue value)
        {
            mResolvers.Add(typeof(TValue), new SingletonResolver(o => value!));
        }

        public void ReplaceInstance<TValue>(TValue value)
        {
            mResolvers.Replace(typeof(TValue), new SingletonResolver(o => value!));
        }

        public void Merge(IoCServiceContainer container)
        {
            mResolvers.Merge(container.mResolvers);
            mScopedResolvers.Merge(container.mScopedResolvers);

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
                    foreach (var resolver in mResolvers.Values)
                    {
                        resolver.Dispose();
                    }
                }

                foreach (var resolver in mScopedResolvers.Values)
                {
                    resolver.Dispose();
                }
            }

            disposed = true;
        }
        #endregion
    }
}
