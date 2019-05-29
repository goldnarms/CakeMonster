using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Kakemons.Common.Dtos;
using Kakemons.Common.Enums;
using ReactiveUI;

namespace Kakemons.Core.ListView
{
    public class CakeListItemViewModel : ReactiveObject
    {
        private bool _isFavorite;
        private int _id;

        public CakeListItemViewModel(int id)
        {
            Id = id;
            ToggleFavoriteCommand = ReactiveCommand.Create(() =>
            {
                IsFavorite = !IsFavorite;
                return Id;
            });

            GoToDetailsCommand = ReactiveCommand.Create<Unit, int>(_ => Id);
            GoToBakerCommand = ReactiveCommand.Create<Unit, string>(_ => BakerId);
        }

        public int Id
        {
            get => _id;
            private set => _id = value;
        }

        public ReactiveCommand<Unit, int> ToggleFavoriteCommand { get; }

        public ReactiveCommand<Unit, int> GoToDetailsCommand { get; }

        public ReactiveCommand<Unit, string> GoToBakerCommand { get; }

        public bool IsFavorite
        {
            get => _isFavorite;
            set => this.RaiseAndSetIfChanged(ref _isFavorite, value);
        }

        public string BakerId { get; set; }
        public CakeAvailability Availability { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<AllergenDto> Allergens { get; set; }
        public List<string> TagNames { get; set; }
        public string BakerName { get; set; }
        public string ImageSrc { get; set; }
        public double Price { get; set; }
        public CakeType CakeType { get; set; }
        public CakeDto CakeModel { get; set; }


        public static CakeListItemViewModel TransformToListItem(CakeDto cakeDto, bool isFavorite)
        {
            return new CakeListItemViewModel(cakeDto.Id)
            {
                IsFavorite = isFavorite,
                Allergens = cakeDto.Allergens?.ToList(),
                TagNames = cakeDto.Tags?.Select(a => a.Name).ToList(),
                Name = cakeDto.Name,
                BakerId = cakeDto.BakerId,
                Description = cakeDto.Description,
                Availability = cakeDto.Availability,
                BakerName = cakeDto.Baker?.Fullname,
                ImageSrc = cakeDto.Images.FirstOrDefault()?.Url,
                Price = cakeDto.Price,
                CakeModel = cakeDto,
                CakeType = cakeDto.CakeType
            };
        }
    }
}
