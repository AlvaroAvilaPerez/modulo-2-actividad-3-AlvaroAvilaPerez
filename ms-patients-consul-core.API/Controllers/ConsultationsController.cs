using Microsoft.AspNetCore.Mvc;
using ms_patients_consul_core.Application.DTOs;
using ms_patients_consul_core.Application.UseCases;

namespace ms_patients_consul_core.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConsultationsController : ControllerBase
    {
        private readonly ExecuteConsultationUseCase _executeConsultationUseCase;

        public ConsultationsController(ExecuteConsultationUseCase executeConsultationUseCase)
        {
            _executeConsultationUseCase = executeConsultationUseCase;
        }

        [HttpPost]
        public async Task<IActionResult> Execute([FromBody] ExecuteConsultationInput input)
        {
            try
            {
                await _executeConsultationUseCase.ExecuteAsync(input);
                return StatusCode(StatusCodes.Status201Created, new { Message = "Consultation recorded successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
    }
}
