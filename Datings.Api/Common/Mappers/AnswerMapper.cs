using Datings.Api.Common.Models.Questions;
using Datings.Api.Data.Entities;

namespace Datings.Api.Common.Mappers;

public static class AnswerMapper
{
    public static AnswerModel? ToAnswerModel(this Answer? answer)
    {
        return answer == null
            ? null
            : new AnswerModel
            {
                Content = answer.Content,
                User = answer.User.ToUserModel(),
                QuestionId = answer.QuestionId!.Value,
                Comments = answer.Comments?.Select(x => x.ToCommentModel()).ToList()
            };
    }

    public static FullCommentModel? ToCommentModel(this AnswerComment? answerComment)
    {
        return answerComment == null
            ? null
            : new FullCommentModel
            {
                Content = answerComment.Content,
                User = answerComment.User.ToUserModel()!,
                AnswerId = answerComment.AnswerId,
                UserId = answerComment.UserId!.Value
            };
    }
}