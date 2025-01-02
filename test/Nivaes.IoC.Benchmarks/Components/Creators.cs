using Grace.DependencyInjection;
using Grace.DependencyInjection.Lifestyle;
using Microsoft.Extensions.DependencyInjection;
using Nivaes.IoC.Benchmarks.Components;

namespace Nivaes.IoC.Benchmarks
{
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
}
