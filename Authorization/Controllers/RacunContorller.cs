using Authorization.Models;
using Authorization.Services; 
using Authorization.Controllers;
using Microsoft.AspNetCore.Mvc;
using static Authorization.Controllers.DapperController;
using Dapper;

namespace Authorization.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RacunController : ControllerBase
    {
        private readonly IRacunService _racunService;

        public RacunController(IRacunService racunService)
        {
            _racunService = racunService;
        }

        [HttpPost("KreirajRacunSaStavkama")]
        public async Task<IActionResult> KreirajRacunSaStavkama([FromBody] BulkInsertRequest request)
        {
            if (request == null || request.racun == null || request.stavke == null)
            {
                return BadRequest("NERADI");
            }

            foreach (var racun in request.racun)
            { 
                var vreme = racun.Vreme ?? TimeSpan.Zero;

                var zahtev = await _racunService.KreirajRacunSaStavkama(racun.StatusRacuna, racun.Datum, vreme, request.stavke);

                if (!zahtev)
                {
                    return BadRequest("fail");
                }
            }

            return Ok(new { message = "Success" });
        }

    }
}
