using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Microsoft.AppCenter.Analytics;
using Serilog;

namespace Kakemons.Core.Extensions
{
    public static class RxExtensions
    {
        public static IObservable<Unit> Signal()
        {
            return Observable.Return(Unit.Default);
        }
         
        public static readonly Func<int, TimeSpan> DefaultStrategy = n => TimeSpan.FromSeconds(Math.Min(Math.Pow(2.0, n), 180.0));

        public static IObservable<T> RetryWithBackoff<T>(this IObservable<T> @this, int? retryCount = 3,
            Func<int, TimeSpan> strategy = null, Func<Exception, bool> retryOnError = null, IScheduler scheduler = null)
        {
            strategy = strategy ?? DefaultStrategy;
            scheduler = scheduler ?? DefaultScheduler.Instance;
            retryOnError = retryOnError ?? (e => true);
            var attempt = 0;
            IObservable<Notification<T>> source = Observable.Defer(() =>
            {
                var num = attempt;
                attempt = num + 1;
                return (num == 0 ? @this : @this.DelaySubscription(strategy(attempt - 1), scheduler))
                    .Select(Notification.CreateOnNext).Catch((Func<Exception, IObservable<Notification<T>>>)(ex =>
                        !retryOnError(ex)
                            ? Observable.Return(Notification.CreateOnError<T>(ex))
                            : Observable.Throw<Notification<T>>(ex)));
            });
            return (!retryCount.HasValue ? source.Retry() : source.Retry(retryCount.Value)).Dematerialize();
        }

        /// <summary>
        /// Logs each step in the observable sequence. Subscribe, OnNext, OnError, OnCompleted, Duration and when Disposed
        /// </summary>
        /// <param name="source"></param>
        /// <param name="name"></param>
        /// <param name="log"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IObservable<T> LogStats<T>(this IObservable<T> source, string name, ILogger log)
        {
            return Observable.Using(
                () => Time(name, log),
                timer => Observable.Create<T>(
                    o =>
                    {
                        log.Debug($"{name}.Subscribe()");
                        var subscription = source
                            .Do(
                                i =>
                                {
                                    log.Debug($"{name}.OnNext({i})");
                                    log.Debug($"{name}.CurrentThread({Thread.CurrentThread.ManagedThreadId})");
                                },
                                ex => log.Debug($"{name}.OnError({ex})\n {ex.Message})"),
                                () => log.Debug($"{name}.OnCompleted()"))
                            .Subscribe(o);
                        var disposal = Disposable.Create(() => log.Debug($"{name}.Dispose()"));
                        return new CompositeDisposable(subscription, disposal);
                    }));
        }

        public static IObservable<T> LogException<T>(this IObservable<T> @this, ILogger log, string message = null,
            [CallerMemberName] string callerMemberName = null,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = 0)
        {
            return @this.Catch<T, Exception>(ex =>
            {
                Dictionary<string, string> properties = new Dictionary<string, string>
                {
                    {"CallerMemberName", callerMemberName}, 
                    {"CallerFilePath", callerFilePath}, 
                    {"CallerLineNumber", callerLineNumber.ToString()}
                };
                log.Error(ex, message ?? ex.Message, properties);
                return @this;
            });
        }

        public static IObservable<T> CatchAndReturnDefault<T>(this IObservable<T> @this)
        {
            return @this.Catch<T, Exception>(exception => Observable.Return(default(T)));
        }

        public static IObservable<T> LogToAnalytics<T>(this IObservable<T> @this, string key,
            string value = null)
        {
            if (value == null)
                return @this.Do(s => Analytics.TrackEvent(key));
            var properties = new Dictionary<string, string> {{key, value}};
            return @this.Do(s => Analytics.TrackEvent(key, properties));
        }

        public static IObservable<T> LogToAnalytics<T>(this IObservable<T> @this, string key, Dictionary<string, string> properties)
        {
            return @this.Do(s => Analytics.TrackEvent(key, properties));
        }

        public static IObservable<T> LogToAnalytics<T>(this IObservable<T> @this, string key, Func<T, string> eventName)
        {
            return @this.Do(s => Analytics.TrackEvent(key, new Dictionary<string, string> {{"Event", eventName.Invoke(s)}}));
        }
        
        public static IDisposable Time(string title, ILogger logger)
        {
            return new Timer(title, null, logger);
        }
    }
    
    internal sealed class Timer : IDisposable
    {
        private readonly string _name;
        private readonly TimeSpan? _threshold;
        
        private readonly string _classFileName;
        private readonly string _methodName;
        private readonly Stopwatch _stopwatch;
        private readonly bool _useCustomDescription;
        private readonly ILogger _log;

        public Timer(string name, TimeSpan? threshold, string classFileName, string methodName, ILogger logger)
        {
            _name = name;
            _threshold = threshold;
            _classFileName = Path.GetFileNameWithoutExtension(classFileName);
            _methodName = methodName;
            _log = logger.ForContext<Timer>();
#if DEBUG
            _stopwatch = Stopwatch.StartNew();
#endif
        }

        public Timer(string name, TimeSpan? threshold, ILogger logger)
        {
            _name = name;
            _threshold = threshold;
            _log = logger.ForContext<Timer>();
            _useCustomDescription = true;
#if DEBUG
            _stopwatch = Stopwatch.StartNew();
#endif      
        }

        public void Dispose()
        {
#if DEBUG
            _stopwatch.Stop();
            if (_threshold.HasValue &&
                _stopwatch.ElapsedMilliseconds >= _threshold.GetValueOrDefault().TotalMilliseconds)
                _log.Debug(_useCustomDescription 
                    ? $"{_name} took {_stopwatch.Elapsed.TotalMilliseconds}ms"
                    : $"{_classFileName}.{_methodName}() took longer than threshold: {_stopwatch.Elapsed.TotalMilliseconds}ms");
            else
                _log.Debug(_useCustomDescription
                    ? $"{_name} took {_stopwatch.Elapsed.TotalMilliseconds}ms"
                    : $"{_classFileName}.{_methodName}() took {_stopwatch.Elapsed.TotalMilliseconds}ms");
#endif
        }
    }
}
