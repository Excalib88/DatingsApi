namespace Datings.Api.Common.Models.Questions;

public class AnswerModel
{
    public string Content { get; set; } = null!;
    
    public UserModel? User { get; set; }
    public long QuestionId { get; set; }
    
    public List<FullCommentModel?>? Comments { get; set; }
}