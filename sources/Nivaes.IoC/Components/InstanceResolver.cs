namespace Nivaes.IoC;

public interface IInstanceResolver : IDisposable
{
    object? Resolve(IIoCResolver resolver);
    object? Resolve(IIoCResolver resolver, IOverrides overrides);

    IInstanceResolver Duplicate();
}

public interface ICreator<out T>
{
    T Create(IIoCResolver resolver);
    T Create(IIoCResolver resolver, IOverrides overrides);
}

public sealed class TransientResolver : IInstanceResolver
{
    private readonly Func<IIoCResolver, object?> activator;

    public TransientResolver(Func<IIoCResolver, object?> activator)
    {
        this.activator = activator;
    }

    public object? Resolve(IIoCResolver resolver)
    {
        return activator(resolver);
    }

    public object? Resolve(IIoCResolver resolver, IOverrides overrides)
    {
        return activator(resolver);
    }

    public IInstanceResolver Duplicate()
    {
        return new TransientResolver(activator);
    }

    public void Dispose()
    {
    }
}

public sealed class TransientResolver<TCreator, TType> : IInstanceResolver
    where TCreator : struct, ICreator<TType>
{
    public object? Resolve(IIoCResolver resolver)
    {
        return default(TCreator).Create(resolver);
    }

    public object? Resolve(IIoCResolver resolver, IOverrides overrides)
    {
        return default(TCreator).Create(resolver, overrides);
    }

    public IInstanceResolver Duplicate()
    {
        return new TransientResolver<TCreator, TType>();
    }

    public void Dispose()
    {
    }
}


public sealed class SingletonResolver<TCreator, TType> : IInstanceResolver
    where TCreator : struct, ICreator<TType>
{
    private readonly object @object = new object();
    private object? cache;
    private bool disposed;
    private Func<IIoCResolver, object?> resolve;
    private Func<IIoCResolver, IOverrides, object?> resolveOverride;

    public SingletonResolver()
    {
        cache = null;
        disposed = false;

        resolve = ResolveInternal;
        resolveOverride = ResolveInternalOverride;
    }

    public object? Resolve(IIoCResolver resolver)
    {
        return resolve(resolver);
    }

    public object? Resolve(IIoCResolver resolver, IOverrides overrides)
    {
        return resolveOverride(resolver, overrides);
    }

    public IInstanceResolver Duplicate()
    {
        return new SingletonResolver<TCreator, TType>();
    }

    public void Dispose()
    {
        ObjectDisposedException.ThrowIf(disposed, this);

        if (cache is IDisposable disposable)
        {
            disposed = true;
            disposable.Dispose();
        }
    }

    private object ResolveInternal(IIoCResolver resolver)
    {
        lock (@object)
        {
            if (cache != null)
            {
                return cache;
            }

            var creator = default(TCreator);
            cache = creator.Create(resolver);
            resolve = o => cache!;
            resolveOverride = (o, oo) => cache!;
            return cache!;
        }
    }

    private object ResolveInternalOverride(IIoCResolver resolver, IOverrides overrides)
    {
        lock (@object)
        {
            if (cache != null)
            {
                return cache;
            }

            var creator = default(TCreator);
            cache = creator.Create(resolver, overrides);
            resolve = o => cache!;
            resolveOverride = (o, oo) => cache!;
            return cache!;
        }
    }
}

public sealed class SingletonResolver : IInstanceResolver
{
    private readonly Func<IIoCResolver, object?> activator;
    private object? cache;
    private bool disposed;
    private Func<IIoCResolver, object?> resolve;

    public SingletonResolver(Func<IIoCResolver, object?> activator)
    {
        this.activator = activator;
        cache = null;
        disposed = false;

        resolve = ResolveInternal;
    }

    public object? Resolve(IIoCResolver resolver)
    {
        return resolve(resolver);
    }

    public object? Resolve(IIoCResolver resolver, IOverrides overrides)
    {
        return resolve(resolver);
    }

    public IInstanceResolver Duplicate()
    {
        return new SingletonResolver(activator);
    }

    public void Dispose()
    {
        ObjectDisposedException.ThrowIf(disposed, this);

        if (cache is IDisposable disposable)
        {
            disposed = true;
            disposable.Dispose();
        }
    }

    private object? ResolveInternal(IIoCResolver resolver)
    {
        lock (activator)
        {
            if (cache != null)
            {
                return cache;
            }

            cache = activator(resolver);
            resolve = GetCached;
            return cache;
        }
    }

    private object? GetCached(IIoCResolver resolver)
    {
        return cache;
    }
}
