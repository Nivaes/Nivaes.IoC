using Shouldly;

namespace Nivaes.IoC.UnitTest;

public partial class ScopeContainerTest
{
    public partial class TestContainer : IoCServiceContainer
    {
        protected override void Bootstrap(IIoCServiceContainerBootstrapper bootstrapper)
        {
            bootstrapper.AddSingleton<Helper1>();
            bootstrapper.AddSingleton<Helper2>();
            bootstrapper.AddSingleton<Helper3>();
            bootstrapper.AddScoped<IUserService1, UserService1>();
            bootstrapper.AddSingleton<UserService2>();
        }
    }

    private readonly ITestOutputHelper output;

    public ScopeContainerTest(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Fact]
    public void ResolveScopedTransient01()
    {
        var container = new TestContainer();
        container.AddInstance<IUserService1>(new UserService1(new Helper1(new Helper2(new Helper3()))));

        var userService1_1 = container.Resolve<IUserService1>();
        userService1_1.ShouldNotBeNull();
        userService1_1!.PrintMessage();
        output.WriteLine($"{userService1_1.Id}");

        var userService1_2 = container.Resolve<IUserService1>();
        userService1_2.ShouldNotBeNull();
        userService1_2!.PrintMessage();
        output.WriteLine($"{userService1_2.Id}");

        var userService1_3 = container.Resolve<IUserService1>();
        userService1_3.ShouldNotBeNull();
        userService1_3!.PrintMessage();
        output.WriteLine($"{userService1_3.Id}");
    }

    [Fact]
    public void ResolveScopedTransient02()
    {
        var container = new TestContainer();
        container.AddDelegate<IUserService1>((container) =>
            {
                var helper3 = container.Resolve<Helper3>();
                helper3.ShouldNotBeNull();
                return new UserService1(new Helper1(new Helper2(helper3!)));
            }
        );

        var userService1_1 = container.Resolve<IUserService1>();
        userService1_1.ShouldNotBeNull();
        userService1_1!.PrintMessage();
        output.WriteLine($"{userService1_1.Id}");

        var userService1_2 = container.Resolve<IUserService1>();
        userService1_2.ShouldNotBeNull();
        userService1_2!.PrintMessage();
        output.WriteLine($"{userService1_2.Id}");

        var userService1_3 = container.Resolve<IUserService1>();
        userService1_3.ShouldNotBeNull();
        userService1_3!.PrintMessage();
        output.WriteLine($"{userService1_3.Id}");
    }

    [Fact]
    public void ResolveMultyScopedTransient()
    {
        var container = new TestContainer();
        container.AddInstance<IUserService1>(new UserService1(new Helper1(new Helper2(new Helper3()))));

        var scope1 = container.CreateScope();
        scope1.ShouldNotBeNull();

        var userService1_1 = scope1.Resolve<IUserService1>();
        userService1_1.ShouldNotBeNull();
        userService1_1!.PrintMessage();
        output.WriteLine($"{userService1_1.Id}");

        var userService1_2 = scope1.Resolve<IUserService1>();
        userService1_2.ShouldNotBeNull();
        userService1_2!.PrintMessage();
        output.WriteLine($"{userService1_2.Id}");

        var scope2 = container.CreateScope();
        scope2.ShouldNotBeNull();

        var userService1_3 = scope2.Resolve<IUserService1>();
        userService1_3.ShouldNotBeNull();
        userService1_3!.PrintMessage();
        output.WriteLine($"{userService1_3.Id}");
    }
}
