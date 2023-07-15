namespace Datings.Api.Data.Entities;

public class CategoryTag : BaseEntity
{
    public string Name { get; set; } = null!;
    
    public List<Question>? Questions { get; set; }
}