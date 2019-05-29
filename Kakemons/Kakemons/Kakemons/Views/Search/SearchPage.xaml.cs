using Kakemons.Core.ViewModels.Search;
using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Xamarin.Forms.Xaml;

namespace Kakemons.UI.Pages.Search
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	[MvxTabbedPagePresentation(WrapInNavigationPage = true, Title = "Søk", Icon = "search")]
    public partial class SearchPage : MvxContentPage<SearchViewModel>
	{
		public SearchPage ()
		{
			InitializeComponent ();
		}
	}
}
