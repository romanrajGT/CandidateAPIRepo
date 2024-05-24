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

        // Default GET action to indicate API readiness
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { message = "Welcome! The Candidate API is ready to be tested." });
        }

        [HttpPost]
        public async Task<IActionResult> AddOrUpdateCandidate([FromBody] Candidate candidate)
        {
            if (string.IsNullOrWhiteSpace(candidate.FirstName))
            {
                ModelState.AddModelError("FirstName", "First name is required.");
            }

            if (string.IsNullOrWhiteSpace(candidate.LastName))
            {
                ModelState.AddModelError("LastName", "Last name is required.");
            }

            if (string.IsNullOrWhiteSpace(candidate.Email))
            {
                ModelState.AddModelError("Email", "Email is required.");
            }

            if (string.IsNullOrWhiteSpace(candidate.Comment))
            {
                ModelState.AddModelError("Comment", "Comment is required.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _service.AddOrUpdateCandidateAsync(candidate);
            return Ok(new { message = "Candidate added or updated successfully." });
        }
    }

}
