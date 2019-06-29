using System.Reactive.Disposables;
using Kakemons.Core.ViewModels.Cake;
using Kakemons.UI.Views;
using ReactiveUI;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Kakemons.UI.Pages.Cake
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	//[MvxContentPagePresentation(WrapInNavigationPage = true)]
    public partial class CakeDetailPage : ContentPageBase<CakeDetailViewModel>
    {
		public CakeDetailPage ()
		{
			InitializeComponent ();

            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, x => x.CakeModel, x => x.CakeDetailView.CakeModel)
                    .DisposeWith(disposables);
                this.OneWayBind(ViewModel, x => x.ToggleFavoriteCommand, x => x.CakeDetailView.ToggleFavoriteCommand)
                    .DisposeWith(disposables);
                this.OneWayBind(ViewModel, x => x.GoToBakerCommand, x => x.CakeDetailView.GoToBaker)
                    .DisposeWith(disposables);
                this.OneWayBind(ViewModel, x => x.OrderCakeCommand, x => x.OrderBtn.Command)
                    .DisposeWith(disposables);
            });
        }
	}
}
