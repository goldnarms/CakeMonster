using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kakemons.Core.ViewModels.Login;
using Kakemons.UI.Views;
using ReactiveUI.XamForms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Kakemons.UI.Pages.Login
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	//[MvxContentPagePresentation(Title = "", NoHistory = true, WrapInNavigationPage = false)]
    public partial class LoginPage : ContentPageBase<LoginViewModel>
    {
		public LoginPage ()
		{
			InitializeComponent ();
		}
	}
}
