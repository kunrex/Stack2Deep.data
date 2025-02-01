using System.Text.Json;
using Stack2Deep.CodeForces;
using Stack2Deep.Services.Interfaces;

namespace Stack2Deep.Services.Implementations;

internal sealed class CodeForcesService : ICodeForcesService
{
    private const string Before = "BEFORE";
    
    private const string BaseUri = "https://codeforces.com/api/";
    private const string PostUri = "";
    
    private readonly HttpClient _client = new HttpClient();

    public CodeForcesService()
    { }

    private async Task<(bool, string)> GetJson(string callString)
    {
        try
        {
            HttpResponseMessage response = await _client.GetAsync(BaseUri + callString);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            return (true, responseBody);
        }
        catch
        {
            return (false, string.Empty);
        }
    }

    public async Task<(bool, int)> GetRating(string codeforcesId)
    {
        var result = await GetJson($"user.info?handles={codeforcesId}");
        if (!result.Item1)
            return (false, 0);
        
        var doc = JsonDocument.Parse(result.Item2);
        JsonElement root = doc.RootElement;

        if (root.GetProperty("status").GetString() == "OK")
            return (true, root.GetProperty("result")[0].GetProperty("rating").GetInt32());

        return (false, 0);
    }

    public async Task<bool> TryGetUser(string codeforcesId)
    {
        var result = await GetJson($"user.info?handles={codeforcesId}");
        var doc = JsonDocument.Parse(result.Item2);
        JsonElement root = doc.RootElement;
        
        return result.Item1 && root.GetProperty("status").GetString() == "OK";
    }

    public async Task<ContestData[]> GetProceedingContests()
    {
        try
        {
            var result = await GetJson("contest.list");
            if (!result.Item1)
                return new ContestData[] { };
            
            var contests = new List<ContestData>();
            var root = JsonDocument.Parse(result.Item2).RootElement;

            if (root.GetProperty("status").GetString() == "OK")
            {
                foreach (var contest in root.GetProperty("result").EnumerateArray())
                {
                    var phase = contest.GetProperty("phase").GetString();
                    if (phase == Before) 
                    {
                        contests.Add(new ContestData
                        {
                            Id = contest.GetProperty("id").GetInt32(),
                            
                            StartTimeSeconds = contest.GetProperty("startTimeSeconds").GetInt32(),
                            Duration = contest.GetProperty("duration").GetInt32(),
                        });
                    }
                }
            }
            
            return contests.ToArray();
        }
        catch 
        {
            return new ContestData[] { };
        }
    }

    public async Task<bool> CheckIfRegistered(string codeforcesId, int contestId)
    {
        try
        {
            var result = await GetJson($"contest.standings?contestId={contestId}&handles={codeforcesId}&showUnofficial=true");
            if (!result.Item1)
                return false;
                    
            var root = JsonDocument.Parse(result.Item2).RootElement;

            if (root.GetProperty("status").GetString() == "OK")
                return root.GetProperty("result").GetProperty("rows").GetArrayLength() > 0;
        }
        catch { }

        return false;
    }

    public async Task CheckSubmissions(Participant participant)
    {
        try
        {
            var result = await GetJson($"user.status?handle={participant.Handle}&from=1&count=10");
            if (!result.Item1)
                return;

            var root = JsonDocument.Parse(result.Item2).RootElement;

            if (root.GetProperty("status").GetString() == "OK")
            {
                foreach (var submission in root.GetProperty("result").EnumerateArray())
                {
                    var verdict = submission.GetProperty("verdict").GetString();
                    var problem = submission.GetProperty("problem").GetProperty("contestId");

                    var problemId = problem.GetInt32() * 1000 + problem.GetString()?[0];
                    var problemName = submission.GetProperty("problem").GetProperty("name").GetString();

                    if (verdict == "OK" && problemId != null)
                    {
                        var pushed = participant.PushTask(problemId.Value);
                        if (pushed)
                            await _client.PostAsJsonAsync(PostUri, new { message = $"{participant.Handle} has solved {problemName}!", code = 200 });
                    }
                }
            }
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}