using System.ComponentModel.DataAnnotations.Schema;

namespace Stack2Deep.Dal.Structures.Contexts;

public sealed class UserGroupContext : Item
{
    [ForeignKey("GroupProfileId")]
    public long GroupProfileId { get; set; }
    
    [ForeignKey("UserProfileId")]
    public long UserProfileId { get; set; }
    
    public UserGroupContext()
    { }

    public UserGroupContext(long groupProfileId, long userProfileId)
    {
        GroupProfileId = groupProfileId;
        UserProfileId = userProfileId;
    }
}