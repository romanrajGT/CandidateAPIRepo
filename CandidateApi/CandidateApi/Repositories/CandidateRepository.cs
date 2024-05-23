using CandidateApi.Data;
using CandidateApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CandidateApi.Repositories
{
    public interface ICandidateRepository
    {
        Task UpsertCandidateAsync(Candidate candidate);
    }

    public class CandidateRepository : ICandidateRepository
    {
        private readonly ApplicationDbContext _context;

        public CandidateRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task UpsertCandidateAsync(Candidate candidate)
        {
            var existingCandidate = await _context.Candidates
                .AsNoTracking()
                .SingleOrDefaultAsync(c => c.Email == candidate.Email);

            if (existingCandidate != null)
            {
                candidate.Id = existingCandidate.Id;
                _context.Candidates.Update(candidate);
            }
            else
            {
                await _context.Candidates.AddAsync(candidate);
            }

            await _context.SaveChangesAsync();
        }
    }

}
