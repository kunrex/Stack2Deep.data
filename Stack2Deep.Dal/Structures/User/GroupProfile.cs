namespace Stack2Deep.Dal.Structures.User;

public class GroupProfile : Item
{
    public string GroupName { get; set; } = string.Empty;

    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset FinishDate { get; set; }

    public GroupProfile()
    { }

    public GroupProfile(string groupName, DateTimeOffset startDate, DateTimeOffset finishDate)
    {
        GroupName = groupName;

        StartDate = startDate;
        FinishDate = finishDate;
    }
}
