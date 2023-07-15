namespace Datings.Api.Common.Models.Questions;

public class QuestionsFilter
{
    public List<long> CategoryTags { get; set; } = new();
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
}