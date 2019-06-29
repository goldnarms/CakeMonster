using Kakemons.Core.ViewModels.Search;
using Kakemons.UI.Views;
using Xamarin.Forms.Xaml;

namespace Kakemons.UI.Pages.Search
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	//[MvxTabbedPagePresentation(WrapInNavigationPage = true, Title = "Søk", Icon = "search")]
    public partial class SearchPage : ContentPageBase<SearchViewModel>
	{
		public SearchPage ()
		{
			InitializeComponent ();
            BindingContext = new SearchViewModel();
		}
	}
}
