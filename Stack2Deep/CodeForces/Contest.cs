using Stack2Deep.Configuration;

using Stack2Deep.Dal;

using Stack2Deep.Services.Interfaces;

namespace Stack2Deep.CodeForces;

internal sealed class Contest
{
    private readonly ContestData _contestData;
    
    private readonly DataContext _context;
    private readonly ICodeForcesService _service;

    private readonly List<Participant> _participants = new List<Participant>();

    public Contest(ContestData contestData, DataContext context, ICodeForcesService service)
    {
        _contestData = contestData;
        
        _context = context;
        _service = service;
    }

    public async Task Begin()
    {
        DateTimeOffset startTime = DateTimeOffset.FromUnixTimeSeconds(_contestData.StartTimeSeconds);
        DateTimeOffset now = DateTimeOffset.UtcNow;
        
        var timeDifferenceSeconds = (int)(startTime - now).TotalSeconds;
        await Task.Delay(timeDifferenceSeconds * 1000);
        
        foreach (var user in _context.Profiles)
        {
            var handle = user.CodeforcesId;

            if (await _service.CheckIfRegistered(handle, _contestData.Id))
                _participants.Add(new Participant(handle, _contestData.Id));
        }

        await RunContestLoop();
    }
    
    private async Task RunContestLoop()
    {
        var end =  DateTimeOffset.UtcNow + TimeSpan.FromSeconds(_contestData.Duration);
        var gap = StackConfigurationManager.Configuration.CallGap * 1000;

        var tasks = new List<Task>();
        foreach (var participant in _participants)
        {
            tasks.Add(Task.Run(async () =>
            {
                while (end > DateTimeOffset.Now)
                {
                    await _service.CheckSubmissions(participant);
                    await Task.Delay(gap);
                }
            }));
        }

        await Task.WhenAll(tasks);
    }
}