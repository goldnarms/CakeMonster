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
    public partial class CakeSmallListViewCell : NoSelectionStateViewCell
    {
        public static readonly BindableProperty CakeProperty = BindableProperty.Create("Cake", typeof(CakeDto), typeof(CakeListViewCell), null);
        public static readonly BindableProperty IsFavoriteProperty = BindableProperty.Create("IsFavorite", typeof(bool), typeof(CakeListViewCell), false);
        public static readonly BindableProperty GoToDetailsProperty = BindableProperty.Create("GoToDetails", typeof(ReactiveCommand<Unit,int>), typeof(CakeListViewCell));
        public static readonly BindableProperty ToggleFavoriteProperty = BindableProperty.Create("ToggleFavorite", typeof(Func<int, Task>), typeof(CakeListViewCell));

        public ReactiveCommand<Unit, Unit> ToggleFavoriteCommand { get; set; }

        private async Task ToggleFavoriteMethod()
        {
            await ToggleFavorite(Cake.Id);
            IsFavorite = !IsFavorite;
            HeartLabel.Style = IsFavorite ? (Style)Application.Current.Resources["LikedIcon"] : (Style)Application.Current.Resources["UnlikedIcon"];
        }

        public CakeDto Cake
        {
            get => (CakeDto)GetValue(CakeProperty);
            set => SetValue(CakeProperty, value);
        }

        public bool IsFavorite
        {
            get => (bool)GetValue(IsFavoriteProperty);
            set => SetValue(IsFavoriteProperty, value);
        }

        public ReactiveCommand<Unit, int> GoToDetails
        {
            get => (ReactiveCommand<Unit, int>)GetValue(GoToDetailsProperty);
            set => SetValue(GoToDetailsProperty, value);
        }

        public Func<int, Task> ToggleFavorite
        {
            get => (Func<int, Task>)GetValue(ToggleFavoriteProperty);
            set => SetValue(ToggleFavoriteProperty, value);
        }

        public CakeSmallListViewCell()
        {
            InitializeComponent();

            ToggleFavoriteCommand = ReactiveCommand.CreateFromTask(ToggleFavoriteMethod);
            HeartLabel.GestureRecognizers.Clear();
            HeartLabel.GestureRecognizers.Add(new TapGestureRecognizer { Command = ToggleFavoriteCommand });
            HeartLabel.Style = (Style)Application.Current.Resources["UnlikedIcon"];
            PropertyChanged += OnPropertyChanged;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == CakeProperty.PropertyName)
            {
                if(Cake == null)
                    return;

                if(Cake.Images != null && Cake.Images.Count > 0)
                    CakeImage.Source = ImageSource.FromUri(new Uri(Cake.Images.First().Url));

                CakeNameLabel.Text = Cake.Name;

                CakePriceLabel.Text = Cake.Price.ToString("C0");
                CakeNameLabel.Text = Cake.Name;
            }
            else if (args.PropertyName == GoToDetailsProperty.PropertyName)
            {
                CakeNameLabel.GestureRecognizers.Clear();
                CakeNameLabel.GestureRecognizers.Add(new TapGestureRecognizer() { Command = GoToDetails});
            }
            else if (args.PropertyName == IsFavoriteProperty.PropertyName)
            {
                HeartLabel.Style = IsFavorite ? (Style)Application.Current.Resources["LikedIcon"] : (Style)Application.Current.Resources["UnlikedIcon"];
            }
        }
    }
}
