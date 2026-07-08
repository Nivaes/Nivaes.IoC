namespace Nivaes.IoC.SourceGenerator.UnitTest;

public class OverridesTest
{
    [Fact]
    public async Task ConstructorOverridesWorks()
    {
        var project = await TestProject.Project.ApplyToProgram(@"

        public class Service
        {
            public string Value { get; set; }

            public Service(string value)
            {
                this.Value = value;
            }
        }

        public partial class TestContainer : IoCServiceContainer
        {
            protected override void Bootstrap(IIoCServiceContainerBootstrapper bootstrapper)
            {
                bootstrapper.AddTransient<Service>();
            }
        }
");

        var newProject = await project.ApplyIoCGenerator();

        var assembly = await newProject.CompileToRealAssembly();
        var containerType = assembly.GetType("TestProject.TestContainer")!;
        var serviceType = assembly.GetType("TestProject.Service");
        Assert.NotNull(serviceType);

        var container = (IoCServiceContainer)Activator.CreateInstance(containerType)!;

        var initialValue = "not override";
        container.AddInstance(initialValue);

        var service = container.Resolve(serviceType!, Overrides.Create().Constructor(("value", "override")));
        Assert.NotNull(service);

        var value = service?.ReflectionGetValue("Value");
        Assert.NotEqual(initialValue, value);
    }

    [Fact]
    public async Task DeepDependencyOverridesWorks()
    {
        var project = await TestProject.Project.ApplyToProgram(@"

        public class Repository
        {
            public string Value { get; set; }

            public Repository(string value)
            {
                this.Value = value;
            }
        }

        public class Service
        {
            public string Value { get; set; }
            public Repository Repository { get; set; }

            public Service(string value, Repository repository)
            {
                this.Value = value;
                this.Repository = repository;
            }
        }

        public partial class TestContainer : IoCServiceContainer
        {
            protected override void Bootstrap(IIoCServiceContainerBootstrapper bootstrapper)
            {
                bootstrapper.AddTransient<Service>();
                bootstrapper.AddTransient<Repository>();
            }
        }
");

        var newProject = await project.ApplyIoCGenerator();

        var assembly = await newProject.CompileToRealAssembly();
        var containerType = assembly.GetType("TestProject.TestContainer")!;
        var serviceType = assembly.GetType("TestProject.Service");
        Assert.NotNull(serviceType);

        var container = (IoCServiceContainer)Activator.CreateInstance(containerType)!;

        var initialValue = "not override";
        container.AddInstance(initialValue);

        var service = container.Resolve(serviceType!, Overrides.Create().Dependency<string>(() => "override"));
        Assert.NotNull(service);

        var value = service?.ReflectionGetValue("Value");
        var repositoryValue = service?.ReflectionGetValue("Repository")?.ReflectionGetValue("Value");

        Assert.NotEqual(initialValue, value);
        Assert.NotEqual(initialValue, repositoryValue);
    }
}
