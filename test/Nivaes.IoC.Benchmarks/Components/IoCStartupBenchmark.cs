namespace Benchmarks
{
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Order;

    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    public class IoCStartupBenchmark
    {
        [Benchmark]
        public void MicrosoftStartup()
        {
            var resolver = Creators.CreateMicrosoft();
            var userService = (IUserService1?)resolver.GetService(typeof(IUserService1));
            var singleService = (SingleService1?)resolver.GetService(typeof(SingleService1));
        }

        [Benchmark]
        public void ZeroIoCStartup()
        {
            var resolver = Creators.CreateZeroIoC();
            var userService = (IUserService1?)resolver.Resolve(typeof(IUserService1));
            var singleService = (SingleService1?)resolver.Resolve(typeof(SingleService1));
        }

        [Benchmark]
        public void IoCServiceContainerStartup()
        {
            var resolver = Creators.CreateIoCServiceContainer();
            var userService = (IUserService1?)resolver.Resolve(typeof(IUserService1));
            var singleService = (SingleService1?)resolver.Resolve(typeof(SingleService1));
        }

        [Benchmark]
        public void GraceStartup()
        {
            var resolver = Creators.CreateGrace();
            var userService = (IUserService1?)resolver.Locate(typeof(IUserService1));
            var singleService = (SingleService1?)resolver.Locate(typeof(SingleService1));
        }
    }
}
