using System;
using System.Collections.Generic;
using System.Text;

namespace Kakemons.Common.Dtos
{
    public class PurchaseDto
    {
        public int Id { get; set; }
        public DateTimeOffset PurchaseDate { get; set; }
        public bool ReceiptSent { get; set; }
        public IEnumerable<OrderLineDto> OrderLines { get; set; }
        public string CustomerId { get; set; }
        public string BakerId { get; set; }
        public CustomerDto Customer { get; set; }
        public BakerDto Baker { get; set; }

    }
}
