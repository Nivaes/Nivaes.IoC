using Xunit;

namespace Nivaes.IoC.SourceGenerator.UnitTest;

public class ScopedContainerTest
{
    [Fact]
    public async Task FailsWhenScopedServiceCreatedWithourScope()
    {
        var project = await TestProject.Project.ApplyToProgram(@"

        public interface IService
        {

        }

        public class Service : IService
        {

        }

        public partial class TestContainer : IoCServiceContainer
        {
            protected override void Bootstrap(IIoCServiceContainerBootstrapper bootstrapper)
            {
                bootstrapper.AddScoped<IService, Service>();
            }
        }
");

        var newProject = await project.ApplyIoCGenerator();

        var assembly = await newProject.CompileToRealAssembly();
        var containerType = assembly.GetType("TestProject.TestContainer");
        var serviceType = assembly.GetType("TestProject.IService");
        Assert.NotNull(containerType);
        Assert.NotNull(serviceType);

        var container = (IIoCResolver?)Activator.CreateInstance(containerType);
        Assert.NotNull(container);

        Assert.Throws<ScopedWithoutScopeException>(() => container.Resolve(serviceType));
    }

    [Fact]
    public async Task CanResolveSimpleScoped()
    {
        var project = await TestProject.Project.ApplyToProgram(@"

        public interface IService
        {

        }

        public class Service : IService
        {

        }

        public partial class TestContainer : IoCServiceContainer
        {
            protected override void Bootstrap(IIoCServiceContainerBootstrapper bootstrapper)
            {
                bootstrapper.AddScoped<IService, Service>();
            }
        }
");

        var newProject = await project.ApplyIoCGenerator();

        var assembly = await newProject.CompileToRealAssembly();
        var containerType = assembly.GetType("TestProject.TestContainer");
        var serviceType = assembly.GetType("TestProject.IService");
        Assert.NotNull(containerType);
        Assert.NotNull(serviceType);

        var container = (IIoCResolver?)Activator.CreateInstance(containerType);
        Assert.NotNull(container);

        var scoped = container.CreateScope();
        var scopedFirstService = scoped.Resolve(serviceType);
        var scopedSecondService = scoped.Resolve(serviceType);

        var scoped2 = container.CreateScope();
        var scopedFirstService2 = scoped2.Resolve(serviceType);
        var scopedSecondService2 = scoped2.Resolve(serviceType);

        Assert.True(scopedFirstService != null && scopedSecondService != null && scopedFirstService.Equals(scopedSecondService));
        Assert.True(scopedFirstService2 != null && scopedSecondService2 != null && scopedFirstService2.Equals(scopedSecondService2));
        Assert.True(!scopedFirstService.Equals(scopedFirstService2));
    }

    [Fact]
    public async Task ServicesWithinTheScopeIsDisposed()
    {
        var project = await TestProject.Project.ApplyToProgram(@"

        public class SingletonService : IDisposable
        {
            public bool Disposed { get; set; }

            public void Dispose()
            {
                Disposed = true;
            }
        }

        public class Service : IDisposable
        {
            public bool Disposed { get; set; }

            public void Dispose()
            {
                Disposed = true;
            }
        }

        public partial class TestContainer : IoCServiceContainer
        {
            protected override void Bootstrap(IIoCServiceContainerBootstrapper bootstrapper)
            {
                bootstrapper.AddScoped<Service>();
                bootstrapper.AddSingleton<SingletonService>();
            }
        }
");

        var newProject = await project.ApplyIoCGenerator();

        var assembly = await newProject.CompileToRealAssembly();
        var containerType = assembly.GetType("TestProject.TestContainer");
        var serviceType = assembly.GetType("TestProject.Service");
        var singletonServiceType = assembly.GetType("TestProject.SingletonService");
        Assert.NotNull(containerType);
        Assert.NotNull(serviceType);
        Assert.NotNull(singletonServiceType);

        var container = (IIoCResolver?)Activator.CreateInstance(containerType);
        Assert.NotNull(container);

        object? service = null;
        object? singletonService = null;
        using (var scoped = container.CreateScope())
        {
            service = scoped.Resolve(serviceType);
            Assert.NotNull(service);

            Assert.False((bool?)service.ReflectionGetValue("Disposed"));

            singletonService = scoped.Resolve(singletonServiceType);
            Assert.NotNull(singletonService);
            Assert.False((bool?)service.ReflectionGetValue("Disposed"));
        }

        Assert.True((bool?)service.ReflectionGetValue("Disposed"));
        Assert.False((bool?)singletonService.ReflectionGetValue("Disposed"));
    }
}
