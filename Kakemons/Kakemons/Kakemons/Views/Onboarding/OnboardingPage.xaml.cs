using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kakemons.Core.ViewModels.Onboarding;
using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Kakemons.UI.Pages.Onboarding
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	[MvxContentPagePresentation(WrapInNavigationPage = false)]
    public partial class OnboardingPage : MvxContentPage<OnboardingViewModel>
	{
		public OnboardingPage ()
		{
			InitializeComponent ();


		    IndicatorsControl.UnselectedIndicatorStyle = (Style)Application.Current.Resources["UnselectIndicatorFrameStyle"];
		    IndicatorsControl.SelectedIndicatorStyle = (Style)Application.Current.Resources["SelectedIndicatorFrameStyle"];
        }
	}
}
