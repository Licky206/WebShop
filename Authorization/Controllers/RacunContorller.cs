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

        //Kreiranje StavkeRacuna
        [HttpPost("KreirajRacunSaStavkama")]
        public async Task<IActionResult> KreirajRacunSaStavkama([FromBody] BulkInsertRequest request)
        {
            if (request == null || request.racun == null || request.stavke == null)
            {
                return BadRequest("NERADI");
            }

            foreach (var racun in request.racun)
            {

                var zahtev = await _racunService.KreirajRacunSaStavkama(racun.StatusRacuna, racun.Datum, request.stavke);

                if (!zahtev)
                {
                    return BadRequest("fail");
                }
            }

            return Ok(new { message = "Success" });
        }

        //Uzimanje Stavke po Racun ID
        [HttpGet("GetStavkeByRacunId/{racunId}")]
        public async Task<ActionResult<IEnumerable<StavkeRacuna>>> GetStavkeByRacunId(int racunId)
        {
            var stavke = await _racunService.GetStavkeByRacunIdAsync(racunId);
            if (stavke == null)
            {
                return NotFound();
            }

            return Ok(stavke);
        }


        [HttpGet("GetAllRacuni")]
        public async Task<ActionResult<IEnumerable<Racun>>> GetAllRacuni()
        {
            var racuni = await _racunService.GetAllRacuniAsync();
            return Ok(racuni);
        }

        [HttpGet("GetRacunById/{racunId}")]
        public async Task<ActionResult<Racun>> GetRacunById(int racunId)
        {
            var racun = await _racunService.GetRacunByIdAsync(racunId);
            if (racun == null)
            {
                return NotFound();
            }
            return Ok(racun);

        }
        [HttpGet("UpdateRacunStatus/{racunId}/{newStatus}")]
        public async Task<IActionResult>UpdateRacunStatus(int racunId, string newStatus)
        {
 
            var result = await _racunService.UpdateRacunStatusAsync(racunId, newStatus);

            if (result)
            {
                return Ok(true);
            }
            else
            {
                return NotFound("Invoice not found.");
            }
        }



        [HttpDelete("DeleteRacun/{racunId}")]
        public async Task<IActionResult> DeleteRacun(int racunId)
        {
            var result = await _racunService.DeleteRacunAsync(racunId);
            if (!result)
            {
                return NotFound("Invoice not found or could not be deleted.");
            }

            return Ok(new { message = "Invoice deleted successfully." });
        }

    }
}
