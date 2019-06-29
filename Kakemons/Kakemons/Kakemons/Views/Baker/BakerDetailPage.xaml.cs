using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;
using Kakemons.Core.ViewModels.Baker;
using Kakemons.UI.Views;
using ReactiveUI;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Kakemons.UI.Pages.Baker
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BakerDetailPage : ContentPageBase<BakerProfileViewModel>
	{
		public BakerDetailPage ()
		{
			InitializeComponent ();

            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, x => x.AvailableCakes, x => x.CakeList.ItemsSource)
                    .DisposeWith(disposables);
                this.OneWayBind(ViewModel, x => x.StartChatCommand, x => x.StartChatBtn.Command)
                    .DisposeWith(disposables);
                this.OneWayBind(ViewModel, x => x.BakerModel, x => x.BakerProfileView.BakerModel)
                    .DisposeWith(disposables);
            });
        }
	}
}
