namespace Authorization.Models
{
   
        public class BulkInsertRequest
        {
            public IEnumerable<RacunDTO> racun { get; set; }
            public IEnumerable<StavkeRacunaDTO> stavke { get; set; }
        }


    
}
