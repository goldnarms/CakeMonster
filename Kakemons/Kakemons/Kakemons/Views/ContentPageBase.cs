using Kakemons.Core.ViewModels;
using ReactiveUI.XamForms;
using Log = Serilog.Log;

namespace Kakemons.UI.Views
{
    public class ContentPageBase<TViewModel> : ReactiveContentPage<TViewModel> where TViewModel : BaseViewModel
    {
        protected ContentPageBase()
        {
            Log.Logger.Information($"Instantiated: {GetType().Name}");
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            Log.Logger.Information($"OnAppearing: {GetType().Name}");
        }
    }
}
