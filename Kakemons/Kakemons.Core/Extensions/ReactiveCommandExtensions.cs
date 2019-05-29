using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveUI;
using Serilog;

namespace Kakemons.Core.Extensions
{
    public static class ReactiveCommandExtensions
    {
        public static IDisposable AttachLogging<TIn, TOut>(this ReactiveCommand<TIn, TOut> command, string title,
            ILogger logger)
        {
            var cleanTitle = title.Replace("Command", string.Empty);
            CompositeDisposable cd = new CompositeDisposable
            {
                command.ThrownExceptions
                    .LogException(logger)
                    .Subscribe(),

                command
                    .LogToAnalytics("Actions", cleanTitle)
                    .Do(l => logger.Information("Action: " + cleanTitle))
                    .Subscribe()
            };
            return cd;
        }

        public static IDisposable LogException<T>(this ObservableAsPropertyHelper<T> oaph, string name, ILogger logger)
        {
            return oaph.ThrownExceptions
                .LogException(logger, $"Exception in {name}")
                .Subscribe();
        }
    }
}
