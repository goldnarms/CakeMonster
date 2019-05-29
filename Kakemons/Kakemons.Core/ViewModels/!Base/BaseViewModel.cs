using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using Microsoft.AppCenter.Analytics;
using Newtonsoft.Json;
using ReactiveUI;
using Splat;
using ILogger = Serilog.ILogger;

namespace Kakemons.Core.ViewModels
{
    [JsonObject]
    public abstract class BaseViewModel : ReactiveObject, IRoutableViewModel, IDisposable, ISupportsActivation
    {
        private readonly BehaviorSubject<bool> _isVisibleSubject;
        protected readonly CompositeDisposable CompositeDisposable = new CompositeDisposable();
        protected readonly ILogger Log;
        private bool _isBusy;


        public BaseViewModel(IScreen hostScreen = null)
        {
            Log = Serilog.Log.ForContext(GetType());
            HostScreen = hostScreen ?? Locator.Current.GetService<IScreen>();

            _isVisibleSubject = new BehaviorSubject<bool>(false);
            Log.Information($"Instatiated: {GetType().Name}");
            this.WhenActivated(disposables =>
            {
                HandleActivation();

                Disposable
                    .Create(HandleDeactivation)
                    .DisposeWith(disposables);
            });
        }

        protected bool IsBusy
        {
            get => _isBusy;
            set => this.RaiseAndSetIfChanged(ref _isBusy, value);
        }

        protected IObservable<bool> IsVisibleObservable => _isVisibleSubject;

        public void SetVisiblity(bool isVisible)
        {
            _isVisibleSubject.OnNext(isVisible);
        }

        public void Dispose()
        {
            CompositeDisposable?.Dispose();
        }

        [JsonIgnore] public string UrlPathSegment { get; }

        [JsonIgnore] public IScreen HostScreen { get; }

        [JsonIgnore] public ViewModelActivator Activator { get; } = new ViewModelActivator();

        private void HandleActivation()
        {
            var viewModelName = GetType().Name.Replace("ViewModel", "");
            Log.Verbose("ViewAppeared: " + viewModelName);
        }

        private void HandleDeactivation()
        {
            var viewModelName = GetType().Name.Replace("ViewModel", "");
            var properties = new Dictionary<string, string> {{"Name", viewModelName}};
            Log.Verbose("ViewDisappeared: " + viewModelName);
            Analytics.TrackEvent("ViewDisappeared", properties);
        }
    }
}