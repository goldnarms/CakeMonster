using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Kakemons.Core.Extensions;
using Kakemons.Core.Helpers;
using Kakemons.Core.ViewModels.Login;
using ReactiveUI;
using Splat;

namespace Kakemons.Core.ViewModels
{
    public class LoadingViewModel : BaseViewModel
    {
        private readonly IAppSettings _appSettings;
        private readonly ObservableAsPropertyHelper<bool> _isLoading;

        public LoadingViewModel(
            IScreen hostScreen = null,
            IAppSettings appSettings = null
            ) : base(hostScreen)
        {
            _appSettings = appSettings ?? Locator.Current.GetService<IAppSettings>();
            LoadCommand = ReactiveCommand.CreateFromTask(LoadStuff);

            RxExtensions.Signal()
                .InvokeCommand(LoadCommand);

            _isLoading = LoadCommand.IsExecuting
                .ToProperty(this, vm => vm.IsLoading);

            LoadCommand.Subscribe();
        }

        public bool IsLoading => _isLoading.Value;

        public ReactiveCommand<Unit, Unit> LoadCommand { get; }

        private async Task LoadStuff(CancellationToken ct)
        {
            try
            {
                var user = await _appSettings.GetUser();
            }
            catch (Exception e)
            {
                Log.Error(e, "LoadStuff failed");
            }
        }
    }
}