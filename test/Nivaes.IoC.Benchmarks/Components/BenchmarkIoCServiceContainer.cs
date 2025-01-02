using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nivaes.IoC.Benchmarks
{
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
}
