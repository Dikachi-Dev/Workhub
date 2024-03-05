namespace Workhub.Domain.Entities
{
    public class Job : BaseEntity
    {
        public string BuyerName { get; set; } = string.Empty;
        public string SellerName { get; set; } = string.Empty;
        public string SellerId { get; set; } = string.Empty;
        public string BuyerId { get; set; } = string.Empty;
        public string Occupation { get; set; } = string.Empty;
        public double BuyerRating { get; set; } = 0.00;
        public double SellerRating { get; set; } = 0.00;
        public string Status { get; set; } = string.Empty;
        public Profile Profile { get; set; } = new Profile();
    }
}
