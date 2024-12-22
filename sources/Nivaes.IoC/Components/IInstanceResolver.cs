using System;

namespace Nivaes.IoC
{
    public interface IInstanceResolver : IDisposable
    {
        object Resolve(IIoCResolver resolver);
        object Resolve(IIoCResolver resolver, IOverrides overrides);

        IInstanceResolver Duplicate();
    }

    public interface ICreator<T>
    {
        T Create(IIoCResolver resolver);
        T Create(IIoCResolver resolver, IOverrides overrides);
    }
    
    public sealed class TransientResolver : IInstanceResolver
    {
        private readonly Func<IIoCResolver, object> _activator;

        public TransientResolver(Func<IIoCResolver, object> activator)
        {
            _activator = activator;
        }

        public object Resolve(IIoCResolver resolver)
        {
            return _activator(resolver);
        }

        public object Resolve(IIoCResolver resolver, IOverrides overrides)
        {
            return _activator(resolver);
        }

        public IInstanceResolver Duplicate()
        {
            return new TransientResolver(_activator);
        }

        public void Dispose()
        {
        }
    }

    public sealed class TransientResolver<TCreator, TType> : IInstanceResolver
        where TCreator : struct, ICreator<TType>
    {
        public object Resolve(IIoCResolver resolver)
        {
            return default(TCreator).Create(resolver);
        }

        public object Resolve(IIoCResolver resolver, IOverrides overrides)
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
        private object @object = new object();
        private object _cache;
        private bool _disposed;
        private Func<IIoCResolver, object> _resolve;
        private Func<IIoCResolver, IOverrides, object> _resolveOverride;

        public SingletonResolver()
        {
            _cache = null;
            _disposed = false;

            _resolve = ResolveInternal;
            _resolveOverride = ResolveInternalOverride;
        }

        public object Resolve(IIoCResolver resolver)
        {
            return _resolve(resolver);
        }

        public object Resolve(IIoCResolver resolver, IOverrides overrides)
        {
            return _resolveOverride(resolver, overrides);
        }

        public IInstanceResolver Duplicate()
        {
            return new SingletonResolver<TCreator, TType>();
        }

        public void Dispose()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException("Instance resolver was disposed. It may happen because the scope was disposed.");
            }

            if (_cache != null && _cache is IDisposable disposable)
            {
                _disposed = true;
                disposable.Dispose();
            }
        }

        private object ResolveInternal(IIoCResolver resolver)
        {
            lock (@object)
            {
                if (_cache != null)
                {
                    return _cache;
                }

                var creator = default(TCreator);
                _cache = creator.Create(resolver);
                _resolve = o => _cache;
                _resolveOverride = (o, oo) => _cache;;
                return _cache;
            }
        }
        
        private object ResolveInternalOverride(IIoCResolver resolver, IOverrides overrides)
        {
            lock (@object)
            {
                if (_cache != null)
                {
                    return _cache;
                }

                var creator = default(TCreator);
                _cache = creator.Create(resolver, overrides);
                _resolve = o => _cache;
                _resolveOverride = (o, oo) => _cache;;
                return _cache;
            }
        }
    }

    public sealed class SingletonResolver : IInstanceResolver
    {
        private readonly Func<IIoCResolver, object> _activator;
        private object _cache;
        private bool _disposed;
        private Func<IIoCResolver, object> _resolve;

        public SingletonResolver(Func<IIoCResolver, object> activator)
        {
            _activator = activator;
            _cache = null;
            _disposed = false;

            _resolve = ResolveInternal;
        }

        public object Resolve(IIoCResolver resolver)
        {
            return _resolve(resolver);
        }

        public object Resolve(IIoCResolver resolver, IOverrides overrides)
        {
            return _resolve(resolver);
        }

        public IInstanceResolver Duplicate()
        {
            return new SingletonResolver(_activator);
        }

        public void Dispose()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException("Instance resolver was disposed. It may happen because the scope was disposed.");
            }

            if (_cache != null && _cache is IDisposable disposable)
            {
                _disposed = true;
                disposable.Dispose();
            }
        }

        private object ResolveInternal(IIoCResolver resolver)
        {
            lock (_activator)
            {
                if (_cache != null)
                {
                    return _cache;
                }

                _cache = _activator(resolver);
                _resolve = GetCached;
                return _cache;
            }
        }

        private object GetCached(IIoCResolver resolver)
        {
            return _cache;
        }
    }
}