namespace Nivaes.IoC;

public abstract class IoCServiceContainer : IIoCResolver, IDisposable
{
    protected IoCCollection<IInstanceResolver> mResolvers;
    protected IoCCollection<IInstanceResolver> mScopedResolvers;

    protected readonly bool Scoped;

    protected IoCServiceContainer()
    {
        mResolvers = new IoCCollection<IInstanceResolver>(0);
        mScopedResolvers = new IoCCollection<IInstanceResolver>(0);
    }

    protected IoCServiceContainer(IoCCollection<IInstanceResolver> resolvers,
        IoCCollection<IInstanceResolver> scopedResolvers, bool scope = false)
    {
        mResolvers = resolvers;
        mScopedResolvers = scopedResolvers;
        Scoped = scope;
    }

    protected void LoadData(KeyInstanceResolverValue<IInstanceResolver>[] resolvers, KeyInstanceResolverValue<IInstanceResolver>[] scopedResolvers)
    {
        mResolvers = new IoCCollection<IInstanceResolver>(resolvers);
        mScopedResolvers = new IoCCollection<IInstanceResolver>(scopedResolvers);
    }

    public abstract IIoCResolver CreateScope();

    public abstract IIoCResolver Clone();

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

    public bool TryResolve(Type serviceType, out object? result)
    {
        if (mResolvers.TryGetValue(serviceType, out var entry))
        {
            result = entry.Resolve(this);
            return true;
        }

        if (Scoped && mScopedResolvers.TryGetValue(serviceType, out entry))
        {
            result = entry.Resolve(this);
            return true;
        }

        if (mScopedResolvers.TryGetValue(serviceType, out entry))
        {
            result = null;
            return false;
        }

        result = null;
        return false;
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

    public bool TryResolve(Type type, IOverrides overrides, out object? result)
    {
        if (mResolvers.TryGetValue(type, out var entry))
        {
            result = entry.Resolve(this, overrides);
            return true;
        }

        if (Scoped && mScopedResolvers.TryGetValue(type, out entry))
        {
            result = entry.Resolve(this, overrides);
            return true;   
        }

        if (mScopedResolvers.TryGetValue(type, out entry))
        {
            result = null;
            return false;
        }

        result = null;
        return false;
    }


    public void AddDelegate(Func<IIoCResolver, object?> resolver,
                            Type interfaceType,
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

    public void ReplaceDelegate(Func<IIoCResolver, object?> resolver,
                                Type interfaceType,
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
