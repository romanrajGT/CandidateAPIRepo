using CandidateApi.Data;
using CandidateApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace CandidateApi.Repositories
{
    public interface ICandidateRepository
    {
        Task UpsertCandidateAsync(Candidate candidate);
        Task<Candidate> GetCandidateByEmailAsync(string email);
    }

    public class CandidateRepository : ICandidateRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;

        public CandidateRepository(ApplicationDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task UpsertCandidateAsync(Candidate candidate)
        {
            var cacheKey = $"Candidate_{candidate.Email}";
            var existingCandidate = await GetCandidateByEmailAsync(candidate.Email);

            if (existingCandidate != null)
            {
                candidate.Id = existingCandidate.Id;
                _context.Candidates.Update(candidate);

                // Update the cache
                _cache.Set(cacheKey, candidate);
            }
            else
            {
                await _context.Candidates.AddAsync(candidate);

                // Add to the cache
                _cache.Set(cacheKey, candidate);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<Candidate> GetCandidateByEmailAsync(string email)
        {
            var cacheKey = $"Candidate_{email}";

            if (!_cache.TryGetValue(cacheKey, out Candidate candidate))
            {
                candidate = await _context.Candidates
                    .AsNoTracking()
                    .SingleOrDefaultAsync(c => c.Email == email);

                if (candidate != null)
                {
                    _cache.Set(cacheKey, candidate);
                }
            }

            return candidate;
        }
    }
}
