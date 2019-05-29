using System;
using System.ComponentModel;
using System.Diagnostics;
using Kakemons.Core.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Kakemons.UI.Controls
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BakerProfileView : ContentView
    {
        public static readonly BindableProperty BakerModelProperty =
            BindableProperty.Create("BakerModel", typeof(BakerModel), typeof(BakerProfileView), null);

        public BakerProfileView ()
		{
			InitializeComponent ();

		    PropertyChanged += OnPropertyChanged;
        }

        public BakerModel BakerModel
        {
            get => (BakerModel)GetValue(BakerModelProperty);
            set => SetValue(BakerModelProperty, value);
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == BakerModelProperty.PropertyName)
            {
                Debug.WriteLine(BakerModel.AvatarUrl);
                Avatar.Source = ImageSource.FromUri(new Uri(BakerModel.AvatarUrl));
                //Avatar.Source = ImageSource.FromUri(new Uri("https://images.pexels.com/photos/1462980/pexels-photo-1462980.jpeg?auto=compress&cs=tinysrgb&h=750&w=1260"));
                FullName.Text = BakerModel.Name;
                Distance.Text = BakerModel.Distance.ToString("N0");
            }
        }
    }
}
