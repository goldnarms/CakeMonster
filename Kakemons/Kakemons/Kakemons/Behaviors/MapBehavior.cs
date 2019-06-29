using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Kakemons.UI.Behaviors
{
    public class MapBehavior : BindableBehavior<Map>
    {
        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource),
            typeof(IEnumerable<Position>), typeof(MapBehavior), null, BindingMode.Default, null, ItemsSourceChanged);

        public IEnumerable<Position> ItemsSource
        {
            get => (IEnumerable<Position>) GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        private static void ItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is MapBehavior behavior)) return;
            behavior.AddPins();
        }

        private void AddPins()
        {
            var map = AssociatedObject;
            if (map == null || ItemsSource == null || !ItemsSource.Any())
                return;
            for (var i = map.Pins.Count - 1; i >= 0; i--)
                map.Pins.RemoveAt(i);

            var pins = ItemsSource.Select(x =>
            {
                var pin = new Pin
                {
                    Type = PinType.Place,
                    Position = new Position(x.Latitude, x.Longitude),
                    Label = "Selected address" //TODO Add label
                };

                return pin;
            }).ToArray();

            map.Pins.Clear();

            foreach (var pin in pins)
                map.Pins.Add(pin);

            PositionMap();
        }


        private void PositionMap()
        {
            if (ItemsSource == null || !ItemsSource.Any()) return;

            var centerPosition =
                new Position(ItemsSource.Average(x => x.Latitude), ItemsSource.Average(x => x.Longitude));
            AssociatedObject.MoveToRegion(MapSpan.FromCenterAndRadius(centerPosition, Distance.FromKilometers(60)));
        }
    }
}
