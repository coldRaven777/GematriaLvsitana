using GematriaLvistana.Models;
using GematriaLvstana.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace GematriaLvstana.Controllers
{
    public class GematriaController : ControllerBase
    {
        private readonly ILogger<GematriaController> _logger;
        private readonly IGematriaService _gematriaService;
        public GematriaController(ILogger<GematriaController> logger, IGematriaService gematriaService)
        {
            _gematriaService = gematriaService;
            _logger = logger;
        }
        [HttpGet]
        [Route("api/gematria")]
        public IActionResult Get()
        {
            return Ok("Gematria API is running");
        }

        //endpoint to get all the words in portuguese that have the specific value
        [HttpGet]
        [Route("api/gematria/{value}")]
        public async Task<ActionResult<GematriaResponseModel>> GetWordsByValue(int value)
        {
            if (value <= 0)
            {
                return BadRequest("Value must be greater than zero.");
            }
            var words = await _gematriaService.GetWordsByValue(value);

            if (words == null || !words.Any())
            {
                return NotFound("No words found with the specified value.");
            }

            var response = new GematriaResponseModel()
            {
                Words = words,
                Count = words.Count
            };
            return Ok(response);
        }

    }
}
