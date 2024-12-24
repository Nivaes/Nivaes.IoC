using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using Nivaes.IoC.SourceGenerator.UnitTest.Data;
using Nivaes.IoC.SourceGenerator.UnitTest.Utils;

namespace Nivaes.IoC.SourceGenerator.UnitTest;

public class AddDelegateTest
{
    [Fact]
    public async Task AddSingletonAsDelegate()
    {
        var project = await TestProject.Project.ApplyToProgram(@"

        public partial class TestContainer : IoCServiceContainer
        {
            protected override void Bootstrap(IIoCServiceContainerBootstrapper bootstrapper)
            {
            }
        }
");

        var newProject = await project.ApplyIoCGenerator();

        var assembly = await newProject.CompileToRealAssembly();
        var containerType = assembly.GetType("TestProject.TestContainer")!;

        var container = (IoCServiceContainer)Activator.CreateInstance(containerType);
        container.AddDelegate(r => new MemoryStream(), Reuse.Singleton);
        using var scope1 = container.CreateScope();

        var stream1 = container.Resolve<MemoryStream>(); 
        var stream2 = container.Resolve<MemoryStream>(); 
        var stream3 = scope1.Resolve<MemoryStream>(); 

        Assert.Equal(stream1, stream2);
        Assert.Equal(stream1, stream3);
    }
        
    [Fact]
    public async Task AddScopedAsDelegate()
    {
        var project = await TestProject.Project.ApplyToProgram(@"

        public partial class TestContainer : IoCServiceContainer
        {
            protected override void Bootstrap(IIoCServiceContainerBootstrapper bootstrapper)
            {
            }
        }
");

        var newProject = await project.ApplyIoCGenerator();

        var assembly = await newProject.CompileToRealAssembly();
        var containerType = assembly.GetType("TestProject.TestContainer");

        var container = (IoCServiceContainer)Activator.CreateInstance(containerType);
        container.AddDelegate(r => new MemoryStream(), Reuse.Scoped);

        using var scope1 = container.CreateScope();
        using var scope2 = container.CreateScope();
            
        var stream1 = scope1.Resolve<MemoryStream>(); 
        var stream2 = scope1.Resolve<MemoryStream>(); 
        var stream3 = scope2.Resolve<MemoryStream>(); 

        Assert.Equal(stream1, stream2);
        Assert.NotEqual(stream1, stream3);
    }
        
    [Fact]
    public async Task AddTransientAsDelegate()
    {
        var project = await TestProject.Project.ApplyToProgram(@"

        public partial class TestContainer : IoCServiceContainer
        {
            protected override void Bootstrap(IIoCServiceContainerBootstrapper bootstrapper)
            {
            }
        }
");

        var newProject = await project.ApplyIoCGenerator();

        var assembly = await newProject.CompileToRealAssembly();
        var containerType = assembly.GetType("TestProject.TestContainer");

        var container = (IoCServiceContainer)Activator.CreateInstance(containerType);
        container.AddDelegate(r => new MemoryStream(), Reuse.Transient);

        using var scope1 = container.CreateScope();
        using var scope2 = container.CreateScope();
            
        var stream1 = container.Resolve<MemoryStream>(); 
        var stream2 = scope1.Resolve<MemoryStream>(); 
        var stream3 = scope2.Resolve<MemoryStream>(); 

        Assert.NotEqual(stream1, stream2);
        Assert.NotEqual(stream2, stream3);
        Assert.NotEqual(stream1, stream3);
    }
}
