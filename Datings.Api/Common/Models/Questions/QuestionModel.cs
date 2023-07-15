namespace Datings.Api.Common.Models.Questions;

public class QuestionModel
{
    public string Content { get; set; } = null!;
    public List<string> CategoryTags { get; set; } = null!;
    public decimal Price { get; set; }
    public AnswerModel? BestAnswer { get; set; }
    public List<AnswerModel?>? Answers { get; set; } = new();
    public UserModel? User { get; set; }
}