
using Core;

namespace DeliveryApp.DAL.Entities
{
    public class DeliveryAddress
    {
        [Key]
        public int DeliveryAddressId { get; set; }
        public int DeliveryDetailsId { get; set; }
        public short CityId { get; set; }
        public string Street { get; set; }
        public string Road { get; set; }
        public string Zip { get; set; }
        public string Address { get; set; }
        public short? AddressTypeId { get; set; }
    }
    public class DeliveryDetails
    {
        [Key]
        public int DeliveryDetailsId { get; set; }
        public int OrderId { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public short DeliveryStatus { get; set; }//Upcoming
        public bool Confirmation { get; set; }
        public DateTime? ConfirmationDate { get; set; }
        public string ConfirmationNote { get; set; }
    }
    public class Item
    {
        [Key]
        public int ItemsId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
    public class OrderItem
    {
        [Key]
        public int OrderItemsId { get; set; }
        public int OrderId { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
    }
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public int UserId { get; set; }
        //public DateTime OrderDate { get; set; } Delivery Date is the Order date
        public decimal Total { get; set; }
        public short? OrderStatusId { get; set; }
    }

}
