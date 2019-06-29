using Acr.UserDialogs;
using Akavache;
using Kakemons.Core.Helpers;
using Kakemons.Core.ModelServices;
using Kakemons.SDK.ApiServices;
using Serilog;
using Splat;
using ILogger = Serilog.ILogger;

namespace Kakemons.UI.Config
{
    public static class Services
    {
        public static void RegisterServices()
        {
            Locator.CurrentMutable.RegisterLazySingleton(() => Log.Logger);
            Locator.CurrentMutable.RegisterConstant(BlobCache.LocalMachine);
            Locator.CurrentMutable.RegisterConstant(BlobCache.Secure);
            Locator.CurrentMutable.RegisterLazySingleton(() => UserDialogs.Instance);

            Locator.CurrentMutable.RegisterLazySingleton(
                () => new AppSettings(Locator.Current.GetService<IBlobCache>()), 
                typeof(IAppSettings));
            Locator.CurrentMutable.RegisterLazySingleton(() => new CakeApiService(
                Locator.Current.GetService<ILogger>(),
                ));
            Locator.CurrentMutable.RegisterLazySingleton(
                () => new CakeModelService(new CakeApiService(), ),
                typeof(IAppSettings));
        }
    }
}
