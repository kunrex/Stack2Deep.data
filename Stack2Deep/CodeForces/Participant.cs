namespace Stack2Deep.CodeForces;

internal sealed class Participant
{
    private readonly string _handle;
    public string Handle { get => _handle; }

    private readonly int _contestId;
    public int ContestId { get => _contestId; }

    private readonly HashSet<int> solvedProblems = new HashSet<int>();

    public Participant(string handle, int contestId)
    {
        _handle = handle;
        _contestId = contestId;
    }

    public bool PushTask(int code)
    {
        if (!solvedProblems.Contains(code))
        {
            solvedProblems.Add(code);
            return true;
        }

        return false;
    }
}