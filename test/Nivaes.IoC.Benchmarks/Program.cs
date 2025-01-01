namespace Benchmarks
{
    using System;
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Configs;
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


    public interface IUserService4
    {
    }

    public class UserService4 : IUserService4
    {

        public UserService4(Helper1 helper1)
        {
        }

        public Guid Id { get; } = Guid.NewGuid();
    }

    public interface IUserService5
    {
    }

    public class UserService5 : IUserService5
    {

        public UserService5(Helper1 helper1)
        {
        }

        public Guid Id { get; } = Guid.NewGuid();
    }

    public interface IUserService6
    {
    }

    public class UserService6 : IUserService6
    {

        public UserService6(Helper1 helper1)
        {
        }

        public Guid Id { get; } = Guid.NewGuid();
    }

    public interface IUserService7
    {
    }

    public class UserService7 : IUserService7
    {

        public UserService7(Helper1 helper1)
        {
        }

        public Guid Id { get; } = Guid.NewGuid();
    }

    public interface IUserService8
    {
    }

    public class UserService8 : IUserService8
    {

        public UserService8(Helper1 helper1)
        {
        }

        public Guid Id { get; } = Guid.NewGuid();
    }

    public interface IUserService9
    {
    }

    public class UserService9 : IUserService9
    {

        public UserService9()
        {
        }

        public Guid Id { get; } = Guid.NewGuid();
    }

    public interface IUserService10
    {
    }

    public class UserService10 : IUserService10
    {

        public UserService10(Helper1 helper1)
        {
        }

        public Guid Id { get; } = Guid.NewGuid();
    }

    public interface IUserService11
    {
    }

    public class UserService11 : IUserService11
    {

        public UserService11(Helper1 helper1)
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

    public class SingleService3(SingleHelper2 helper)
    {
        private readonly SingleHelper2 helper = helper;
    }

    public class SingleService4(SingleHelper2 helper)
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
            bootstrapper.AddTransient<IUserService4, UserService4>();
            bootstrapper.AddTransient<IUserService5, UserService5>();
            bootstrapper.AddTransient<IUserService6, UserService6>();
            bootstrapper.AddTransient<IUserService7, UserService7>();
            bootstrapper.AddTransient<IUserService8, UserService8>();
            bootstrapper.AddTransient<IUserService9, UserService9>();
            bootstrapper.AddTransient<IUserService10, UserService10>();
            bootstrapper.AddTransient<IUserService11, UserService11>();
            bootstrapper.AddSingleton<SingleHelper1>();
            bootstrapper.AddSingleton<SingleHelper2>();
            bootstrapper.AddSingleton<SingleHelper3>();
            bootstrapper.AddSingleton<SingleService1>();
            bootstrapper.AddSingleton<SingleService2>();
            bootstrapper.AddSingleton<SingleService3>();
            bootstrapper.AddSingleton<SingleService4>();
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
            bootstrapper.AddTransient<IUserService4, UserService4>();
            bootstrapper.AddTransient<IUserService5, UserService5>();
            bootstrapper.AddTransient<IUserService6, UserService6>();
            bootstrapper.AddTransient<IUserService7, UserService7>();
            bootstrapper.AddTransient<IUserService8, UserService8>();
            bootstrapper.AddTransient<IUserService9, UserService9>();
            bootstrapper.AddTransient<IUserService10, UserService10>();
            bootstrapper.AddTransient<IUserService11, UserService11>();
            bootstrapper.AddSingleton<SingleHelper1>();
            bootstrapper.AddSingleton<SingleHelper2>();
            bootstrapper.AddSingleton<SingleHelper3>();
            bootstrapper.AddSingleton<SingleService1>();
            bootstrapper.AddSingleton<SingleService2>();
            bootstrapper.AddSingleton<SingleService3>();
            bootstrapper.AddSingleton<SingleService4>();
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
            services.AddTransient<Helper1>();
            services.AddTransient<Helper2>();
            services.AddTransient<IUserService1, UserService1>();
            services.AddTransient<IUserService2, UserService2>();
            services.AddTransient<IUserService3, UserService3>();
            services.AddTransient<IUserService4, UserService4>();
            services.AddTransient<IUserService5, UserService5>();
            services.AddTransient<IUserService6, UserService6>();
            services.AddTransient<IUserService7, UserService7>();
            services.AddTransient<IUserService8, UserService8>();
            services.AddTransient<IUserService9, UserService9>();
            services.AddTransient<IUserService10, UserService10>();
            services.AddTransient<IUserService11, UserService11>();
            services.AddSingleton<SingleHelper1>();
            services.AddSingleton<SingleHelper2>();
            services.AddSingleton<SingleHelper3>();
            services.AddSingleton<SingleService1>();
            services.AddSingleton<SingleService2>();
            services.AddSingleton<SingleService3>();
            services.AddSingleton<SingleService4>();

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
                o.Export<UserService4>().As<IUserService4>();
                o.Export<UserService5>().As<IUserService5>();
                o.Export<UserService6>().As<IUserService6>();
                o.Export<UserService7>().As<IUserService7>();
                o.Export<UserService8>().As<IUserService8>();
                o.Export<UserService9>().As<IUserService9>();
                o.Export<UserService10>().As<IUserService10>();
                o.Export<UserService11>().As<IUserService11>();
                o.Export<SingleHelper1>().As<SingleHelper1>().UsingLifestyle(new SingletonLifestyle());
                o.Export<SingleHelper2>().As<SingleHelper2>().UsingLifestyle(new SingletonLifestyle());
                o.Export<SingleHelper3>().As<SingleHelper3>().UsingLifestyle(new SingletonLifestyle());
                o.Export<SingleService1>().As<SingleService1>().UsingLifestyle(new SingletonLifestyle());
                o.Export<SingleService2>().As<SingleService2>().UsingLifestyle(new SingletonLifestyle());
                o.Export<SingleService3>().As<SingleService3>().UsingLifestyle(new SingletonLifestyle());
                o.Export<SingleService4>().As<SingleService4>().UsingLifestyle(new SingletonLifestyle());
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

        //[Benchmark]
        //public void IoCServiceContainerFrozenStartup()
        //{
        //    var resolver = Creators.CreateIoCServiceContainer();
        //    resolver.Frozen();
        //    var userService = (IUserService1?)resolver.Resolve(typeof(IUserService1));
        //    var singleService = (SingleService1?)resolver.Resolve(typeof(SingleService1));
        //}

        //[Benchmark]
        //public void IoCServiceContainerOptimizeStartup()
        //{
        //    var resolver = Creators.CreateIoCServiceContainer();
        //    resolver.Optimize();
        //    var userService = (IUserService1?)resolver.Resolve(typeof(IUserService1));
        //    var singleService = (SingleService1?)resolver.Resolve(typeof(SingleService1));
        //}

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
        //private readonly BenchmarkIoCServiceContainer _iocServiceContainerFrozen;
        //private readonly BenchmarkIoCServiceContainer _iocServiceContainerOptimize;
        private readonly ServiceProvider _serviceProvider;

        public IoCRuntimeBenchmark()
        {
            _grace = Creators.CreateGrace();
            _serviceProvider = Creators.CreateMicrosoft();
            _zeroIoCContainer = Creators.CreateZeroIoC();
            _iocServiceContainer = Creators.CreateIoCServiceContainer();

            //_iocServiceContainerFrozen = Creators.CreateIoCServiceContainer();
            //_iocServiceContainerFrozen.Frozen();

            //_iocServiceContainerOptimize = Creators.CreateIoCServiceContainer();
            //_iocServiceContainerOptimize.Optimize();
        }

        private static readonly IEnumerable<Type> RandomUserServices = [typeof(IUserService1), typeof(IUserService2), typeof(IUserService3), typeof(IUserService4), typeof(IUserService5), typeof(IUserService6), typeof(IUserService7), typeof(IUserService8), typeof(IUserService9), typeof(IUserService10), typeof(IUserService11)];

        private static readonly IEnumerable<Type> RandomSingleServices = [typeof(SingleService1), typeof(SingleService2), typeof(SingleService3), typeof(SingleService4)];

        [Benchmark]
        public void MicrosoftTransient()
        {
            foreach (var userServiceType in RandomUserServices)
            {
                _ = _serviceProvider.GetService(userServiceType);
            }
        }

        [Benchmark]
        public void ZeroIoCTransient()
        {
            foreach (var userServiceType in RandomUserServices)
            {
                _ = _zeroIoCContainer.Resolve(userServiceType);
            }
        }

        [Benchmark]
        public void IoCServiceContainerTransient()
        {
            foreach (var userServiceType in RandomUserServices)
            {
                _ = _iocServiceContainer.Resolve(userServiceType);
            }
        }

        //[Benchmark]
        //public void IoCServiceContainerOptimizeTransient()
        //{
        //    foreach (var userServiceType in RandomUserServices)
        //    {
        //        _ = _iocServiceContainerOptimize.Resolve(userServiceType);
        //    }
        //}

        //[Benchmark]
        //public void IoCServiceContainerFrozenTransient()
        //{
        //    foreach (var userServiceType in RandomUserServices)
        //    {
        //        _ = _iocServiceContainerFrozen.Resolve(userServiceType);
        //    }
        //}

        [Benchmark]
        public void GraceTransient()
        {
            foreach (var userServiceType in RandomUserServices)
            {
                _ = _grace.Locate(userServiceType);
            }
        }

        [Benchmark]
        public void MicrosoftSingleton()
        {
            foreach (var singleServiceType in RandomSingleServices)
            {
                _ = _serviceProvider.GetService(singleServiceType);
            }
        }

        [Benchmark]
        public void ZeroIoCSingleton()
        {
            foreach (var singleServiceType in RandomSingleServices)
            {
                _ = _zeroIoCContainer.Resolve(singleServiceType);
            }
        }

        [Benchmark]
        public void IoCServiceContainerSingleton()
        {
            foreach (var singleServiceType in RandomSingleServices)
            {
                _ = _iocServiceContainer.Resolve(singleServiceType);
            }
        }

        //[Benchmark]
        //public void IoCServiceContainerOptimizeSingleton()
        //{
        //    foreach (var singleServiceType in RandomSingleServices)
        //    {
        //        _ = _iocServiceContainerOptimize.Resolve(singleServiceType);
        //    }
        //}

        //[Benchmark]
        //public void IoCServiceContainerFrozenSingleton()
        //{
        //    foreach (var singleServiceType in RandomSingleServices)
        //    {
        //        _ = _iocServiceContainerFrozen.Resolve(singleServiceType);
        //    }
        //}

        [Benchmark]
        public void GraceSingleton()
        {
            foreach (var singleServiceType in RandomSingleServices)
            {
                _ = _grace.Locate(singleServiceType);
            }
        }
    }
}
