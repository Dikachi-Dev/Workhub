namespace Workhub.Domain.Entities
{
    public class JobRequest : BaseEntity
    {
        public string? BuyerName { get; set; }
        public string? SellerName { get; set; }
        public string? SellerId { get; set; }
        public string? BuyerId { get; set; }
    }
}
