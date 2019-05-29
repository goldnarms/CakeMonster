using Kakemons.Core.ViewModels.Favorites;
using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Xamarin.Forms.Xaml;

namespace Kakemons.UI.Pages.Favorites
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [MvxTabbedPagePresentation(WrapInNavigationPage = true, Title = "Favoritter", Icon = "heart")]
    public partial class FavoritesPage : MvxContentPage<FavoritesViewModel>
	{
		public FavoritesPage ()
		{
			InitializeComponent ();
		}
	}
}
