namespace Datings.Api.Common.Models.Questions;

public class FullCommentModel : CommentModel
{
    public long UserId { get; set; }
    public UserModel User { get; set; } = null!;
}