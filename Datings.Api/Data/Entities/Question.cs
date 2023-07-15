namespace Datings.Api.Data.Entities;

public class Question : BaseEntity
{
    public string Content { get; set; } = null!;
    public decimal Price { get; set; }

    public List<CategoryTag> CategoryTags { get; set; } = new();
    public List<Answer>? Answers { get; set; } 
    
    public ApplicationUser? User { get; set; }
    public long? UserId { get; set; }

    public Answer? BestAnswer => Answers?.FirstOrDefault(x => x.MarkedAsTheBest);
}