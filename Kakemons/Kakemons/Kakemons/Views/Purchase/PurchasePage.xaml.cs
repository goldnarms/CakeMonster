using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kakemons.Core.ViewModels.Purchase;
using MvvmCross.Forms.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Kakemons.UI.Pages.Purchase
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PurchasePage : MvxContentPage<PurchaseViewModel>
	{
		public PurchasePage ()
		{
			InitializeComponent ();
		}
	}
}
