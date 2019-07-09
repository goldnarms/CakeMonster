using Acr.UserDialogs;
using Akavache;
using Kakemons.Core.Contracts;
using Kakemons.Core.Helpers;
using Kakemons.Core.ModelServices;
using Kakemons.Core.Services;
using Kakemons.SDK.ApiContracts;
using Kakemons.SDK.ApiServiceContracts;
using Kakemons.SDK.ApiServices;
using Kakemons.SDK.FakeApiServices;
using ReactiveUI;
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

            Locator.CurrentMutable.RegisterLazySingleton(
                () => new DialogService(), 
                typeof(IDialogService));

            Locator.CurrentMutable.RegisterLazySingleton(() => new SettingsService(
                Locator.Current.GetService<ILogger>(),
                Locator.Current.GetService<ISecureBlobCache>(),
                Locator.Current.GetService<IBlobCache>()), 
                typeof(ISettingsService));

            // API Services
            //SetupApiServices();
            SetupFakeApiServices();

            Locator.CurrentMutable.RegisterLazySingleton(() => new AppUserModelService(
                Locator.Current.GetService<IUserApiService>(),
                Locator.Current.GetService<ILogger>(),
                Locator.Current.GetService<IAppSettings>()), typeof(IAppUserModelService));

            Locator.CurrentMutable.RegisterLazySingleton(
                () => new CakeModelService(Locator.Current.GetService<ICakeApiService>(),
                    Locator.Current.GetService<IAppUserModelService>(),
                    Locator.Current.GetService<IPaymentApiService>(),
                    Locator.Current.GetService<ILogger>()),
                typeof(ICakeModelService));
        }

        private static void SetupFakeApiServices()
        {
            Locator.CurrentMutable.RegisterLazySingleton(() => new FakeCakeApiService(
                Locator.Current.GetService<IBakerApiService>()), typeof(ICakeApiService));

            Locator.CurrentMutable.RegisterLazySingleton(() => new FakeAccountApiService(), 
                typeof(IAccountApiService));

            Locator.CurrentMutable.RegisterLazySingleton(() => new FakeBakerApiService(), 
                typeof(IBakerApiService));

            Locator.CurrentMutable.RegisterLazySingleton(() => new FakeChatApiService(), 
                typeof(IChatApiService));

            Locator.CurrentMutable.RegisterLazySingleton(() => new FakePaymentApiService(),
                typeof(IPaymentApiService));

            Locator.CurrentMutable.RegisterLazySingleton(() => new FakeUserApiService(), 
                typeof(IUserApiService));

        }

        private static void SetupApiServices()
        {
            Locator.CurrentMutable.RegisterLazySingleton(() => new CakeApiService(
                Locator.Current.GetService<ILogger>(), HttpClientService.Instance), typeof(ICakeApiService));

            Locator.CurrentMutable.RegisterLazySingleton(() => new AccountApiService(
                    Locator.Current.GetService<ILogger>(),
                    HttpClientService.Instance),
                typeof(IAccountApiService));

            Locator.CurrentMutable.RegisterLazySingleton(() => new BakerApiService(
                    Locator.Current.GetService<ILogger>(),
                    HttpClientService.Instance),
                typeof(IBakerApiService));

            Locator.CurrentMutable.RegisterLazySingleton(() => new ChatApiService(),
                typeof(IChatApiService));

            Locator.CurrentMutable.RegisterLazySingleton(() => new PaymentApiService(),
                typeof(IPaymentApiService));

            Locator.CurrentMutable.RegisterLazySingleton(() => new UserApiService(),
                typeof(IUserApiService));

        }
    }
}
