using Xamarin.Forms;

namespace Kakemons.UI.Controls
{
    public class GradientFrame : Frame
    {
        public static readonly BindableProperty StartColorProperty =
            BindableProperty.Create(nameof(StartColor),
                typeof(Color),
                typeof(GradientFrame),
                default(Color));

        public Color StartColor
        {
            get => (Color) GetValue(StartColorProperty);
            set => SetValue(StartColorProperty, value);
        }
        
        public static readonly BindableProperty EndColorProperty =
            BindableProperty.Create(nameof(EndColor),
                typeof(Color),
                typeof(GradientFrame),
                default(Color));

        public Color EndColor
        {
            get => (Color) GetValue(EndColorProperty);
            set => SetValue(EndColorProperty, value);
        }
        
        public static readonly BindableProperty IsVerticalProperty =
            BindableProperty.Create(nameof(IsVertical),
                typeof(bool),
                typeof(GradientFrame),
                default(bool));

        public bool IsVertical
        {
            get => (bool) GetValue(IsVerticalProperty);
            set => SetValue(IsVerticalProperty, value);
        }
    }
}

