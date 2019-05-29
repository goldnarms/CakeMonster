using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kakemons.Core.ViewModels.Register;
using Kakemons.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Kakemons.UI.Pages.Registration
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	//[MvxContentPagePresentation(Title = "", NoHistory = true, WrapInNavigationPage = false)]
    public partial class RegistrationPage : ContentPageBase<RegisterUserViewModel>
	{
		public RegistrationPage ()
		{
			InitializeComponent ();
		}
	}
}
