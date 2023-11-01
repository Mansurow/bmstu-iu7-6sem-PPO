using HotChocolate.Authorization;
using Portal.Common.Converter;
using Portal.Common.Dto.Feedback;
using Portal.Common.Enums;
using Portal.Services.FeedbackService;

namespace Portal.Graphql.Queries;

[ExtendObjectType("Query")]
public class FeedbackQuery
{
    /// <summary>
    /// Пполучить все отзывы зон.
    /// </summary>
    /// <param name="feedbackService"></param>
    /// <returns>Список отзывов зоны.</returns>
    [GraphQLName("GetFeedbacks")]
    [Authorize(Roles = new []{nameof(Role.Administrator)})]
    public async Task<IEnumerable<Feedback>> GetFeedbacks(
        [Service(ServiceKind.Resolver)] IFeedbackService feedbackService)
    {
        var feedbacks = await feedbackService.GetAllFeedbackAsync();

        return feedbacks.Select(FeedbackConverter.ConvertCoreToDtoModel);
    }
    
    /// <summary>
    /// Получить отзывы зоны.
    /// </summary>
    /// <param name="feedbackService"></param>
    /// <param name="zoneId">Идентификатор зоны.</param>
    /// <returns>Список отзывов зоны.</returns>
    [GraphQLName("GetZoneFeedbacks")]
    public async Task<IEnumerable<Feedback>> GetZoneFeedbacks(
        [Service(ServiceKind.Resolver)] IFeedbackService feedbackService, Guid zoneId)
    {
        var feedbacks = await feedbackService.GetAllFeedbackByZoneAsync(zoneId);

        return feedbacks.Select(FeedbackConverter.ConvertCoreToDtoModel);
    }
}