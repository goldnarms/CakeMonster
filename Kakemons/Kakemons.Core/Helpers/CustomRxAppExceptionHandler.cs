using System;
using System.Diagnostics;
using System.Reactive.Concurrency;
using ReactiveUI;
using Serilog;

namespace Kakemons.Core.Helpers
{
    public class CustomRxAppExceptionHandler : IObserver<Exception>
    {
        public void OnNext(Exception value)
        {
            if (Debugger.IsAttached) Debugger.Break();

            Log.Logger.Error(value, "RxApp");

            RxApp.MainThreadScheduler.Schedule(() => { throw value; });
        }

        public void OnError(Exception error)
        {
            if (Debugger.IsAttached) Debugger.Break();

            Log.Logger.Error(error, "RxApp uncaught error");
            RxApp.MainThreadScheduler.Schedule(() => { throw error; });
        }

        public void OnCompleted()
        {
            if (Debugger.IsAttached) Debugger.Break();
            RxApp.MainThreadScheduler.Schedule(() => { throw new NotImplementedException(); });
        }
    }
}