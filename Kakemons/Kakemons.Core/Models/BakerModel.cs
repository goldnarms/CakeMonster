using System;
using System.Collections.Generic;
using System.Text;
using Kakemons.Common.Dtos;

namespace Kakemons.Core.Models
{
    public class BakerModel
    {
        public string Id {get; set; }
        public string Name { get; set; }
        public IEnumerable<CakeDto> Cakes { get; set; }
        public string Address { get; set; }
        public string AvatarUrl { get; set; }
        public string Description { get; set; }
        public double Distance { get; set; }
    }
}
