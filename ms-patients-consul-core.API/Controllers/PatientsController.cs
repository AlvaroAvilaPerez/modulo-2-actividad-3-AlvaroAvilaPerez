using Microsoft.AspNetCore.Mvc;
using ms_patients_consul_core.Application.DTOs;
using ms_patients_consul_core.Application.UseCases;
using ms_patients_consul_core.Application.Queries;

namespace ms_patients_consul_core.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientsController : ControllerBase
    {
        private readonly RegisterPatientUseCase _registerPatientUseCase;
        private readonly UpdateEatingHabitsUseCase _updateEatingHabitsUseCase;
        private readonly AssignNutritionistUseCase _assignNutritionistUseCase;
        private readonly GetPatientHistoryQueryService _getPatientHistoryQueryService;

        public PatientsController(
            RegisterPatientUseCase registerPatientUseCase,
            UpdateEatingHabitsUseCase updateEatingHabitsUseCase,
            AssignNutritionistUseCase assignNutritionistUseCase,
            GetPatientHistoryQueryService getPatientHistoryQueryService)
        {
            _registerPatientUseCase = registerPatientUseCase;
            _updateEatingHabitsUseCase = updateEatingHabitsUseCase;
            _assignNutritionistUseCase = assignNutritionistUseCase;
            _getPatientHistoryQueryService = getPatientHistoryQueryService;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterPatientInput input)
        {
            try
            {
                await _registerPatientUseCase.ExecuteAsync(input);
                return StatusCode(StatusCodes.Status201Created, new { Message = "Patient registered successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPut("eating-habits")]
        public async Task<IActionResult> UpdateEatingHabits([FromBody] UpdateEatingHabitsInput input)
        {
            try
            {
                await _updateEatingHabitsUseCase.ExecuteAsync(input);
                return NoContent();
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

        [HttpPut("assign-nutritionist")]
        public async Task<IActionResult> AssignNutritionist([FromBody] AssignNutritionistInput input)
        {
            try
            {
                await _assignNutritionistUseCase.ExecuteAsync(input);
                return NoContent();
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

        [HttpGet("{id:guid}/history")]
        public async Task<ActionResult<PatientHistoryDto>> GetHistory(Guid id)
        {
            var history = await _getPatientHistoryQueryService.ExecuteAsync(id);
            if (history == null)
            {
                return NotFound(new { Error = $"Patient with ID {id} was not found." });
            }

            return Ok(history);
        }
    }
}
