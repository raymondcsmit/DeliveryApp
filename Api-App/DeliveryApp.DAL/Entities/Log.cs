using Core;


namespace DeliveryApp.DAL.Entities
{
    public class DeliveryLog
    {
        [Key]
        public int LogId { get; set; }
        public int DeliveryDetailsId { get; set; }
        public DateTime ActivityDate { get; set; }
        public string ActivityDescription { get; set; }
        public short ActivityTypeId { get; set; }
    }

}
