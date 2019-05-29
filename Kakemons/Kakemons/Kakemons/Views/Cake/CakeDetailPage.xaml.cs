using Kakemons.Core.ViewModels.Cake;
using Kakemons.UI.Views;
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
		}
	}
}
