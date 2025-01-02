namespace Benchmarks
{
    using ZeroIoC;

    public partial class ZeroContainer : ZeroIoCContainer
    {
        protected override void Bootstrap(IZeroIoCContainerBootstrapper bootstrapper)
        {
            #region Helper
            bootstrapper.AddTransient<Helper1>();
            bootstrapper.AddTransient<Helper2>();
            bootstrapper.AddTransient<Helper3>();
            bootstrapper.AddTransient<Helper4>();
            bootstrapper.AddTransient<Helper5>();
            bootstrapper.AddTransient<Helper6>();
            bootstrapper.AddTransient<Helper7>();
            bootstrapper.AddTransient<Helper8>();
            bootstrapper.AddTransient<Helper9>();
            bootstrapper.AddTransient<Helper10>();
            bootstrapper.AddTransient<Helper11>();
            bootstrapper.AddTransient<Helper12>();
            bootstrapper.AddTransient<Helper13>();
            bootstrapper.AddTransient<Helper14>();
            bootstrapper.AddTransient<Helper15>();
            bootstrapper.AddTransient<Helper16>();
            bootstrapper.AddTransient<Helper17>();
            bootstrapper.AddTransient<Helper18>();
            bootstrapper.AddTransient<Helper19>();
            bootstrapper.AddTransient<Helper20>();
            bootstrapper.AddTransient<Helper21>();
            bootstrapper.AddTransient<Helper22>();
            bootstrapper.AddTransient<Helper23>();
            bootstrapper.AddTransient<Helper24>();
            bootstrapper.AddTransient<Helper25>();
            bootstrapper.AddTransient<Helper26>();
            bootstrapper.AddTransient<Helper27>();
            bootstrapper.AddTransient<Helper28>();
            bootstrapper.AddTransient<Helper29>();
            bootstrapper.AddTransient<Helper30>();
            bootstrapper.AddTransient<Helper31>();
            bootstrapper.AddTransient<Helper32>();
            bootstrapper.AddTransient<Helper33>();
            bootstrapper.AddTransient<Helper34>();
            bootstrapper.AddTransient<Helper35>();
            bootstrapper.AddTransient<Helper36>();
            bootstrapper.AddTransient<Helper37>();
            bootstrapper.AddTransient<Helper38>();
            bootstrapper.AddTransient<Helper39>();
            bootstrapper.AddTransient<Helper40>();
            bootstrapper.AddTransient<Helper41>();
            bootstrapper.AddTransient<Helper42>();
            bootstrapper.AddTransient<Helper43>();
            bootstrapper.AddTransient<Helper44>();
            bootstrapper.AddTransient<Helper45>();
            bootstrapper.AddTransient<Helper46>();
            bootstrapper.AddTransient<Helper47>();
            bootstrapper.AddTransient<Helper48>();
            bootstrapper.AddTransient<Helper49>();
            bootstrapper.AddTransient<Helper50>();
            bootstrapper.AddTransient<Helper51>();
            bootstrapper.AddTransient<Helper52>();
            bootstrapper.AddTransient<Helper53>();
            bootstrapper.AddTransient<Helper54>();
            bootstrapper.AddTransient<Helper55>();
            bootstrapper.AddTransient<Helper56>();
            bootstrapper.AddTransient<Helper57>();
            bootstrapper.AddTransient<Helper58>();
            bootstrapper.AddTransient<Helper59>();
            bootstrapper.AddTransient<Helper60>();
            #endregion

            #region UserService
            bootstrapper.AddTransient<IUserService1, UserService1>();
            bootstrapper.AddTransient<IUserService2, UserService2>();
            bootstrapper.AddTransient<IUserService3, UserService3>();
            bootstrapper.AddTransient<IUserService4, UserService4>();
            bootstrapper.AddTransient<IUserService5, UserService5>();
            bootstrapper.AddTransient<IUserService6, UserService6>();
            bootstrapper.AddTransient<IUserService7, UserService7>();
            bootstrapper.AddTransient<IUserService8, UserService8>();
            bootstrapper.AddTransient<IUserService9, UserService9>();
            bootstrapper.AddTransient<IUserService10, UserService10>();
            bootstrapper.AddTransient<IUserService11, UserService11>();
            bootstrapper.AddTransient<IUserService12, UserService12>();
            bootstrapper.AddTransient<IUserService13, UserService13>();
            bootstrapper.AddTransient<IUserService14, UserService14>();
            bootstrapper.AddTransient<IUserService15, UserService15>();
            bootstrapper.AddTransient<IUserService16, UserService16>();
            bootstrapper.AddTransient<IUserService17, UserService17>();
            bootstrapper.AddTransient<IUserService18, UserService18>();
            bootstrapper.AddTransient<IUserService19, UserService19>();
            bootstrapper.AddTransient<IUserService20, UserService20>();
            bootstrapper.AddTransient<IUserService21, UserService21>();
            bootstrapper.AddTransient<IUserService22, UserService22>();
            bootstrapper.AddTransient<IUserService23, UserService23>();
            bootstrapper.AddTransient<IUserService24, UserService24>();
            bootstrapper.AddTransient<IUserService25, UserService25>();
            bootstrapper.AddTransient<IUserService26, UserService26>();
            bootstrapper.AddTransient<IUserService27, UserService27>();
            bootstrapper.AddTransient<IUserService28, UserService28>();
            bootstrapper.AddTransient<IUserService29, UserService29>();
            bootstrapper.AddTransient<IUserService30, UserService30>();
            #endregion

            #region SingleHelper
            bootstrapper.AddSingleton<SingleHelper1>();
            bootstrapper.AddSingleton<SingleHelper2>();
            bootstrapper.AddSingleton<SingleHelper3>();
            bootstrapper.AddSingleton<SingleHelper4>();
            bootstrapper.AddSingleton<SingleHelper5>();
            bootstrapper.AddSingleton<SingleHelper6>();
            bootstrapper.AddSingleton<SingleHelper7>();
            bootstrapper.AddSingleton<SingleHelper8>();
            bootstrapper.AddSingleton<SingleHelper9>();
            bootstrapper.AddSingleton<SingleHelper10>();
            bootstrapper.AddSingleton<SingleHelper11>();
            bootstrapper.AddSingleton<SingleHelper12>();
            bootstrapper.AddSingleton<SingleHelper13>();
            bootstrapper.AddSingleton<SingleHelper14>();
            bootstrapper.AddSingleton<SingleHelper15>();
            bootstrapper.AddSingleton<SingleHelper16>();
            bootstrapper.AddSingleton<SingleHelper17>();
            bootstrapper.AddSingleton<SingleHelper18>();
            bootstrapper.AddSingleton<SingleHelper19>();
            bootstrapper.AddSingleton<SingleHelper20>();
            bootstrapper.AddSingleton<SingleHelper21>();
            bootstrapper.AddSingleton<SingleHelper22>();
            bootstrapper.AddSingleton<SingleHelper23>();
            bootstrapper.AddSingleton<SingleHelper24>();
            bootstrapper.AddSingleton<SingleHelper25>();
            bootstrapper.AddSingleton<SingleHelper26>();
            bootstrapper.AddSingleton<SingleHelper27>();
            bootstrapper.AddSingleton<SingleHelper28>();
            bootstrapper.AddSingleton<SingleHelper29>();
            bootstrapper.AddSingleton<SingleHelper30>();
            #endregion

            #region SingleService
            bootstrapper.AddSingleton<SingleService1>();
            bootstrapper.AddSingleton<SingleService2>();
            bootstrapper.AddSingleton<SingleService3>();
            bootstrapper.AddSingleton<SingleService4>();
            bootstrapper.AddSingleton<SingleService5>();
            bootstrapper.AddSingleton<SingleService6>();
            bootstrapper.AddSingleton<SingleService7>();
            bootstrapper.AddSingleton<SingleService8>();
            bootstrapper.AddSingleton<SingleService9>();
            bootstrapper.AddSingleton<SingleService10>();
            bootstrapper.AddSingleton<SingleService11>();
            bootstrapper.AddSingleton<SingleService12>();
            bootstrapper.AddSingleton<SingleService13>();
            bootstrapper.AddSingleton<SingleService14>();
            bootstrapper.AddSingleton<SingleService15>();
            bootstrapper.AddSingleton<SingleService16>();
            bootstrapper.AddSingleton<SingleService17>();
            bootstrapper.AddSingleton<SingleService18>();
            bootstrapper.AddSingleton<SingleService19>();
            bootstrapper.AddSingleton<SingleService20>();
            bootstrapper.AddSingleton<SingleService21>();
            bootstrapper.AddSingleton<SingleService22>();
            bootstrapper.AddSingleton<SingleService23>();
            bootstrapper.AddSingleton<SingleService24>();
            bootstrapper.AddSingleton<SingleService25>();
            bootstrapper.AddSingleton<SingleService26>();
            bootstrapper.AddSingleton<SingleService27>();
            bootstrapper.AddSingleton<SingleService28>();
            bootstrapper.AddSingleton<SingleService29>();
            bootstrapper.AddSingleton<SingleService30>();
            #endregion
        }
    }
}
