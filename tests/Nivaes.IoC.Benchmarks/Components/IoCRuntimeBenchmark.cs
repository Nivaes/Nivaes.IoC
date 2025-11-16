namespace Benchmarks
{
    using System;
    using System.Collections.Generic;
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Order;
    using Grace.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection;
    using Nivaes.IoC;

    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    public class IoCRuntimeBenchmark
    {
        private readonly DependencyInjectionContainer _grace;
        private readonly ZeroContainer _zeroIoCContainer;
        private readonly BenchmarkIoCServiceContainer _iocServiceContainer;
        private readonly ServiceProvider _serviceProvider;

        public IoCRuntimeBenchmark()
        {
            _grace = Creators.CreateGrace();
            _serviceProvider = Creators.CreateMicrosoft();
            _zeroIoCContainer = Creators.CreateZeroIoC();
            _iocServiceContainer = Creators.CreateIoCServiceContainer();
        }

        private static readonly IEnumerable<Type> RandomUserServices = [typeof(IUserService1), typeof(IUserService2), typeof(IUserService3), typeof(IUserService24), typeof(IUserService5), typeof(IUserService16), typeof(IUserService7), typeof(IUserService28), typeof(IUserService9), typeof(IUserService13), typeof(IUserService13)];

        private static readonly IEnumerable<Type> RandomSingleServices = [typeof(SingleService1), typeof(SingleService2), typeof(SingleService7), typeof(SingleService17), typeof(SingleService27), typeof(SingleService1), typeof(SingleService30), typeof(SingleService15)];

        [Benchmark]
        public void MicrosoftTransient()
        {
            foreach (var userServiceType in RandomUserServices)
            {
                _ = _serviceProvider.GetService(userServiceType);
            }
        }

        [Benchmark]
        public void ZeroIoCTransient()
        {
            foreach (var userServiceType in RandomUserServices)
            {
                _ = _zeroIoCContainer.Resolve(userServiceType);
            }
        }

        [Benchmark]
        public void IoCServiceContainerTransient()
        {
            foreach (var userServiceType in RandomUserServices)
            {
                _ = _iocServiceContainer.Resolve(userServiceType);
            }
        }

        [Benchmark]
        public void GraceTransient()
        {
            foreach (var userServiceType in RandomUserServices)
            {
                _ = _grace.Locate(userServiceType);
            }
        }

        [Benchmark]
        public void MicrosoftSingleton()
        {
            foreach (var singleServiceType in RandomSingleServices)
            {
                _ = _serviceProvider.GetService(singleServiceType);
            }
        }

        [Benchmark]
        public void ZeroIoCSingleton()
        {
            foreach (var singleServiceType in RandomSingleServices)
            {
                _ = _zeroIoCContainer.Resolve(singleServiceType);
            }
        }

        [Benchmark]
        public void IoCServiceContainerSingleton()
        {
            foreach (var singleServiceType in RandomSingleServices)
            {
                _ = _iocServiceContainer.Resolve(singleServiceType);
            }
        }

        [Benchmark]
        public void GraceSingleton()
        {
            foreach (var singleServiceType in RandomSingleServices)
            {
                _ = _grace.Locate(singleServiceType);
            }
        }
    }
}
