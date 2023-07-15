namespace Datings.Api.Data.Entities;

public class Answer : BaseEntity
{
    public string Content { get; set; } = null!;
    public bool MarkedAsTheBest { get; set; }
    
    public ApplicationUser? User { get; set; }
    public long? UserId { get; set; }
    
    public Question? Question { get; set; }
    public long? QuestionId { get; set; }
    
    public List<AnswerComment>? Comments { get; set; }
}