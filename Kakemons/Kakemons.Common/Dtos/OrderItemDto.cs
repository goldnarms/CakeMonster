namespace Kakemons.Common.Dtos
{
    public class OrderLineDto
    {
        public int Id { get; set; }
        public int CakeId { get; set; }
        public int Quantity { get; set; }
        public CakeDto Cake { get; set; }
    }
}
