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
	public partial class BakerChatPage : ContentPageBase<BakerChatViewModel>
	{
		public BakerChatPage ()
		{
			InitializeComponent ();

            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, x => x.SendMessageCommand, x => x.SendMessageBtn.Command)
                    .DisposeWith(disposables);
                this.OneWayBind(ViewModel, x => x.ChatMessages, x => x.ChatListView.ItemsSource)
                    .DisposeWith(disposables);
                this.Bind(ViewModel, x => x.Message, x => x.MessageEntry.Text)
                    .DisposeWith(disposables);
            });
        }
	}
}
