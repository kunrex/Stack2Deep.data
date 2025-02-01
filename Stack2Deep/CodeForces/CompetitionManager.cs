using Stack2Deep.Dal;
using Stack2Deep.Services.Interfaces;

namespace Stack2Deep.CodeForces;

internal sealed class CompetitionManager
{
    private readonly HttpClient client = new HttpClient();

    private readonly DataContext _context;
    
    private readonly IRegistrationService _registration;
    private readonly ICodeForcesService _codeForces;

    public CompetitionManager(DataContext context, IRegistrationService registrationService, ICodeForcesService codeForcesService)
    {
        _context = context;
        
        _registration = registrationService;
        _codeForces = codeForcesService;

        Task.Run(async () =>
        {
            await Begin();
        });
    }

    private async Task Begin()
    {
        var contestData = await _codeForces.GetProceedingContests();
        
        var tasks = new List<Task>();
        foreach (var data in contestData)
        {
            var contest = new Contest(data, _context, _codeForces);

            tasks.Add(Task.Run(async () =>
            {
                await contest.Begin();
            }));
        }

        await Task.WhenAll(tasks);
    }
}