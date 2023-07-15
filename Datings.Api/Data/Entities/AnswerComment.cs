namespace Datings.Api.Data.Entities;

public class AnswerComment : BaseEntity
{
    public string Content { get; set; } = null!;
    public long? UserId { get; set; }
    public ApplicationUser? User { get; set; }
    public long? AnswerId { get; set; }
    public Answer? Answer { get; set; }
}