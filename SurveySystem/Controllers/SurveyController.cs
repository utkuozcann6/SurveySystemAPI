using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SurveySystem.Services;
using SurveySystem.DTOs;
using System.Security.Claims;

namespace SurveySystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SurveyController : ControllerBase
    {
        private readonly ISurveyService _surveyService;

        public SurveyController(ISurveyService surveyService)
        {
            _surveyService = surveyService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSurvey([FromBody] CreateSurveyDto createSurveyDto)
        {
            try
            {
                var userId = GetCurrentUserId();
                var survey = await _surveyService.CreateSurveyAsync(createSurveyDto, userId);
                return CreatedAtAction(nameof(GetSurvey), new { id = survey.Id }, survey);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "server error" });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSurvey(int id)
        {
            try
            {
                var survey = await _surveyService.GetSurveyByIdAsync(id);
                if (survey == null)
                {
                    return NotFound(new { message = "Anket Bulunamadı" });
                }
                return Ok(survey);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "server error" });
            }
        }

        [HttpGet("my-surveys")]
        public async Task<IActionResult> GetMySurveys()
        {
            try
            {
                var userId = GetCurrentUserId();
                var surveys = await _surveyService.GetUserSurveysAsync(userId);
                return Ok(surveys);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "server error" });
            }
        }

        [HttpGet("{id}/results")]
        public async Task<IActionResult> GetSurveyResults(int id)
        {
            try
            {
                var results = await _surveyService.GetSurveyResultsAsync(id);
                return Ok(results);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "server error" });
            }
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                throw new UnauthorizedAccessException("Kullanıcı doğrulanamadı");
            }
            return int.Parse(userIdClaim.Value);
        }
    }
}
