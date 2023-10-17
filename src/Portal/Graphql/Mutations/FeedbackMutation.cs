using HotChocolate.Authorization;
using Portal.Common.Dto;
using Portal.Common.Dto.Feedback;
using Portal.Services.FeedbackService;

namespace Portal.Graphql.Mutations;

[ExtendObjectType("Mutation")]
public class FeedbackMutation
{
    /// <summary>
    /// Добавить отзыв.
    /// </summary>
    /// <param name="feedbackService"></param>
    /// <param name="createFeedback">Данные для добавления отзыва.</param>
    /// <returns>Идентификатор отзыва.</returns>
    [GraphQLName("AddFeedback")]
    [Authorize]
    public async Task<IdResponse> AddFeedback([Service(ServiceKind.Resolver)] IFeedbackService feedbackService, 
        CreateFeedback createFeedback)
    {
        var feedbackId = await feedbackService.AddFeedbackAsync(createFeedback.ZoneId, 
            createFeedback.UserId,
            createFeedback.Mark, 
            createFeedback.Message);
            
        return new IdResponse(feedbackId);
    }
    
    /// <summary>
    /// Удалить отзыв.
    /// </summary>
    /// <param name="feedbackService"></param>
    /// <param name="feedbackId">Идентификатор отзыва.</param>
    [GraphQLName("DeleteFeedback")]
    [Authorize]
    public async Task<IdResponse> DeleteFeedback([Service(ServiceKind.Resolver)] IFeedbackService feedbackService, 
        Guid feedbackId)
    {
        await feedbackService.RemoveFeedbackAsync(feedbackId);

        return new IdResponse(feedbackId);
    }
}