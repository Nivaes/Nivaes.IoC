using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.CodeAnalysis;
using Xunit;
using Xunit.Abstractions;

namespace Nivaes.IoC.UnitTest;

public partial class ResolveOptimizeContainerTest
{
    public partial class TestContainer : IoCServiceContainer
    {
        protected override void Bootstrap(IIoCServiceContainerBootstrapper bootstrapper)
        {
            bootstrapper.AddSingleton<Helper1>();
            bootstrapper.AddSingleton<Helper2>();
            bootstrapper.AddSingleton<Helper3>();
            bootstrapper.AddTransient<IUserService1, UserService1>();
            bootstrapper.AddTransient<IUserService2, UserService2>();
            bootstrapper.AddTransient<IUserService3, UserService3>();
        }
    }

    private readonly ITestOutputHelper output;

    public ResolveOptimizeContainerTest(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Fact]
    public void ResolveSimpleTransient1()
    {
        var container = new TestContainer();
        container.Optimize();

        var userService1_1 = container.Resolve<IUserService1>();
        userService1_1.Should().NotBeNull();
        userService1_1!.PrintMessage();
        output.WriteLine($"{userService1_1.Id}");

        var userService1_2 = container.Resolve<IUserService1>();
        userService1_2.Should().NotBeNull();
        userService1_2!.PrintMessage();
        output.WriteLine($"{userService1_2.Id}");

        var userService1_3 = container.Resolve<IUserService1>();
        userService1_3.Should().NotBeNull();
        userService1_3!.PrintMessage();
        output.WriteLine($"{userService1_3.Id}");
    }
}
