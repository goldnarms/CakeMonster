
//using Microsoft.AspNetCore.SignalR.Client;

namespace Kakemons.Core.Extensions
{
    public static class SignalRExtensions
    {
        /*
        public static IObservable<TReturn> ToObservableFromHubMethod<TReturn>(this HubConnection connection, string hubMethodName)
        {
            Debug.WriteLine("------ Subscribing on: " + hubMethodName + "------");
            return Observable.Create<TReturn>(obs =>
            {
                var cts = new CancellationTokenSource();

                var disposable = Disposable.Create(() => { cts.Cancel(); });
                try
                {
                    connection.On<TReturn>(hubMethodName, v =>
                    {
                        Debug.WriteLine($"----- Subscribing on {hubMethodName} got message -----");
                        obs.OnNext(v);
                    });
                }
                catch (Exception ex)
                {
                    obs.OnError(ex);
                }

                return disposable;
            });
        }

        public static IObservable<Unit> ToObservableFromHubMethod(this HubConnection connection, string hubMethodName)
        {
            return connection.ToObservableFromHubMethod<Unit>(hubMethodName);
        }
            */
    }
}
