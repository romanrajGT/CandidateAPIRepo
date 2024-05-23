using CandidateApi.Models;
using CandidateApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace CandidateApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandidatesController : ControllerBase
    {
        private readonly ICandidateService _service;

        public CandidatesController(ICandidateService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> AddOrUpdateCandidate([FromBody] Candidate candidate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _service.AddOrUpdateCandidateAsync(candidate);
            return Ok(new { message = "Candidate added or updated successfully." });
        }
    }

}
