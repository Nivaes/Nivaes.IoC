using FluentAssertions;
using Nivaes.IoC.SourceGenerator.UnitTest.Data;
using Nivaes.IoC.SourceGenerator.UnitTest.Utils;
using Xunit;

namespace Nivaes.IoC.SourceGenerator.UnitTest;

public class SubClassContainerTest
{
    [Fact]
    public async Task FatherClassContainerSimpleSingleton()
    {
        var project = await TestProject.Project.ApplyToProgram(@"

        public interface IService
        {

        }

        public class Service : IService
        {

        }

        public partial class FatherClass
        {
            public partial class TestContainer : IoCServiceContainer
            {
                protected override void Bootstrap(IIoCServiceContainerBootstrapper bootstrapper)
                {
                    bootstrapper.AddSingleton<IService, Service>();
                }
            }
        }
");

        var newProject = await project.ApplyIoCGenerator();

        var assembly = await newProject.CompileToRealAssembly();
        var containerType = assembly.GetType("TestProject.FatherClass+TestContainer");
        containerType.Should().NotBeNull();
        var serviceType = assembly.GetType("TestProject.IService");
        serviceType.Should().NotBeNull();

        var container = (IIoCResolver?)Activator.CreateInstance(containerType!);
        var firstService = container!.Resolve(serviceType!);
        var secondService = container!.Resolve(serviceType!);

        Assert.True(firstService != null && secondService != null && firstService.Equals(secondService));
    }

    [Fact]
    public async Task GrandFatherClassContainerSimpleSingleton()
    {
        var project = await TestProject.Project.ApplyToProgram(@"

        public interface IService
        {

        }

        public class Service : IService
        {

        }

        public partial class GrandGrandFatherClass
        {
            public partial class GrandFatherClass
            {
                public partial class FatherClass
                {
                    public partial class TestContainer : IoCServiceContainer
                    {
                        protected override void Bootstrap(IIoCServiceContainerBootstrapper bootstrapper)
                        {
                            bootstrapper.AddSingleton<IService, Service>();
                        }
                    }
                }
            }
        }
");

        var newProject = await project.ApplyIoCGenerator();

        var assembly = await newProject.CompileToRealAssembly();
        var containerType = assembly.GetType("TestProject.GrandGrandFatherClass+GrandFatherClass+FatherClass+TestContainer");
        containerType.Should().NotBeNull();
        var serviceType = assembly.GetType("TestProject.IService");
        serviceType.Should().NotBeNull();

        var container = (IIoCResolver?)Activator.CreateInstance(containerType!);
        var firstService = container!.Resolve(serviceType!);
        var secondService = container!.Resolve(serviceType!);

        Assert.True(firstService != null && secondService != null && firstService.Equals(secondService));
    }

    [Fact]
    public async Task TwoFatherClassContainerSimpleSingleton()
    {
        var project = await TestProject.Project.ApplyToProgram(@"

        public interface IService
        {

        }

        public class Service : IService
        {

        }

        public partial class FatherClass1
        {
            public partial class TestContainer : IoCServiceContainer
            {
                protected override void Bootstrap(IIoCServiceContainerBootstrapper bootstrapper)
                {
                    bootstrapper.AddSingleton<IService, Service>();
                }
            }
        }

        public partial class FatherClass2
        {
            public partial class TestContainer : IoCServiceContainer
            {
                protected override void Bootstrap(IIoCServiceContainerBootstrapper bootstrapper)
                {
                    bootstrapper.AddSingleton<IService, Service>();
                }
            }
        }
");
        var newProject = await project.ApplyIoCGenerator();

        var assembly = await newProject.CompileToRealAssembly();
        var containerType1 = assembly.GetType("TestProject.FatherClass1+TestContainer");
        var containerType2 = assembly.GetType("TestProject.FatherClass2+TestContainer");
        var serviceType = assembly.GetType("TestProject.IService");

        containerType1.Should().NotBeNull();
        containerType2.Should().NotBeNull();

        var container1 = (IIoCResolver?)Activator.CreateInstance(containerType1!);
        var firstService1 = container1!.Resolve(serviceType!);
        var secondService1 = container1!.Resolve(serviceType!);

        Assert.True(firstService1 != null && secondService1 != null && firstService1.Equals(secondService1));

        var container2 = (IIoCResolver?)Activator.CreateInstance(containerType2!);
        var firstService2 = container2!.Resolve(serviceType!);
        var secondService2 = container2!.Resolve(serviceType!);

        Assert.True(firstService2 != null && secondService2 != null && firstService2.Equals(secondService2));
    }
}
