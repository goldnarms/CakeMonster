using System;
using System.ComponentModel;
using System.Reactive;
using System.Threading.Tasks;
using Kakemons.Core.ListView;
using ReactiveUI;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Kakemons.UI.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CakeDetailView : ContentView
    {
        public static readonly BindableProperty CakeModelProperty = BindableProperty.Create("CakeModel", typeof(CakeListItemViewModel), typeof(CakeDetailView), null);
        public static readonly BindableProperty ToggleFavoriteProperty = BindableProperty.Create("ToggleFavorite", typeof(Func<int, Task>), typeof(CakeDetailView));
        public static readonly BindableProperty GoToBakerProperty = BindableProperty.Create("GoToBaker", typeof(ReactiveCommand<string,Unit>), typeof(CakeDetailView));
        private bool _isFavorite;
        private string _bakerId;

        public CakeDetailView()
        {
            InitializeComponent();

            ToggleFavoriteCommand = ReactiveCommand.CreateFromTask(ToggleFavoriteMethod);

            HeartFrame.GestureRecognizers.Add(new TapGestureRecognizer { Command = ToggleFavoriteCommand });
            PropertyChanged += OnPropertyChanged;
        }

        public Func<int, Task> ToggleFavorite
        {
            get => (Func<int, Task>)GetValue(ToggleFavoriteProperty);
            set => SetValue(ToggleFavoriteProperty, value);
        }



        public CakeListItemViewModel CakeModel
        {
            get => (CakeListItemViewModel)GetValue(CakeModelProperty);
            set => SetValue(CakeModelProperty, value);
        }

        public ReactiveCommand<string, Unit> GoToBaker
        {
            get => (ReactiveCommand<string, Unit>)GetValue(GoToBakerProperty);
            set => SetValue(GoToBakerProperty, value);
        }

        public ReactiveCommand<Unit, Unit> ToggleFavoriteCommand { get; set; }

        private async Task ToggleFavoriteMethod()
        {
            await ToggleFavorite(CakeModel.Id);
            _isFavorite = !_isFavorite;
            HeartLabel.Style = _isFavorite ? (Style)Application.Current.Resources["LikedIcon"] : (Style)Application.Current.Resources["UnlikedIcon"];
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == CakeModelProperty.PropertyName)
            {
                CakeImage.Source = ImageSource.FromUri(new Uri(CakeModel.ImageSrc));
                CakeNameLabel.Text = CakeModel.Name;
                CakePriceLabel.Text = CakeModel?.Price.ToString("C0");
                BakerNameLabel.Text = CakeModel.BakerName;
                BakerDistance.Text = "3km"; //TODO: replace with distance
                HeartLabel.Style = CakeModel.IsFavorite ? (Style)Application.Current.Resources["LikedIcon"] : (Style)Application.Current.Resources["UnlikedIcon"];
                _isFavorite = CakeModel.IsFavorite;
                _bakerId = CakeModel.BakerId;
                if (CakeModel.Allergens != null)
                {
                    foreach (var cakeAllergen in CakeModel.Allergens)
                    {
                        var allergenFrame = new Frame { Padding = new Thickness(5, 2), CornerRadius = 2, BackgroundColor = Color.FromHex(cakeAllergen.Color) };
                        var allergenNameLabel = new Label { Text = cakeAllergen.Name, TextColor = Color.White, FontSize = 11 };
                        allergenFrame.Content = allergenNameLabel;
                        AllergensList.Children.Add(allergenFrame);
                    }
                }
                CakeDetails.Text = CakeModel.Description;
            }
            else if (args.PropertyName == GoToBakerProperty.PropertyName)
            {
                BakerLabel.GestureRecognizers.Clear();
                BakerLabel.GestureRecognizers.Add(new TapGestureRecognizer { Command = GoToBaker, CommandParameter = _bakerId});
            }
        }
    }
}
