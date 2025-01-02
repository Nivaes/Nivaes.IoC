namespace Benchmarks
{
    using Grace.DependencyInjection;
    using Grace.DependencyInjection.Lifestyle;
    using Microsoft.Extensions.DependencyInjection;

    public class Creators
    {
        public static ZeroContainer CreateZeroIoC()
        {
            return new ZeroContainer();
        }

        public static BenchmarkIoCServiceContainer CreateIoCServiceContainer()
        {
            return new BenchmarkIoCServiceContainer();
        }

        public static ServiceProvider CreateMicrosoft()
        {
            var services = new ServiceCollection();
            #region Helper
            services.AddTransient<Helper1>();
            services.AddTransient<Helper2>();
            services.AddTransient<Helper3>();
            services.AddTransient<Helper4>();
            services.AddTransient<Helper5>();
            services.AddTransient<Helper6>();
            services.AddTransient<Helper7>();
            services.AddTransient<Helper8>();
            services.AddTransient<Helper9>();
            services.AddTransient<Helper10>();
            services.AddTransient<Helper21>();
            services.AddTransient<Helper22>();
            services.AddTransient<Helper23>();
            services.AddTransient<Helper24>();
            services.AddTransient<Helper25>();
            services.AddTransient<Helper26>();
            services.AddTransient<Helper27>();
            services.AddTransient<Helper28>();
            services.AddTransient<Helper29>();
            services.AddTransient<Helper30>();
            services.AddTransient<Helper31>();
            services.AddTransient<Helper32>();
            services.AddTransient<Helper33>();
            services.AddTransient<Helper34>();
            services.AddTransient<Helper35>();
            services.AddTransient<Helper36>();
            services.AddTransient<Helper37>();
            services.AddTransient<Helper38>();
            services.AddTransient<Helper39>();
            services.AddTransient<Helper40>();
            services.AddTransient<Helper41>();
            services.AddTransient<Helper42>();
            services.AddTransient<Helper43>();
            services.AddTransient<Helper44>();
            services.AddTransient<Helper45>();
            services.AddTransient<Helper46>();
            services.AddTransient<Helper47>();
            services.AddTransient<Helper48>();
            services.AddTransient<Helper49>();
            services.AddTransient<Helper50>();
            services.AddTransient<Helper51>();
            services.AddTransient<Helper52>();
            services.AddTransient<Helper53>();
            services.AddTransient<Helper54>();
            services.AddTransient<Helper55>();
            services.AddTransient<Helper56>();
            services.AddTransient<Helper57>();
            services.AddTransient<Helper58>();
            services.AddTransient<Helper59>();
            services.AddTransient<Helper60>();
            #endregion

            #region UserService
            services.AddTransient<IUserService1, UserService1>();
            services.AddTransient<IUserService2, UserService2>();
            services.AddTransient<IUserService3, UserService3>();
            services.AddTransient<IUserService4, UserService4>();
            services.AddTransient<IUserService5, UserService5>();
            services.AddTransient<IUserService6, UserService6>();
            services.AddTransient<IUserService7, UserService7>();
            services.AddTransient<IUserService8, UserService8>();
            services.AddTransient<IUserService9, UserService9>();
            services.AddTransient<IUserService10, UserService10>();
            services.AddTransient<IUserService12, UserService12>();
            services.AddTransient<IUserService13, UserService13>();
            services.AddTransient<IUserService14, UserService14>();
            services.AddTransient<IUserService15, UserService15>();
            services.AddTransient<IUserService16, UserService16>();
            services.AddTransient<IUserService17, UserService17>();
            services.AddTransient<IUserService18, UserService18>();
            services.AddTransient<IUserService19, UserService19>();
            services.AddTransient<IUserService20, UserService20>();
            services.AddTransient<IUserService21, UserService21>();
            services.AddTransient<IUserService22, UserService22>();
            services.AddTransient<IUserService23, UserService23>();
            services.AddTransient<IUserService24, UserService24>();
            services.AddTransient<IUserService25, UserService25>();
            services.AddTransient<IUserService26, UserService26>();
            services.AddTransient<IUserService27, UserService27>();
            services.AddTransient<IUserService28, UserService28>();
            services.AddTransient<IUserService29, UserService29>();
            services.AddTransient<IUserService30, UserService30>();
            #endregion

            #region SingleHelper
            services.AddSingleton<SingleHelper1>();
            services.AddSingleton<SingleHelper2>();
            services.AddSingleton<SingleHelper3>();
            services.AddSingleton<SingleHelper4>();
            services.AddSingleton<SingleHelper5>();
            services.AddSingleton<SingleHelper6>();
            services.AddSingleton<SingleHelper7>();
            services.AddSingleton<SingleHelper8>();
            services.AddSingleton<SingleHelper9>();
            services.AddSingleton<SingleHelper10>();
            services.AddSingleton<SingleHelper11>();
            services.AddSingleton<SingleHelper12>();
            services.AddSingleton<SingleHelper13>();
            services.AddSingleton<SingleHelper14>();
            services.AddSingleton<SingleHelper15>();
            services.AddSingleton<SingleHelper16>();
            services.AddSingleton<SingleHelper17>();
            services.AddSingleton<SingleHelper18>();
            services.AddSingleton<SingleHelper19>();
            services.AddSingleton<SingleHelper20>();
            services.AddSingleton<SingleHelper21>();
            services.AddSingleton<SingleHelper22>();
            services.AddSingleton<SingleHelper23>();
            services.AddSingleton<SingleHelper24>();
            services.AddSingleton<SingleHelper25>();
            services.AddSingleton<SingleHelper26>();
            services.AddSingleton<SingleHelper27>();
            services.AddSingleton<SingleHelper28>();
            services.AddSingleton<SingleHelper29>();
            services.AddSingleton<SingleHelper30>();
            #endregion

            #region SingleService
            services.AddSingleton<SingleService1>();
            services.AddSingleton<SingleService2>();
            services.AddSingleton<SingleService3>();
            services.AddSingleton<SingleService4>();
            services.AddSingleton<SingleService5>();
            services.AddSingleton<SingleService6>();
            services.AddSingleton<SingleService7>();
            services.AddSingleton<SingleService8>();
            services.AddSingleton<SingleService9>();
            services.AddSingleton<SingleService10>();
            services.AddSingleton<SingleService11>();
            services.AddSingleton<SingleService12>();
            services.AddSingleton<SingleService13>();
            services.AddSingleton<SingleService14>();
            services.AddSingleton<SingleService15>();
            services.AddSingleton<SingleService16>();
            services.AddSingleton<SingleService17>();
            services.AddSingleton<SingleService18>();
            services.AddSingleton<SingleService19>();
            services.AddSingleton<SingleService20>();
            services.AddSingleton<SingleService21>();
            services.AddSingleton<SingleService22>();
            services.AddSingleton<SingleService23>();
            services.AddSingleton<SingleService24>();
            services.AddSingleton<SingleService25>();
            services.AddSingleton<SingleService26>();
            services.AddSingleton<SingleService27>();
            services.AddSingleton<SingleService28>();
            services.AddSingleton<SingleService29>();
            services.AddSingleton<SingleService30>();
            #endregion

            return services.BuildServiceProvider();
        }

        public static DependencyInjectionContainer CreateGrace()
        {
            var grace = new DependencyInjectionContainer();
            grace.Configure(o =>
            {
                #region Helper
                o.Export<Helper1>().As<Helper1>();
                o.Export<Helper2>().As<Helper2>();
                o.Export<Helper3>().As<Helper3>();
                o.Export<Helper4>().As<Helper4>();
                o.Export<Helper5>().As<Helper5>();
                o.Export<Helper6>().As<Helper6>();
                o.Export<Helper7>().As<Helper7>();
                o.Export<Helper8>().As<Helper8>();
                o.Export<Helper9>().As<Helper9>();
                o.Export<Helper10>().As<Helper10>();
                o.Export<Helper11>().As<Helper11>();
                o.Export<Helper12>().As<Helper12>();
                o.Export<Helper13>().As<Helper13>();
                o.Export<Helper14>().As<Helper14>();
                o.Export<Helper15>().As<Helper15>();
                o.Export<Helper16>().As<Helper16>();
                o.Export<Helper17>().As<Helper17>();
                o.Export<Helper18>().As<Helper18>();
                o.Export<Helper19>().As<Helper19>();
                o.Export<Helper20>().As<Helper20>();
                o.Export<Helper21>().As<Helper21>();
                o.Export<Helper22>().As<Helper22>();
                o.Export<Helper23>().As<Helper23>();
                o.Export<Helper24>().As<Helper24>();
                o.Export<Helper25>().As<Helper25>();
                o.Export<Helper26>().As<Helper26>();
                o.Export<Helper27>().As<Helper27>();
                o.Export<Helper28>().As<Helper28>();
                o.Export<Helper29>().As<Helper29>();
                o.Export<Helper30>().As<Helper30>();
                o.Export<Helper31>().As<Helper31>();
                o.Export<Helper32>().As<Helper32>();
                o.Export<Helper33>().As<Helper33>();
                o.Export<Helper34>().As<Helper34>();
                o.Export<Helper35>().As<Helper35>();
                o.Export<Helper36>().As<Helper36>();
                o.Export<Helper37>().As<Helper37>();
                o.Export<Helper38>().As<Helper38>();
                o.Export<Helper39>().As<Helper39>();
                o.Export<Helper40>().As<Helper40>();
                o.Export<Helper41>().As<Helper41>();
                o.Export<Helper42>().As<Helper42>();
                o.Export<Helper43>().As<Helper43>();
                o.Export<Helper44>().As<Helper44>();
                o.Export<Helper45>().As<Helper45>();
                o.Export<Helper46>().As<Helper46>();
                o.Export<Helper47>().As<Helper47>();
                o.Export<Helper48>().As<Helper48>();
                o.Export<Helper49>().As<Helper49>();
                o.Export<Helper50>().As<Helper50>();
                o.Export<Helper51>().As<Helper51>();
                o.Export<Helper52>().As<Helper52>();
                o.Export<Helper53>().As<Helper53>();
                o.Export<Helper54>().As<Helper54>();
                o.Export<Helper55>().As<Helper55>();
                o.Export<Helper56>().As<Helper56>();
                o.Export<Helper57>().As<Helper57>();
                o.Export<Helper58>().As<Helper58>();
                o.Export<Helper59>().As<Helper59>();
                o.Export<Helper60>().As<Helper60>();
                #endregion

                #region UserService
                o.Export<UserService1>().As<IUserService1>();
                o.Export<UserService2>().As<IUserService2>();
                o.Export<UserService3>().As<IUserService3>();
                o.Export<UserService4>().As<IUserService4>();
                o.Export<UserService5>().As<IUserService5>();
                o.Export<UserService6>().As<IUserService6>();
                o.Export<UserService7>().As<IUserService7>();
                o.Export<UserService8>().As<IUserService8>();
                o.Export<UserService9>().As<IUserService9>();
                o.Export<UserService10>().As<IUserService10>();
                o.Export<UserService11>().As<IUserService11>();
                o.Export<UserService12>().As<IUserService12>();
                o.Export<UserService13>().As<IUserService13>();
                o.Export<UserService14>().As<IUserService14>();
                o.Export<UserService15>().As<IUserService15>();
                o.Export<UserService16>().As<IUserService16>();
                o.Export<UserService17>().As<IUserService17>();
                o.Export<UserService18>().As<IUserService18>();
                o.Export<UserService19>().As<IUserService19>();
                o.Export<UserService21>().As<IUserService21>();
                o.Export<UserService22>().As<IUserService22>();
                o.Export<UserService23>().As<IUserService23>();
                o.Export<UserService24>().As<IUserService24>();
                o.Export<UserService25>().As<IUserService25>();
                o.Export<UserService26>().As<IUserService26>();
                o.Export<UserService27>().As<IUserService27>();
                o.Export<UserService28>().As<IUserService28>();
                o.Export<UserService29>().As<IUserService29>();
                o.Export<UserService30>().As<IUserService30>();
                #endregion

                #region SingleHelper
                o.Export<SingleHelper1>().As<SingleHelper1>().UsingLifestyle(new SingletonLifestyle());
                o.Export<SingleHelper2>().As<SingleHelper2>().UsingLifestyle(new SingletonLifestyle());
                o.Export<SingleHelper3>().As<SingleHelper3>().UsingLifestyle(new SingletonLifestyle());
                o.Export<SingleHelper4>().As<SingleHelper4>().UsingLifestyle(new SingletonLifestyle());
                o.Export<SingleHelper5>().As<SingleHelper5>().UsingLifestyle(new SingletonLifestyle());
                o.Export<SingleHelper6>().As<SingleHelper6>().UsingLifestyle(new SingletonLifestyle());
                o.Export<SingleHelper7>().As<SingleHelper7>().UsingLifestyle(new SingletonLifestyle());
                o.Export<SingleHelper8>().As<SingleHelper8>().UsingLifestyle(new SingletonLifestyle());
                o.Export<SingleHelper9>().As<SingleHelper9>().UsingLifestyle(new SingletonLifestyle());
                o.Export<SingleHelper10>().As<SingleHelper10>().UsingLifestyle(new SingletonLifestyle());
                o.Export<SingleHelper11>().As<SingleHelper11>().UsingLifestyle(new SingletonLifestyle());
                o.Export<SingleHelper12>().As<SingleHelper12>().UsingLifestyle(new SingletonLifestyle());
                o.Export<SingleHelper13>().As<SingleHelper13>().UsingLifestyle(new SingletonLifestyle());
                o.Export<SingleHelper14>().As<SingleHelper14>().UsingLifestyle(new SingletonLifestyle());
                o.Export<SingleHelper15>().As<SingleHelper15>().UsingLifestyle(new SingletonLifestyle());
                o.Export<SingleHelper16>().As<SingleHelper16>().UsingLifestyle(new SingletonLifestyle());
                o.Export<SingleHelper17>().As<SingleHelper17>().UsingLifestyle(new SingletonLifestyle());
                o.Export<SingleHelper18>().As<SingleHelper18>().UsingLifestyle(new SingletonLifestyle());
                o.Export<SingleHelper19>().As<SingleHelper19>().UsingLifestyle(new SingletonLifestyle());
                o.Export<SingleHelper20>().As<SingleHelper20>().UsingLifestyle(new SingletonLifestyle());
                o.Export<SingleHelper21>().As<SingleHelper21>().UsingLifestyle(new SingletonLifestyle());
                o.Export<SingleHelper22>().As<SingleHelper22>().UsingLifestyle(new SingletonLifestyle());
                o.Export<SingleHelper23>().As<SingleHelper23>().UsingLifestyle(new SingletonLifestyle());
                o.Export<SingleHelper24>().As<SingleHelper24>().UsingLifestyle(new SingletonLifestyle());
                o.Export<SingleHelper25>().As<SingleHelper25>().UsingLifestyle(new SingletonLifestyle());
                o.Export<SingleHelper26>().As<SingleHelper26>().UsingLifestyle(new SingletonLifestyle());
                o.Export<SingleHelper27>().As<SingleHelper27>().UsingLifestyle(new SingletonLifestyle());
                o.Export<SingleHelper28>().As<SingleHelper28>().UsingLifestyle(new SingletonLifestyle());
                o.Export<SingleHelper29>().As<SingleHelper29>().UsingLifestyle(new SingletonLifestyle());
                o.Export<SingleHelper30>().As<SingleHelper30>().UsingLifestyle(new SingletonLifestyle());
                #endregion

                #region SingleService
                o.Export<SingleService1>().As<SingleService1>().UsingLifestyle(new SingletonLifestyle());
                o.Export<SingleService2>().As<SingleService2>().UsingLifestyle(new SingletonLifestyle());
                o.Export<SingleService3>().As<SingleService3>().UsingLifestyle(new SingletonLifestyle());
                o.Export<SingleService4>().As<SingleService4>().UsingLifestyle(new SingletonLifestyle());
                o.Export<SingleService5>().As<SingleService5>().UsingLifestyle(new SingletonLifestyle());
                o.Export<SingleService6>().As<SingleService6>().UsingLifestyle(new SingletonLifestyle());
                o.Export<SingleService7>().As<SingleService7>().UsingLifestyle(new SingletonLifestyle());
                o.Export<SingleService8>().As<SingleService8>().UsingLifestyle(new SingletonLifestyle());
                o.Export<SingleService9>().As<SingleService9>().UsingLifestyle(new SingletonLifestyle());
                o.Export<SingleService10>().As<SingleService10>().UsingLifestyle(new SingletonLifestyle());
                o.Export<SingleService11>().As<SingleService11>().UsingLifestyle(new SingletonLifestyle());
                o.Export<SingleService12>().As<SingleService12>().UsingLifestyle(new SingletonLifestyle());
                o.Export<SingleService13>().As<SingleService13>().UsingLifestyle(new SingletonLifestyle());
                o.Export<SingleService14>().As<SingleService14>().UsingLifestyle(new SingletonLifestyle());
                o.Export<SingleService15>().As<SingleService15>().UsingLifestyle(new SingletonLifestyle());
                o.Export<SingleService16>().As<SingleService16>().UsingLifestyle(new SingletonLifestyle());
                o.Export<SingleService17>().As<SingleService17>().UsingLifestyle(new SingletonLifestyle());
                o.Export<SingleService18>().As<SingleService18>().UsingLifestyle(new SingletonLifestyle());
                o.Export<SingleService19>().As<SingleService19>().UsingLifestyle(new SingletonLifestyle());
                o.Export<SingleService20>().As<SingleService20>().UsingLifestyle(new SingletonLifestyle());
                o.Export<SingleService21>().As<SingleService21>().UsingLifestyle(new SingletonLifestyle());
                o.Export<SingleService22>().As<SingleService22>().UsingLifestyle(new SingletonLifestyle());
                o.Export<SingleService23>().As<SingleService23>().UsingLifestyle(new SingletonLifestyle());
                o.Export<SingleService24>().As<SingleService24>().UsingLifestyle(new SingletonLifestyle());
                o.Export<SingleService25>().As<SingleService25>().UsingLifestyle(new SingletonLifestyle());
                o.Export<SingleService26>().As<SingleService26>().UsingLifestyle(new SingletonLifestyle());
                o.Export<SingleService27>().As<SingleService27>().UsingLifestyle(new SingletonLifestyle());
                o.Export<SingleService28>().As<SingleService28>().UsingLifestyle(new SingletonLifestyle());
                o.Export<SingleService29>().As<SingleService29>().UsingLifestyle(new SingletonLifestyle());
                o.Export<SingleService30>().As<SingleService20>().UsingLifestyle(new SingletonLifestyle());

                #endregion
            });

            return grace;
        }
    }
}
