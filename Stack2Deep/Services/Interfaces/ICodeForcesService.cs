using Stack2Deep.CodeForces;

namespace Stack2Deep.Services.Interfaces;

internal interface ICodeForcesService
{
    public Task<bool> TryGetUser(string codeforcesId);
    public Task<(bool, int)> GetRating(string codeforcesId);

    public Task<ContestData[]> GetProceedingContests();
    
    public Task<bool> CheckIfRegistered(string codeforcesId, int contestId);

    public Task CheckSubmissions(Participant participant);
}