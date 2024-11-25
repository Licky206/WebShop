namespace Authorization.Models
{
    public class UpdateStavkaRequest
    {
        public int stavkaId { get; set; }
        public int NewKolicina { get; set; }
        public decimal NewPopust { get; set; }
    }
}
