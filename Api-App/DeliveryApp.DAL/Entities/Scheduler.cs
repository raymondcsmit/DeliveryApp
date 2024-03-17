using Core;


namespace DeliveryApp.DAL.Entities
{
    public class Scheduler
    {
        [Key]
        public int SchedulerId { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime FromDate { get; set; }
        public string DeliverySlot { get; set; }
        public int? AssignedTo { get; set; }
        public string Note { get; set; }
        public short? PeriodId { get; set; }
        public short? ScheduleStatusId { get; set; }

    }
}
