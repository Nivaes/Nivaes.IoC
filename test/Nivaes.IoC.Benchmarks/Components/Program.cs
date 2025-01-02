namespace Nivaes.IoC.Benchmarks
{
    using BenchmarkDotNet.Running;

    internal class Program
    {
        private static void Main(string[] args)
        {
            BenchmarkRunner.Run<IoCStartupBenchmark>();
            //BenchmarkRunner.Run<IoCRuntimeBenchmark>();
        }
    }
}
