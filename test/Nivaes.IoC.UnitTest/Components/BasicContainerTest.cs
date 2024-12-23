using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Xunit;
using Xunit.Abstractions;

namespace Nivaes.IoC.UnitTest;

public partial class MyContainer : IoCContainer
{
    protected override void Bootstrap(IIoCContainerBootstrapper bootstrapper)
    {
        bootstrapper.AddSingleton<Helper1>();
        bootstrapper.AddSingleton<Helper2>();
        bootstrapper.AddSingleton<Helper3>();
        bootstrapper.AddTransient<IUserService1, UserService1>();
        bootstrapper.AddTransient<IUserService2, UserService2>();
        bootstrapper.AddTransient<IUserService3, UserService3>();
    }
}

public class BasicContainerTest
{
    private readonly ITestOutputHelper output;

    public BasicContainerTest(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Fact]
    public void ResolveSimpleTransient()
    {
        var container = new MyContainer();
        var userService1_1 = container.Resolve<IUserService1>();

        Assert.NotNull(userService1_1);

        userService1_1.PrintMessage();
        output.WriteLine($"{userService1_1.Id}");

        var userService1_2 = container.Resolve<IUserService1>();

        Assert.NotNull(userService1_2);

        userService1_2.PrintMessage();
        output.WriteLine($"{userService1_2.Id}");

        var userService1_3 = container.Resolve<IUserService1>();

        Assert.NotNull(userService1_3);

        userService1_3.PrintMessage();
        output.WriteLine($"{userService1_3.Id}");
    }
}
