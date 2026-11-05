namespace Trusted.ViewModel.DTO.Response.Deal
{
    public class CreateDealRes
    {
        public long DealId { get; set; }
    }

    public class DealDetailsRes
    {
        public long Id { get; set; }
        public long AccountId { get; set; }
        public string AccountName { get; set; }
        public long BuyerId { get; set; }
        public string BuyerName { get; set; }
        public long SelectedMediatorId { get; set; }
        public string SelectedMediatorName { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
