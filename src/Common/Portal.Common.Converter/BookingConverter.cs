using Portal.Database.Models;
using Portal.Common.Models;

namespace Portal.Converter
{
    public static class BookingConverter
    {
        public static Booking ConvertDbModelToAppModel(BookingDbModel booking) 
        {
            return new Booking(id: booking.Id,
                               roomId: booking.RoomId,
                               userId: booking.UserId,
                               amountPeople: booking.AmountPeople,
                               date: booking.Date,
                               startTime: booking.StartTime,
                               endTime: booking.EndTime,
                               status: booking.Status);
        }

        public static BookingDbModel ConvertAppModelToDbModel(Booking booking)
        {
            return new BookingDbModel(id: booking.Id,
                               roomId: booking.RoomId,
                               userId: booking.UserId,
                               amountPeople: booking.AmountPeople,
                               date: booking.Date,
                               startTime: booking.StartTime,
                               endTime: booking.EndTime,
                               status: booking.Status);
        }
    }
}
