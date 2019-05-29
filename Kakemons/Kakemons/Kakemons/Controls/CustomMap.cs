using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Kakemons.UI.Controls
{
    public class CustomMap : Map
    {
        public static readonly BindableProperty PositionProperty =
            BindableProperty.Create(nameof(Position),
                typeof(Position),
                typeof(CustomMap),
                default(Position));

        public Position Position
        {
            get => (Position) GetValue(PositionProperty);
            set => SetValue(PositionProperty, value);
        }
    }
}
