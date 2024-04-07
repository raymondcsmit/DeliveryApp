namespace Delivery.ViewModel
{
    public class DeliveryViewModel
    {
        public string DeliveryNumber { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string DeliveryTimeRange { get; set; }
        public string DeliveryAddress { get; set; }
        public string CourierName { get; set; }
        public List<ItemViewModel> Items { get; set; }
        public DateTime EditableUntil { get; set; }
    }

    public class ItemViewModel
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
    }

}
