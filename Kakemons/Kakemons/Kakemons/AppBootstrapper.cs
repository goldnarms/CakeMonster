using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using Akavache;
using Kakemons.Core.Extensions;
using Kakemons.Core.Helpers;
using Kakemons.Core.ViewModels;
using Kakemons.Core.ViewModels.Baker;
using Kakemons.Core.ViewModels.Cake;
using Kakemons.Core.ViewModels.Favorites;
using Kakemons.Core.ViewModels.Home;
using Kakemons.Core.ViewModels.Login;
using Kakemons.Core.ViewModels.Onboarding;
using Kakemons.Core.ViewModels.Purchase;
using Kakemons.Core.ViewModels.Register;
using Kakemons.Core.ViewModels.Search;
using Kakemons.UI.Pages.Baker;
using Kakemons.UI.Pages.Cake;
using Kakemons.UI.Pages.Chat;
using Kakemons.UI.Pages.Favorites;
using Kakemons.UI.Pages.Login;
using Kakemons.UI.Pages.Onboarding;
using Kakemons.UI.Pages.Purchase;
using Kakemons.UI.Pages.Registration;
using Kakemons.UI.Pages.Search;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Crashes;
using ReactiveUI;
using ReactiveUI.XamForms;
using Splat;
using SQLitePCL;
using Xamarin.Essentials;
using Xamarin.Forms;
using ILogger = Serilog.ILogger;
using LogLevel = Microsoft.AppCenter.LogLevel;

namespace Kakemons.UI
{
    public class AppBootstrapper : ReactiveObject, IScreen
    {
        public AppBootstrapper()
        {
            RxApp.DefaultExceptionHandler = new CustomRxAppExceptionHandler();
                
            using (var scopedInstance = TimerUtil.Instance.ScopedInstance("Bootstrapper"))
            {

                RegisterViews();
                scopedInstance.AddTimestamp("Register views");
                Initialize();
                scopedInstance.AddTimestamp("Initialize");
            }

            Router = new RoutingState();
            Router
                    .NavigateAndReset
                    .Execute(new LoadingViewModel(this))
                    .Subscribe();
        }

        private void Initialize()
        {
            using (var scoped = TimerUtil.Instance.ScopedInstance(nameof(Initialize)))
            {
                Config.Services.RegisterServices();
                scoped.AddTimestamp("Register services");
                Batteries.Init();
                scoped.AddTimestamp("Batteries Init");

                BlobCache.ApplicationName = "MittEnergihjem";

                AppCenter.LogLevel = LogLevel.None;

                var log = Locator.Current.GetService<ILogger>();
                scoped.AddTimestamp("Get log service");

                ObserveCrashReporting(log);
                scoped.AddTimestamp("Observe crash reporting");
            }
        }

        private void ObserveCrashReporting(ILogger log)
        {
            AttachTraceLogToCrashReport(log);

            var failedToSendObservable = Observable.FromEventPattern<FailedToSendErrorReportEventHandler, FailedToSendErrorReportEventArgs>(
                    h => Crashes.FailedToSendErrorReport += h,
                    h => Crashes.FailedToSendErrorReport -= h)
                .Select(e => e.EventArgs);

            var sendingObservable = Observable.FromEventPattern<SendingErrorReportEventHandler, SendingErrorReportEventArgs>(
                    h => Crashes.SendingErrorReport += h,
                    h => Crashes.SendingErrorReport -= h)
                .Select(e => e.EventArgs);

            var sentObservable = Observable.FromEventPattern<SentErrorReportEventHandler, SentErrorReportEventArgs>(
                    h => Crashes.SentErrorReport += h,
                    h => Crashes.SentErrorReport -= h)
                .Select(e => e.EventArgs);

            Observable.FromAsync(async _ => await Crashes.HasCrashedInLastSessionAsync())
                .Where(c => c)
                .LogToAnalytics("Crash", c => "Crashed in last session")
                .Do(c => log.Debug("Crashed in last session"))
                .Subscribe();

            sendingObservable
                .LogToAnalytics("Crash", c => "Sending crash report")
                .Do(c => log.Debug("Sending crash report"))
                .Subscribe();

            failedToSendObservable
                .LogToAnalytics("Crash", c => "Failed to send crash report")
                .Do(c => log.Debug("Failed to send crash report"))
                .Subscribe();

            sentObservable
                .LogToAnalytics("Crash", c => "Sent crash report")
                .Do(c => log.Debug("Sent crash report"))
                .Subscribe();
        }

        private static void AttachTraceLogToCrashReport(ILogger log)
        {
            var info = new DirectoryInfo(FileSystem.AppDataDirectory);
            var textFiles = info.EnumerateFiles("*.txt");
            var lastFile = textFiles.OrderByDescending(t => t.LastWriteTime).FirstOrDefault();
            if (lastFile == null)
            {
                log.Verbose("Log file was not found");
                return;
            }

            try
            {
                var logFilePath = lastFile.FullName;
                var tempPath = Path.Combine(FileSystem.CacheDirectory, lastFile.Name + "-temp");
                if (File.Exists(tempPath))
                    File.Delete(tempPath);
                File.Copy(logFilePath, tempPath);
                var traceLog = File.ReadAllText(tempPath);
                Crashes.GetErrorAttachments = report => new[]
                {
                    ErrorAttachmentLog.AttachmentWithText(traceLog, "traceLog.txt")
                };
            }
            catch (Exception e)
            {
                log.Error(e, "Failed to parse text in traceLog");
            }
        }

        private void RegisterViews()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            
            Locator.CurrentMutable.RegisterConstant(this, typeof(IScreen));
            Locator.CurrentMutable.Register(() => new BakerChatPage(), typeof(IViewFor<BakerChatViewModel>));
            Locator.CurrentMutable.Register(() => new BakerDetailPage(), typeof(IViewFor<BakerProfileViewModel>));

            Locator.CurrentMutable.Register(() => new CakeDetailPage(), typeof(IViewFor<CakeDetailViewModel>));
            //Locator.CurrentMutable.Register(() => new ChatsPage(), typeof(IViewFor<SpotPriceViewModel>));
            Locator.CurrentMutable.Register(() => new FavoritesPage(), typeof(IViewFor<FavoritesViewModel>));

            Locator.CurrentMutable.Register(() => new LoginPage(), typeof(IViewFor<LoginViewModel>));
            Locator.CurrentMutable.Register(() => new OnboardingPage(), typeof(IViewFor<OnboardingViewModel>));
            Locator.CurrentMutable.Register(() => new PurchasePage(), typeof(IViewFor<PurchaseViewModel>));
            Locator.CurrentMutable.Register(() => new RegistrationPage(), typeof(IViewFor<RegisterUserViewModel>));

            Locator.CurrentMutable.Register(() => new SearchPage(), typeof(IViewFor<SearchViewModel>));
            stopwatch.Stop();
            Debug.WriteLine($"{nameof(RegisterViews)} lasted: {stopwatch.ElapsedMilliseconds}ms");
        }

        public Page CreateMainPage()
        {
            return new AppShell();
        }

        private IRoutableViewModel SetFirstViewModel()
        {
            //var settingsService = Locator.Current.GetService<ISettingsService>();
            //var token = settingsService.GetToken().Result;
            //if (token == null || token.TokenType == "Basic")
            //{
            //    if (settingsService.IsNewUser)
            //        return new OnboardingViewModel(this);
            //    return new LoginViewModel(this);
            //}

            return new SearchViewModel(this);
        }

        public RoutingState Router { get; }
    }
}
