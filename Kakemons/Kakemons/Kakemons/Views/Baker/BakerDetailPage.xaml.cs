using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kakemons.Core.ViewModels.Baker;
using MvvmCross.Forms.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Kakemons.UI.Pages.Baker
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BakerDetailPage : MvxContentPage<BakerProfileViewModel>
	{
		public BakerDetailPage ()
		{
			InitializeComponent ();
		}
	}
}
