using Xamarin.Forms;

namespace Kakemons.UI.Controls
{
    public class TintableImage : Image
    {
        public static readonly BindableProperty TintColorProperty =
            BindableProperty.Create(nameof(Tint),
                typeof(Color),
                typeof(TintableImage),
                default(Color));

        public Color Tint
        {
            get => (Color) GetValue(TintColorProperty);
            set => SetValue(TintColorProperty, value);
        }
    }
}
