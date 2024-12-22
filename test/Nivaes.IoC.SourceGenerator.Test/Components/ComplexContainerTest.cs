using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Nivaes.IoC.Tests.Data;
using Nivaes.IoC.Tests.Utils;

namespace Nivaes.IoC.Tests;

public class ComplexContainerTest
{
    [Fact]
    public async Task CanResolveNestedServices()
    {
        var project = await TestProject.Project.ApplyToProgram(@"

        public interface IRepository
        {

        }

        public class Repository : IRepository
        {
            
        }

        public interface IService
        {

        }

        public class Service : IService
        {
            public Service(IRepository repository)
            {

            }
        }

        public partial class TestContainer : IoCContainer
        {
            protected override void Bootstrap(IIoCContainerBootstrapper bootstrapper)
            {
                bootstrapper.AddTransient<IRepository, Repository>();
                bootstrapper.AddTransient<IService, Service>();
            }
        }
");

        var newProject = await project.ApplyIoCGenerator();

        var assembly = await newProject.CompileToRealAssembly();
        var containerType = assembly.GetType("TestProject.TestContainer");
        var serviceType = assembly.GetType("TestProject.IService");

        var container = (IIoCResolver)Activator.CreateInstance(containerType);
        var firstService = container.Resolve(serviceType);
        var secondService = container.Resolve(serviceType);

        Assert.True(firstService != null && secondService != null && !firstService.Equals(secondService));
    }

    [Fact]
    public async Task MergeMultipleContainers()
    {
        var project = await TestProject.Project.ApplyToProgram(@"

        public interface IRepository
        {

        }

        public class Repository : IRepository
        {
            
        }

        public interface IService
        {

        }

        public class Service : IService
        {
            public Service(IRepository repository)
            {

            }
        }

        public partial class ServiceContainer : IoCContainer
        {
            protected override void Bootstrap(IIoCContainerBootstrapper bootstrapper)
            {
                bootstrapper.AddSingleton<IService, Service>();
            }
        }

        public partial class RepositoryContainer : IoCContainer
        {
            protected override void Bootstrap(IIoCContainerBootstrapper bootstrapper)
            {
                bootstrapper.AddSingleton<IRepository, Repository>();
            }
        }
");

        var newProject = await project.ApplyIoCGenerator();

        var assembly = await newProject.CompileToRealAssembly();
        var serviceContainerType = assembly.GetType("TestProject.ServiceContainer");
        var repositoryContainerType = assembly.GetType("TestProject.RepositoryContainer");
        var serviceType = assembly.GetType("TestProject.IService");

        var serviceContainer = (IoCContainer)Activator.CreateInstance(serviceContainerType);
        var repositoryContainer = (IoCContainer)Activator.CreateInstance(repositoryContainerType);
        repositoryContainer.Merge(serviceContainer);

        var service = repositoryContainer.Resolve(serviceType);

        Assert.NotNull(service);
    }
        
    [Fact]
    public async Task CloneContainer()
    {
        var project = await TestProject.Project.ApplyToProgram(@"

        public interface IRepository
        {

        }

        public class Repository : IRepository
        {
            
        }

        public interface IService
        {

        }

        public class Service : IService
        {
            public Service(IRepository repository)
            {

            }
        }

        public partial class ServiceContainer : IoCContainer
        {
            protected override void Bootstrap(IIoCContainerBootstrapper bootstrapper)
            {
                bootstrapper.AddSingleton<IRepository, Repository>();
                bootstrapper.AddSingleton<IService, Service>();
            }
        }
");

        var newProject = await project.ApplyIoCGenerator();

        var assembly = await newProject.CompileToRealAssembly();
        var serviceContainerType = assembly.GetType("TestProject.ServiceContainer");
        var serviceType = assembly.GetType("TestProject.IService");

        var serviceContainer = (IoCContainer)Activator.CreateInstance(serviceContainerType);
        var serviceContainerCopy = serviceContainer.Clone();

        var service = serviceContainer.Resolve(serviceType);
        var serviceCopy = serviceContainerCopy.Resolve(serviceType);

        Assert.NotNull(service);
        Assert.NotNull(serviceCopy);
        Assert.NotSame(service, serviceCopy);
    }

    [Fact]
    public async Task AddDelegateEachTimeDifferent()
    {
        var project = await TestProject.Project.ApplyToProgram(@"

        public interface IService
        {

        }

        public class Service : IService
        {
            public string Id { get; } 
            public Service(string id)
            {
                Id = id;
            }
        }

        public partial class TestContainer : IoCContainer
        {
            protected override void Bootstrap(IIoCContainerBootstrapper bootstrapper)
            {
                bootstrapper.AddSingleton<IService, Service>();
            }
        }
");

        var newProject = await project.ApplyIoCGenerator();

        var assembly = await newProject.CompileToRealAssembly();
        var containerType = assembly.GetType("TestProject.TestContainer");

        var container = (IoCContainer)Activator.CreateInstance(containerType);
        container.AddDelegate(o => Guid.NewGuid().ToString());

        var service1 = container.Resolve(typeof(string));
        var service2 = container.Resolve(typeof(string));

        Assert.NotSame(service1, service2);
    }

    [Fact]
    public async Task AddInstance()
    {
        var project = await TestProject.Project.ApplyToProgram(@"

        public interface IService
        {

        }

        public class Service : IService
        {
            public string Id { get; } 
            public Service(string id)
            {
                Id = id;
            }
        }

        public partial class TestContainer : IoCContainer
        {
            protected override void Bootstrap(IIoCContainerBootstrapper bootstrapper)
            {
                bootstrapper.AddSingleton<IService, Service>();
            }
        }
");

        var newProject = await project.ApplyIoCGenerator();

        var assembly = await newProject.CompileToRealAssembly();
        var containerType = assembly.GetType("TestProject.TestContainer");
        var serviceType = assembly.GetType("TestProject.IService");

        var container = (IoCContainer)Activator.CreateInstance(containerType);
        container.AddInstance(Guid.NewGuid().ToString());
        var service = container.Resolve(serviceType);

        Assert.NotNull(service);
    }
        
    [Fact]
    public async Task ReplaceInstance()
    {
        var project = await TestProject.Project.ApplyToProgram(@"
        public partial class TestContainer : IoCContainer
        {
            protected override void Bootstrap(IIoCContainerBootstrapper bootstrapper)
            {
            }
        }
");
        var newProject = await project.ApplyIoCGenerator();

        var assembly = await newProject.CompileToRealAssembly();
        var containerType = assembly.GetType("TestProject.TestContainer");

        var container = (IoCContainer)Activator.CreateInstance(containerType);
            
        var guidValue = Guid.NewGuid();
        container.AddInstance(guidValue);
        var resolvedGuid = container.Resolve<Guid>();

        Assert.True(guidValue == resolvedGuid);
            
        var newGuid = Guid.NewGuid();
        container.ReplaceInstance(newGuid);
            
        Assert.False(newGuid == resolvedGuid);
    }
        
    [Fact]
    public async Task ReplaceDelegate()
    {
        var project = await TestProject.Project.ApplyToProgram(@"
        public partial class TestContainer : IoCContainer
        {
            protected override void Bootstrap(IIoCContainerBootstrapper bootstrapper)
            {
            }
        }
");
        var newProject = await project.ApplyIoCGenerator();

        var assembly = await newProject.CompileToRealAssembly();
        var containerType = assembly.GetType("TestProject.TestContainer");

        var container = (IoCContainer)Activator.CreateInstance(containerType);
            
        var guidValue = Guid.NewGuid();
        container.AddDelegate(o => guidValue, Reuse.Singleton);
        var resolvedGuid = container.Resolve<Guid>();

        Assert.True(guidValue == resolvedGuid);
            
        var newGuid = Guid.NewGuid();
        container.ReplaceDelegate(o => newGuid, Reuse.Singleton);
            
        Assert.False(newGuid == resolvedGuid);
    }
}
