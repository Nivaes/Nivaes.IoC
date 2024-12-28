namespace Benchmarks
{
    using System;
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Order;
    using BenchmarkDotNet.Running;
    using Grace.DependencyInjection;
    using Grace.DependencyInjection.Lifestyle;
    using Microsoft.Extensions.DependencyInjection;
    using Nivaes.IoC;
    using ZeroIoC;

    #region TestClass
    #region IUserService
    public interface IUserService1
    {
    }

    public class UserService1 : IUserService1
    {

        public UserService1(Helper1 helper1)
        {
        }

        public Guid Id { get; } = Guid.NewGuid();
    }

    public interface IUserService2
    {
    }

    public class UserService2 : IUserService2
    {

        public UserService2(Helper1 helper1)
        {
        }

        public Guid Id { get; } = Guid.NewGuid();
    }

    public interface IUserService3
    {
    }

    public class UserService3 : IUserService3
    {

        public UserService3(Helper1 helper1)
        {
        }

        public Guid Id { get; } = Guid.NewGuid();
    }
    #endregion

    #region Helper
    public class Helper1
    {
    }

    public class Helper2
    {
        private readonly Helper1 helper1;

        public Helper2(Helper1 helper1)
        {
            this.helper1 = helper1;
        }
    }
    #endregion

    #region SingleHelper
    public class SingleHelper1
    {
        private readonly Helper1 helper1;
        private readonly Helper2 helper2;

        public SingleHelper1(Helper1 helper1, Helper2 helper2)
        {
            this.helper1 = helper1;
            this.helper2 = helper2;
        }
    }

    public class SingleHelper2
    {
        private readonly Helper1 helper1;

        public SingleHelper2(Helper1 helper1)
        {
            this.helper1 = helper1;
        }
    }

    public class SingleHelper3
    {
        private readonly Helper2 helper2;

        public SingleHelper3(Helper2 helper2)
        {
            this.helper2 = helper2;
        }
    }
    #endregion

    #region SingleService
    public class SingleService1(SingleHelper1 helper)
    {
        private readonly SingleHelper1 helper = helper;
    }

    public class SingleService2(SingleHelper2 helper)
    {
        private readonly SingleHelper2 helper = helper;
    }
    #endregion
    #endregion

    public partial class ZeroContainer : ZeroIoCContainer
    {
        protected override void Bootstrap(IZeroIoCContainerBootstrapper bootstrapper)
        {
            bootstrapper.AddTransient<Helper1>();
            bootstrapper.AddTransient<Helper2>();
            bootstrapper.AddTransient<IUserService1, UserService1>();
            bootstrapper.AddTransient<IUserService2, UserService2>();
            bootstrapper.AddTransient<IUserService3, UserService3>();
            bootstrapper.AddSingleton<SingleHelper1>();
            bootstrapper.AddSingleton<SingleHelper2>();
            bootstrapper.AddSingleton<SingleHelper3>();
            bootstrapper.AddSingleton<SingleService1>();
            bootstrapper.AddSingleton<SingleService2>();
        }
    }

    public partial class BenchmarkIoCServiceContainer : IoCServiceContainer
    {
        protected override void Bootstrap(IIoCServiceContainerBootstrapper bootstrapper)
        {
            bootstrapper.AddTransient<Helper1>();
            bootstrapper.AddTransient<Helper2>();
            bootstrapper.AddTransient<IUserService1, UserService1>();
            bootstrapper.AddTransient<IUserService2, UserService2>();
            bootstrapper.AddTransient<IUserService3, UserService3>();
            bootstrapper.AddSingleton<SingleHelper1>();
            bootstrapper.AddSingleton<SingleHelper2>();
            bootstrapper.AddSingleton<SingleHelper3>();
            bootstrapper.AddSingleton<SingleService1>();
            bootstrapper.AddSingleton<SingleService2>();
        }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            BenchmarkRunner.Run<IoCStartupBenchmark>();
            BenchmarkRunner.Run<IoCRuntimeBenchmark>();
        }
    }

    public class Creators
    {
        public static ZeroContainer CreateZeroIoC()
        {
            return new ZeroContainer();
        }

        public static BenchmarkIoCServiceContainer CreateIoCServiceContainer()
        {
            return new BenchmarkIoCServiceContainer();
        }

        public static ServiceProvider CreateMicrosoft()
        {
            var services = new ServiceCollection();
            services.AddTransient<Helper1>();
            services.AddTransient<Helper2>();
            services.AddTransient<IUserService1, UserService1>();
            services.AddTransient<IUserService2, UserService2>();
            services.AddTransient<IUserService3, UserService3>();
            services.AddSingleton<SingleHelper1>();
            services.AddSingleton<SingleHelper2>();
            services.AddSingleton<SingleHelper3>();
            services.AddSingleton<SingleService1>();
            services.AddSingleton<SingleService2>();

            return services.BuildServiceProvider();
        }

        public static DependencyInjectionContainer CreateGrace()
        {
            var grace = new DependencyInjectionContainer();
            grace.Configure(o =>
            {
                o.Export<Helper1>().As<Helper1>();
                o.Export<Helper2>().As<Helper2>();
                o.Export<UserService1>().As<IUserService1>();
                o.Export<UserService2>().As<IUserService2>();
                o.Export<UserService3>().As<IUserService3>();

                o.Export<SingleHelper1>().As<SingleHelper1>().UsingLifestyle(new SingletonLifestyle());
                o.Export<SingleHelper2>().As<SingleHelper2>().UsingLifestyle(new SingletonLifestyle());
                o.Export<SingleHelper3>().As<SingleHelper3>().UsingLifestyle(new SingletonLifestyle());
                o.Export<SingleService1>().As<SingleService1>().UsingLifestyle(new SingletonLifestyle());
                o.Export<SingleService2>().As<SingleService2>().UsingLifestyle(new SingletonLifestyle());
            });

            return grace;
        }
    }

    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    public class IoCStartupBenchmark
    {
        [Benchmark]
        public void MicrosoftStartup()
        {
            var resolver = Creators.CreateMicrosoft();
            var userService = (IUserService1?)resolver.GetService(typeof(IUserService1));
            var singleService = (SingleService1?)resolver.GetService(typeof(SingleService1));
        }

        [Benchmark]
        public void ZeroIoCStartup()
        {
            var resolver = Creators.CreateZeroIoC();
            var userService = (IUserService1?)resolver.Resolve(typeof(IUserService1));
            var singleService = (SingleService1?)resolver.Resolve(typeof(SingleService1));
        }

        [Benchmark]
        public void IoCServiceContainerStartup()
        {
            var resolver = Creators.CreateIoCServiceContainer();
            var userService = (IUserService1?)resolver.Resolve(typeof(IUserService1));
            var singleService = (SingleService1?)resolver.Resolve(typeof(SingleService1));
        }

        [Benchmark]
        public void IoCServiceContainerFrozenStartup()
        {
            var resolver = Creators.CreateIoCServiceContainer();
            resolver.Frozen();
            var userService = (IUserService1?)resolver.Resolve(typeof(IUserService1));
            var singleService = (SingleService1?)resolver.Resolve(typeof(SingleService1));
        }

        [Benchmark]
        public void GraceStartup()
        {
            var resolver = Creators.CreateGrace();
            var userService = (IUserService1?)resolver.Locate(typeof(IUserService1));
            var singleService = (SingleService1?)resolver.Locate(typeof(SingleService1));
        }
    }

    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    public class IoCRuntimeBenchmark
    {
        private readonly DependencyInjectionContainer _grace;
        private readonly ZeroContainer _zeroIoCContainer;
        private readonly BenchmarkIoCServiceContainer _iocServiceContainer;
        private readonly BenchmarkIoCServiceContainer _iocServiceContainerFrozen;
        private readonly ServiceProvider _serviceProvider;

        public IoCRuntimeBenchmark()
        {
            _grace = Creators.CreateGrace();
            _serviceProvider = Creators.CreateMicrosoft();
            _zeroIoCContainer = Creators.CreateZeroIoC();
            _iocServiceContainer = Creators.CreateIoCServiceContainer();

            _iocServiceContainerFrozen = Creators.CreateIoCServiceContainer();
            _iocServiceContainerFrozen.Frozen();
        }

        [Benchmark]
        public IUserService1? MicrosoftTransient()
        {
            return (IUserService1?)_serviceProvider.GetService(typeof(IUserService1));
        }

        [Benchmark]
        public IUserService1? ZeroIoCTransient()
        {
            return (IUserService1?)_zeroIoCContainer.Resolve(typeof(IUserService1));
        }

        [Benchmark]
        public IUserService1? IoCServiceContainerTransient()
        {
            return (IUserService1?)_iocServiceContainer.Resolve(typeof(IUserService1));
        }

        [Benchmark]
        public IUserService1? IoCServiceContainerFrozenTransient()
        {
            return (IUserService1?)_iocServiceContainerFrozen.Resolve(typeof(IUserService1));
        }

        [Benchmark]
        public IUserService1 GraceTransient()
        {
            return (IUserService1)_grace.Locate(typeof(IUserService1));
        }

        [Benchmark]
        public SingleService1? MicrosoftSingleton()
        {
            return (SingleService1?)_serviceProvider.GetService(typeof(SingleService1));
        }

        [Benchmark]
        public SingleService1? ZeroIoCSingleton()
        {
            return (SingleService1?)_zeroIoCContainer.Resolve(typeof(SingleService1));
        }

        [Benchmark]
        public SingleService1? IoCServiceContainerSingleton()
        {
            return (SingleService1?)_iocServiceContainer.Resolve(typeof(SingleService1));
        }
        [Benchmark]
        public SingleService1? IoCServiceContainerFrozenSingleton()
        {
            return (SingleService1?)_iocServiceContainerFrozen.Resolve(typeof(SingleService1));
        }

        [Benchmark]
        public SingleService1 GraceSingleton()
        {
            return (SingleService1)_grace.Locate(typeof(SingleService1));
        }
    }
}
