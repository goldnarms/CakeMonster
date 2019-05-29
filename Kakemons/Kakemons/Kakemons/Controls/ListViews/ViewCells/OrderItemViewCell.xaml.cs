using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Kakemons.Common.Dtos;
using ReactiveUI;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Kakemons.UI.Controls.ListViews.ViewCells
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class OrderItemViewCell : NoSelectionStateViewCell
    {
        public static readonly BindableProperty CakeProperty = BindableProperty.Create("Cake", typeof(CakeDto), typeof(OrderItemViewCell), null);
        public static readonly BindableProperty RemoveCakeProperty = BindableProperty.Create("RemoveCake", typeof(Func<int, Task>), typeof(OrderItemViewCell));

        public CakeDto Cake
        {
            get => (CakeDto)GetValue(CakeProperty);
            set => SetValue(CakeProperty, value);
        }

        public OrderItemViewCell ()
		{
			InitializeComponent ();
		    RemoveCakeCommand = ReactiveCommand.CreateFromTask(RemoveCakeMethod);
		    PropertyChanged += OnPropertyChanged;

		    RemoveLabel.GestureRecognizers.Clear();
		    RemoveLabel.GestureRecognizers.Add(new TapGestureRecognizer { Command = RemoveCakeCommand });
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == CakeProperty.PropertyName)
            {
                if (Cake == null)
                    return;

                if (Cake.Images != null && Cake.Images.Count > 0)
                    CakeImage.Source = ImageSource.FromUri(new Uri(Cake.Images.First().Url));

                CakeNameLabel.Text = Cake.Name;

                CakePriceLabel.Text = Cake.Price.ToString("C0");
                CakeNameLabel.Text = Cake.Name;
            }
        }

        private async Task RemoveCakeMethod()
        {
            await RemoveCake(Cake.Id);
        }

        public ReactiveCommand<Unit, Unit> RemoveCakeCommand { get; set; }

        public Func<int, Task> RemoveCake
        {
            get => (Func<int, Task>)GetValue(RemoveCakeProperty);
            set => SetValue(RemoveCakeProperty, value);
        }
    }
}
