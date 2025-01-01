using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.CodeAnalysis;
using Xunit;
using Xunit.Abstractions;

namespace Nivaes.IoC.UnitTest;

public partial class MergeContainerTest
{
    public partial class TestContainer1 : IoCServiceContainer
    {
        protected override void Bootstrap(IIoCServiceContainerBootstrapper bootstrapper)
        {
            bootstrapper.AddSingleton<Helper1>();
            bootstrapper.AddSingleton<Helper2>();
            bootstrapper.AddSingleton<Helper3>();
            bootstrapper.AddScoped<IUserService1, UserService1>();
        }
    }

    public partial class TestContainer2 : IoCServiceContainer
    {
        protected override void Bootstrap(IIoCServiceContainerBootstrapper bootstrapper)
        {
            bootstrapper.AddSingleton<UserService2>();
            bootstrapper.AddSingleton<IUserService3, UserService3>();
        }
    }

    private readonly ITestOutputHelper output;

    public MergeContainerTest(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Fact]
    public void ResolveSimpleTransient()
    {
        var container1 = new TestContainer1();
        var container2 = new TestContainer2();
        container1.AddInstance<IUserService1>(new UserService1(new Helper1(new Helper2(new Helper3()))));
        container1.Merge(container2);


        var userService1_1 = container1.Resolve<IUserService1>();
        userService1_1.Should().NotBeNull();
        userService1_1!.PrintMessage();
        output.WriteLine($"{userService1_1.Id}");

        var userService1_2 = container1.Resolve<IUserService1>();
        userService1_2.Should().NotBeNull();
        userService1_2!.PrintMessage();
        output.WriteLine($"{userService1_2.Id}");

        var userService1_3 = container1.Resolve<IUserService1>();
        userService1_3.Should().NotBeNull();
        userService1_3!.PrintMessage();
        output.WriteLine($"{userService1_3.Id}");

        var userService3_1 = container1.Resolve<IUserService3>();
        userService3_1.Should().NotBeNull();
        userService3_1!.PrintMessage();
        output.WriteLine($"{userService3_1.Id}");
    }

    [Fact]
    public void ResolveScopeTransient()
    {
        var container1 = new TestContainer1();
        var container2 = new TestContainer2();
        container2.Merge(container1);
        container2.AddInstance<IUserService1>(new UserService1(new Helper1(new Helper2(new Helper3()))));

        var userService1_1 = container2.Resolve<IUserService1>();
        userService1_1.Should().NotBeNull();
        userService1_1!.PrintMessage();
        output.WriteLine($"{userService1_1.Id}");

        var userService1_2 = container2.Resolve<IUserService1>();
        userService1_2.Should().NotBeNull();
        userService1_2!.PrintMessage();
        output.WriteLine($"{userService1_2.Id}");

        var userService1_3 = container2.Resolve<IUserService1>();
        userService1_3.Should().NotBeNull();
        userService1_3!.PrintMessage();
        output.WriteLine($"{userService1_3.Id}");

        var userService3_1 = container2.Resolve<IUserService3>();
        userService3_1.Should().NotBeNull();
        userService3_1!.PrintMessage();
        output.WriteLine($"{userService3_1.Id}");
    }

    //[Fact]
    //public void ResolveScopedTransient02()
    //{
    //    var container = new TestContainer1();
    //    container.AddDelegate<IUserService1>((container) =>
    //        {
    //            var helper3 = container.Resolve<Helper3>();
    //            helper3.Should().NotBeNull();
    //            return new UserService1(new Helper1(new Helper2(helper3!)));
    //        }
    //    );
    //    //container.Frozen();

    //    var userService1_1 = container.Resolve<IUserService1>();
    //    userService1_1.Should().NotBeNull();
    //    userService1_1!.PrintMessage();
    //    output.WriteLine($"{userService1_1.Id}");

    //    var userService1_2 = container.Resolve<IUserService1>();
    //    userService1_2.Should().NotBeNull();
    //    userService1_2!.PrintMessage();
    //    output.WriteLine($"{userService1_2.Id}");

    //    var userService1_3 = container.Resolve<IUserService1>();
    //    userService1_3.Should().NotBeNull();
    //    userService1_3!.PrintMessage();
    //    output.WriteLine($"{userService1_3.Id}");
    //}

    //[Fact]
    //public void ResolveMultyScopedTransient()
    //{
    //    var container = new TestContainer1();
    //    container.AddInstance<IUserService1>(new UserService1(new Helper1(new Helper2(new Helper3()))));
    //    //container.Frozen();

    //    var scope1 = container.CreateScope();
    //    scope1.Should().NotBeNull();

    //    var userService1_1 = scope1.Resolve<IUserService1>();
    //    userService1_1.Should().NotBeNull();
    //    userService1_1!.PrintMessage();
    //    output.WriteLine($"{userService1_1.Id}");

    //    var userService1_2 = scope1.Resolve<IUserService1>();
    //    userService1_2.Should().NotBeNull();
    //    userService1_2!.PrintMessage();
    //    output.WriteLine($"{userService1_2.Id}");

    //    var scope2 = container.CreateScope();
    //    scope2.Should().NotBeNull();

    //    var userService1_3 = scope2.Resolve<IUserService1>();
    //    userService1_3.Should().NotBeNull();
    //    userService1_3!.PrintMessage();
    //    output.WriteLine($"{userService1_3.Id}");
    //}
}
