using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Xunit;

namespace Nivaes.IoC.UnitTest;

public partial class MyContainer : IoCContainer
{
    protected override void Bootstrap(IIoCContainerBootstrapper bootstrapper)
    {
        bootstrapper.AddSingleton<Helper1>();
        bootstrapper.AddSingleton<Helper2>();
        bootstrapper.AddSingleton<Helper3>();
        bootstrapper.AddTransient<IUserService, UserService>();
    }
}

public class BasicContainerTest
{
    [Fact]
    public void CanResolveSimpleSingleton()
    {
        var container = new MyContainer();
        var userService = container.Resolve<IUserService>();

        Assert.NotNull(userService);

        userService.PrintMessage();
    }
}
