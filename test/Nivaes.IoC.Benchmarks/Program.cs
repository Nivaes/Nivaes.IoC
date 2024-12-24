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

    public interface IUserService
    {
    }

    public class UserService : IUserService
    {
        public UserService(Helper helper)
        {
        }

        public Guid Id { get; } = Guid.NewGuid();
    }

    public class Helper
    {
    }

    public class SingleHelper
    {
    }

    public class SingleService(SingleHelper helper)
    {
        private readonly SingleHelper helper = helper;
    }

    public partial class ZeroContainer : ZeroIoCContainer
    {
        protected override void Bootstrap(IZeroIoCContainerBootstrapper bootstrapper)
        {
            bootstrapper.AddTransient<Helper>();
            bootstrapper.AddTransient<IUserService, UserService>();
            bootstrapper.AddSingleton<SingleHelper>();
            bootstrapper.AddSingleton<SingleService>();
        }
    }

    public partial class BenchmarkIoCServiceContainer : IoCServiceContainer
    {
        protected override void Bootstrap(IIoCServiceContainerBootstrapper bootstrapper)
        {
            bootstrapper.AddTransient<Helper>();
            bootstrapper.AddTransient<IUserService, UserService>();
            bootstrapper.AddSingleton<SingleHelper>();
            bootstrapper.AddSingleton<SingleService>();
        }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            BenchmarkRunner.Run<IoCStartupBenchmark>();
            //BenchmarkRunner.Run<IoCRuntimeBenchmark>();
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
            services.AddSingleton<SingleHelper>();
            services.AddSingleton<SingleService>();
            services.AddTransient<Helper>();
            services.AddTransient<IUserService, UserService>();

            return services.BuildServiceProvider();
        }

        public static DependencyInjectionContainer CreateGrace()
        {
            var grace = new DependencyInjectionContainer();
            grace.Configure(o =>
            {
                o.Export<SingleHelper>().As<SingleHelper>().UsingLifestyle(new SingletonLifestyle());
                o.Export<SingleService>().As<SingleService>().UsingLifestyle(new SingletonLifestyle());

                o.Export<Helper>().As<Helper>();
                o.Export<UserService>().As<IUserService>();
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
            var userService = (IUserService?)resolver.GetService(typeof(IUserService));
            var singleService = (SingleService?)resolver.GetService(typeof(SingleService));
        }

        [Benchmark]
        public void ZeroIoCStartup()
        {
            var resolver = Creators.CreateZeroIoC();
            var userService = (IUserService?)resolver.Resolve(typeof(IUserService));
            var singleService = (SingleService?)resolver.Resolve(typeof(SingleService));
        }

        [Benchmark]
        public void IoCServiceContainerStartup()
        {
            var resolver = Creators.CreateIoCServiceContainer();
            var userService = (IUserService?)resolver.Resolve(typeof(IUserService));
            var singleService = (SingleService?)resolver.Resolve(typeof(SingleService));
        }

        [Benchmark]
        public void GraceStartup()
        {
            var resolver = Creators.CreateGrace();
            var userService = (IUserService?)resolver.Locate(typeof(IUserService));
            var singleService = (SingleService?)resolver.Locate(typeof(SingleService));
        }
    }

    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    public class IoCRuntimeBenchmark
    {
        private readonly DependencyInjectionContainer _grace;
        private readonly ZeroContainer _zeroIoCContainer;
        private readonly BenchmarkIoCServiceContainer _iocServiceContainer;
        private readonly ServiceProvider _serviceProvider;

        public IoCRuntimeBenchmark()
        {
            _grace = Creators.CreateGrace();
            _serviceProvider = Creators.CreateMicrosoft();
            _zeroIoCContainer = Creators.CreateZeroIoC();
            _iocServiceContainer = Creators.CreateIoCServiceContainer();
        }

        [Benchmark]
        public IUserService? MicrosoftTransient()
        {
            return (IUserService?)_serviceProvider.GetService(typeof(IUserService));
        }

        [Benchmark]
        public IUserService? ZeroIoCTransient()
        {
            return (IUserService?)_zeroIoCContainer.Resolve(typeof(IUserService));
        }

        [Benchmark]
        public IUserService? IoCServiceContainerTransient()
        {
            return (IUserService?)_iocServiceContainer.Resolve(typeof(IUserService));
        }

        [Benchmark]
        public IUserService GraceTransient()
        {
            return (IUserService)_grace.Locate(typeof(IUserService));
        }

        [Benchmark]
        public SingleService? MicrosoftSingleton()
        {
            return (SingleService?)_serviceProvider.GetService(typeof(SingleService));
        }

        [Benchmark]
        public SingleService? ZeroIoCSingleton()
        {
            return (SingleService?)_zeroIoCContainer.Resolve(typeof(SingleService));
        }

        [Benchmark]
        public SingleService? IoCServiceContainerSingleton()
        {
            return (SingleService?)_iocServiceContainer.Resolve(typeof(SingleService));
        }

        [Benchmark]
        public SingleService GraceSingleton()
        {
            return (SingleService)_grace.Locate(typeof(SingleService));
        }
    }
}
