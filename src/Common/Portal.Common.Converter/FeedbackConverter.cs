using FeedbackCore = Portal.Common.Core.Feedback;
using FeedbackDB = Portal.Database.Models.FeedbackDbModel;
using FeedbackDto = Portal.Common.Dto.Feedback.Feedback;

namespace Portal.Common.Converter;

/// <summary>
/// Конвертатор модели Feedback
/// </summary>
public static class FeedbackConverter
{
    /// <summary>
    /// Преобразовать из модели базы данных в модель бизнес логики приложения
    /// </summary>
    /// <param name="feedback">Модель базы данных</param>
    /// <returns>Модель бизнес логики</returns>
    public static FeedbackCore ConvertDBToCoreModel(FeedbackDB feedback) 
    {
        return new FeedbackCore(id: feedback.Id,
            userId: feedback.UserId,
            zoneId: feedback.ZoneId,
            date: feedback.Date,
            mark: feedback.Mark,
            message: feedback.Message);
    }

    /// <summary>
    /// Преобразовать из модели бизнес логики в модели базы данных приложения
    /// </summary>
    /// <param name="feedback">Модель бизнес логики</param>
    /// <returns>Модель базы данных </returns>
    public static FeedbackDB ConvertCoreToDBModel(FeedbackCore feedback)
    {
        return new FeedbackDB(id: feedback.Id,
        userId: feedback.UserId,
        zoneId: feedback.ZoneId,
        date: feedback.Date,
        mark: feedback.Mark,
        message: feedback.Message);
    }
    
    /// <summary>
    /// Преобразовать из модели бизнес логики в модели DTO
    /// </summary>
    /// <param name="feedback">Модель бизнес логики</param>
    /// <returns>Модель DTO</returns>
    public static FeedbackDto ConvertCoreToDtoModel(FeedbackCore feedback)
    {
        return new FeedbackDto(id: feedback.Id,
            userId: feedback.UserId,
            zoneId: feedback.ZoneId,
            date: feedback.Date,
            mark: feedback.Mark,
            message: feedback.Message);
    }
}
