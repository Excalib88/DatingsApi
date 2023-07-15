namespace Datings.Api.Common.Models.Questions;

public class CommentModel
{
    public string Content { get; set; } = null!;
    public AnswerModel? Answer { get; set; }
    public long? AnswerId { get; set; }
}