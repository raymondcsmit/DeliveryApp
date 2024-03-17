namespace Delivery.ViewModel
{
    public class SubScribeItemVM
    {
        public int ItemId { get; set; }
        public int Quantity { get; set; }
    }
    public class SubScribeVM
    {
        public DateTime ToDate { get; set; }
        public DateTime FromDate { get; set; } = DateTime.UtcNow;// Current Date
        public string DeliverySlot { get; set; }
        public int? AssignedTo { get; set; }
        public string Note { get; set; }
        public short? PeriodId { get; set; }

        public DateTime OrderDate { get; set; }
        public decimal Total { get; set; }
        public List<SubScribeItemVM> Items { get; set; }
    }
}
