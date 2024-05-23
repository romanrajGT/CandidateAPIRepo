using CandidateApi.Models;
using CandidateApi.Repositories;

namespace CandidateApi.Services
{
    public interface ICandidateService
    {
        Task AddOrUpdateCandidateAsync(Candidate candidate);
    }

    public class CandidateService : ICandidateService
    {
        private readonly ICandidateRepository _repository;

        public CandidateService(ICandidateRepository repository)
        {
            _repository = repository;
        }

        public async Task AddOrUpdateCandidateAsync(Candidate candidate)
        {
            await _repository.UpsertCandidateAsync(candidate);
        }
    }

}
