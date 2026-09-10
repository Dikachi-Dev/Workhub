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
        public string BuyerAddress { get; set; } = string.Empty;
        public string Remark { get; set; } = string.Empty;
        public bool IsRated { get; set; } = false;

        public void Accept(string? sellerAddress = null)
        {
            Status = "Accepted";
            if (!string.IsNullOrWhiteSpace(sellerAddress))
            {
                SellerAddress = sellerAddress;
            }
        }

        public void Cancel()
        {
            Status = "Cancelled";
        }

        public void Rate(int rating, string remark)
        {
            SellerRating = Math.Clamp(rating, 0, 5);
            Remark = remark;
            IsRated = true;
            Status = "Completed";
        }
    }
}
