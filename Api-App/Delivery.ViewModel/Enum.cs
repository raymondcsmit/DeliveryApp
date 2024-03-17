namespace Delivery.ViewModel
{
    public enum OrderDeliveryStatus
    {
        UpComming = 1,
        Delivered = 2,
        InProcess = 3
    }
    public enum SubScriptionPeriod
    {
        None = 0,
        Monthly = 1,
        BiMonthly = 2,
        Weekly = 3,
        BiWeekly = 4
    }
    public enum DeliveryActivityType
    {
        Adding = 1,
        Updating = 2,
        Deleting = 3
    }
}
