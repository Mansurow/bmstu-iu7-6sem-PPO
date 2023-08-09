using Portal.Common.Models;
using Portal.Database.Models;

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
    public static Feedback ConvertDbModelToAppModel(FeedbackDbModel feedback) 
    {
        return new Feedback(id: feedback.Id,
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
    public static FeedbackDbModel ConvertAppModelToDbModel(Feedback feedback)
    {
        return new FeedbackDbModel(id: feedback.Id,
        userId: feedback.UserId,
        zoneId: feedback.ZoneId,
        date: feedback.Date,
        mark: feedback.Mark,
        message: feedback.Message);
    }
}
