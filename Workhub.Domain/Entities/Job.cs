namespace Workhub.Domain.Entities
{
    public class Job : BaseEntity
    {
        public string BuyerName { get; set; } = string.Empty;
        public string SellerName { get; set; } = string.Empty;
        public string SellerId { get; set; } = string.Empty;
        public string BuyerId { get; set; } = string.Empty;
        public string Occupation { get; set; } = string.Empty;
        public int SellerRating { get; set; } = 0;
        public string Status { get; set; } = string.Empty;
        public string SellerAddress { get; set; } = string.Empty;
        public string BuyerAddeess { get; set; } = string.Empty;
        public string Remark { get; set; } = string.Empty;
        public bool IsRated { get; set; } = false;
    }
}
