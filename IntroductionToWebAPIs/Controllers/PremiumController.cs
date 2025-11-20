using IntroductionToWebAPIs.DTO;
using IntroductionToWebAPIs.Services.IService;
using Microsoft.AspNetCore.Mvc;

namespace IntroductionToWebAPIs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PremiumController : ControllerBase
    {
        private readonly IPremiumCalculationService _premiumService;

        public PremiumController(IPremiumCalculationService premiumService)
        {
            _premiumService = premiumService;
        }

        [HttpGet("calculate-premium/{clientId}")]
        public async Task<ActionResult<PremiumCalculationResult>> CalculatePremium(
            Guid clientId,
            CancellationToken ct = default)
        {
            try
            {
                var result = await _premiumService.CalculateAsync(clientId, ct);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ошибка при расчёте премии: " + ex.Message);
            }
        }
    }
}
