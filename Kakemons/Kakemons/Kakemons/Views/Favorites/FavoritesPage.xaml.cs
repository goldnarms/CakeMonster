using Kakemons.Core.ViewModels.Favorites;
using Kakemons.UI.Views;
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
		}
	}
}
