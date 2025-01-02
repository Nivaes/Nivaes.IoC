namespace Benchmarks
{
    using BenchmarkDotNet.Running;
    using ZeroIoC;

    internal class Program
    {

        private static void Main(string[] args)
        {
            BenchmarkRunner.Run<IoCStartupBenchmark>();
            BenchmarkRunner.Run<IoCRuntimeBenchmark>();
        }
    }
}
