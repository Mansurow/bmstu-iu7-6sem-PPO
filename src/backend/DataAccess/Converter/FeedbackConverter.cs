using Anticafe.BL.Models;
using Anticafe.DataAccess.DBModels;

namespace Anticafe.DataAccess.Converter;

public static class FeedbackConverter
{
    public static Feedback ConvertDbModelToAppModel(FeedbackDbModel feedback) 
    {
        return new Feedback(id: feedback.Id,
                            userId: feedback.UserId,
                            roomId: feedback.RoomId,
                            date: feedback.Date,
                            mark: feedback.Mark,
                            message: feedback.Message);
    }

    public static FeedbackDbModel ConvertAppModelToDbModel(Feedback feedback)
    {
        return new FeedbackDbModel(id: feedback.Id,
                            userId: feedback.UserId,
                            roomId: feedback.RoomId,
                            date: feedback.Date,
                            mark: feedback.Mark,
                            message: feedback.Message);
    }
}
