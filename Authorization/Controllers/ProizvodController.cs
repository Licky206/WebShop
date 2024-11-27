using Authorization.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Authorization.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ProizvodController : Controller
    {
        private readonly IProizvodService _proizvodService;

        public ProizvodController(IProizvodService proizvodService)
        {
            _proizvodService = proizvodService;
        }
        [HttpGet("PROIZVODI")]
        public async Task<IActionResult> GetProizvodi()
        {
            try
            {
                var proizvodi = await _proizvodService.GetAllAsync();
                return Ok(proizvodi); // Return the list of products as a JSON response
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}"); // Handle errors
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddProizvod(string nazivProizvoda, decimal cena, int kolicina)
        {
            await _proizvodService.AddProductAsync(nazivProizvoda, cena, kolicina);
            return Ok("Proizvod je uspesno dodat");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProizvod(int proizvodId)
        {
            await _proizvodService.DeleteProductAsync(proizvodId);
            return Ok("Proizvod je uspesno OBrisan");

        }

    }
}
