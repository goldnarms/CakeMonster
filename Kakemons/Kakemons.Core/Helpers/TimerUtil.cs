using System;
using System.Diagnostics;
using System.Reactive.Disposables;
using Serilog;

namespace Kakemons.Core.Helpers
{
    public class TimerUtil : IDisposable
    {
        public string Title { get; }
        private long _previousTimestamp = 0;
        private readonly Stopwatch _stopwatch;
        private readonly CompositeDisposable _cd;
        private static TimerUtil _instance;
        private readonly Stopwatch _internalStopWatch;
        private long _previousInternalTimestamp;

        private TimerUtil(string title, Stopwatch internalStopWatch)
        {
            Title = title;
            _cd = new CompositeDisposable();

            
            if (internalStopWatch != null)
            {
                _internalStopWatch = new Stopwatch();
                _internalStopWatch.Start();
                _stopwatch = internalStopWatch;
                Log.Logger.Information($"[{title}] Started:{_stopwatch.ElapsedMilliseconds}ms");
            }
            else
            {
                Log.Logger.Information($"{Title}: Started");
                _stopwatch = new Stopwatch();
                _stopwatch.Start();
            }
        }

        public static TimerUtil Instance => _instance ?? (_instance = new TimerUtil("Timer", null));

        public void Dispose()
        {
            if (_internalStopWatch != null)
            {
                Log.Logger.Information($"[{Title}] Ended:{_internalStopWatch.ElapsedMilliseconds}ms Duration:{_internalStopWatch.ElapsedMilliseconds}ms");
            }
            else
                Log.Logger.Information($"[{Title}] Ended:{_stopwatch.ElapsedMilliseconds}ms Duration:{_stopwatch.ElapsedMilliseconds}ms");
            _cd?.Dispose();
        }

        public TimerUtil ScopedInstance(string name)
        {

            TimerUtil scopedTimerUtil = new TimerUtil($"{Title}->{name}", _stopwatch);
            
            _cd.Add(scopedTimerUtil);
            return scopedTimerUtil;
        }

        public void AddTimestamp(string title)
        {
            var elapsedMilliseconds = _stopwatch.ElapsedMilliseconds;
            var delta = elapsedMilliseconds - _previousTimestamp;
            _previousTimestamp = elapsedMilliseconds;
            
            if (_internalStopWatch != null)
            {
                var elapsedMillisecondsInternalStopWatch = _internalStopWatch.ElapsedMilliseconds;
                var internalDelta = elapsedMillisecondsInternalStopWatch - _previousInternalTimestamp;
                _previousInternalTimestamp = elapsedMillisecondsInternalStopWatch;
                Log.Logger.Information($"[{Title}]:{title}: {elapsedMilliseconds}ms Internal:{elapsedMillisecondsInternalStopWatch}ms (+{internalDelta}ms)");
                return;
            }
            
            Log.Logger.Information($"[{Title}]:{title}: {elapsedMilliseconds}ms (+{delta}ms)");
        }
    }
}