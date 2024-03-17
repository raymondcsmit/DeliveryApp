using Core;


namespace DeliveryApp.DAL.Entities
{
    public class ActivityType
    {
        [Key]
        public int ActivityTypeId { get; set; }
        public string ActivityTypeName { get; set; }
    }
    public class AddressType
    {
        [Key]
        public short AddressTypeId { get; set; }
        public string Name { get; set; }
    }
    public class DeliveryStatus
    {
        [Key]
        public short DeliveryStatusId { get; set; }
        public string Name { get; set; }
    }
    public class UserType
    {
        [Key]
        public short UserTypeId { get; set; }
        public string Name { get; set; }
    }
    public class Period
    {
        [Key]
        public short PeriodId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
    public class ScheduleStatus
    {
        [Key]
        public short ScheduleStatusId { get; set; }
        public string Name { get; set; }
    }

}
