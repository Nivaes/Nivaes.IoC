using Xunit;

namespace Nivaes.IoC.SourceGenerator.UnitTest;

public class StatetmentsAnalyzerTest
{
    [Fact]
    public async Task IfNotAllowed()
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
                if(true)
                {
                    bootstrapper.AddTransient<IService, Service>();
                }
            }
        }
");

        var diagnostics = await project.ApplyAnalyzer(new IoCServiceContainerAnalyzer());

        Assert.True(diagnostics.Any(o => o.Id == Descriptors.StatementsNotAllowed.Id));

    }

    [Fact]
    public async Task WhileNotAllowed()
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
                while(true)
                {
                    bootstrapper.AddTransient<IService, Service>();
                }
            }
        }
");

        var diagnostics = await project.ApplyAnalyzer(new IoCServiceContainerAnalyzer());

        Assert.True(diagnostics.Any(o => o.Id == Descriptors.StatementsNotAllowed.Id));
    }

    [Fact]
    public async Task ForNotAllowed()
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
                for(int i = 0; i < 10; i++)
                {
                    bootstrapper.AddTransient<IService, Service>();
                }
            }
        }
");

        var diagnostics = await project.ApplyAnalyzer(new IoCServiceContainerAnalyzer());

        Assert.True(diagnostics.Any(o => o.Id == Descriptors.StatementsNotAllowed.Id));
    }
}
