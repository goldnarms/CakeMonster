using Kakemons.Core.ViewModels.Cake;
using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Kakemons.UI.Pages.Cake
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	[MvxContentPagePresentation(WrapInNavigationPage = true)]
    public partial class CakeDetailPage : MvxContentPage<CakeDetailViewModel>
    {
		public CakeDetailPage ()
		{
			InitializeComponent ();
		}
	}
}
