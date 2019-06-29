using System.Reactive.Disposables;
using Kakemons.Core.ViewModels.Favorites;
using Kakemons.UI.Views;
using ReactiveUI;
using Xamarin.Forms.Xaml;

namespace Kakemons.UI.Pages.Favorites
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    //[MvxTabbedPagePresentation(WrapInNavigationPage = true, Title = "Favoritter", Icon = "heart")]
    public partial class FavoritesPage : ContentPageBase<FavoritesViewModel>
	{
		public FavoritesPage ()
		{
			InitializeComponent ();

            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, x => x.FavoriteCakes, x => x.CakeList.ItemsSource)
                    .DisposeWith(disposables);
            });


        }
    }
}
