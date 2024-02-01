namespace Workhub.Domain.Entities
{
    public class JobRequest : BaseEntity
    {
        public string BuyerName { get; set; } = string.Empty;
        public string SellerName { get; set; } = string.Empty;
        public string SellerId { get; set; } = string.Empty;
        public string BuyerId { get; set; } = string.Empty;
    }
}
