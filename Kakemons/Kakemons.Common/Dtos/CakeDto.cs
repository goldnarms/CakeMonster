using System;
using System.Collections.Generic;
using Kakemons.Common.Enums;

namespace Kakemons.Common.Dtos
{
    public class CakeDto:IEquatable<CakeDto>
    {
        public int Id { get; set;}
        public string BakerId { get; set; }
        public CakeAvailability Availability { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public BakerDto Baker { get; set; }
        public List<AllergenDto> Allergens { get; set; }
        public List<TagDto> Tags { get; set; }
        public List<ImageUrlDto> Images { get; set; }
        public double Price { get; set; }
        public DateTimeOffset Modified { get; set; }
        public CakeType CakeType { get; set; }

        public bool Equals(CakeDto other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id && Modified.Equals(other.Modified);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CakeDto) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Id * 397) ^ Modified.GetHashCode();
            }
        }
    }
}
