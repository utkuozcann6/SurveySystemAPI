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
    public class VoteController : ControllerBase
    {
        private readonly IVoteService _voteService;

        public VoteController(IVoteService voteService)
        {
            _voteService = voteService;
        }

        [HttpPost]
        public async Task<IActionResult> Vote([FromBody] VoteDto voteDto)
        {
            try
            {
                var userId = GetCurrentUserId();
                await _voteService.VoteAsync(voteDto, userId);
                return Ok(new { message = "Anket başarıyla kaydedildi" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
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