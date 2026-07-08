namespace Nivaes.IoC.UnitTest;

public partial class ResolveDelegateContainerTest
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
        }
    }

    private readonly ITestOutputHelper output;

    public ResolveDelegateContainerTest(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Fact]
    public void ResolveSimpleTransient1()
    {
        var container = new TestContainer();
        container.AddDelegate<IUserService3>(
            (provider) =>
            {
                return new UserService3(provider.Resolve<Helper3>()!);
            });

        var userService3_1 = container.Resolve<IUserService3>();
        userService3_1.ShouldNotBeNull();
        userService3_1!.PrintMessage();
        output.WriteLine($"{userService3_1.Id}");

        var userService3_2 = container.Resolve<IUserService3>();
        userService3_2.ShouldNotBeNull();
        userService3_2!.PrintMessage();
        output.WriteLine($"{userService3_2.Id}");

        var userService3_3 = container.Resolve<IUserService3>();
        userService3_3.ShouldNotBeNull();
        userService3_3!.PrintMessage();
        output.WriteLine($"{userService3_3.Id}");
    }


    [Fact]
    public void ResolveDelegateNull()
    {
        var container = new TestContainer();
        container.AddDelegate<IUserService1>(
            (provider) =>
            {
                return null;
            });


        var userService = container.Resolve<IUserService1>();
        userService.ShouldBeNull();
    }

    [Fact]
    public void TryResolve()
    {
        var container = new TestContainer();

        var result = container.TryResolve<IUserService1>(out var userService);

        result.ShouldBeTrue();
        userService.ShouldNotBeNull();
    }

    [Fact]
    public void NotResolve()
    {
        var container = new TestContainer();

        var result = container.TryResolve<IUserService3>(out var userService);

        result.ShouldBeFalse();
        userService.ShouldBeNull();
    }
}
