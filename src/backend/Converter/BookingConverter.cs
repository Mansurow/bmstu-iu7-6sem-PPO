using Anticafe.BL.Models;
using Anticafe.DataAccess.DBModels;
using Common.Models.DTO;

namespace Anticafe.DataAccess.Converter
{
    public static class BookingConverter
    {
        public static Booking ConvertDbModelToAppModel(BookingDbModel booking) 
        {
            return new Booking(id: booking.Id,
                               roomId: booking.RoomId,
                               userId: booking.UserId,
                               amountPeople: booking.AmountPeople,
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
                               startTime: booking.StartTime,
                               endTime: booking.EndTime,
                               status: booking.Status);
        }

        public static BookingDto ConvertAppModelToDto(Booking booking)
        {
            return new BookingDto(id: booking.Id,
                               roomId: booking.RoomId,
                               userId: booking.UserId,
                               amountPeople: booking.AmountPeople,
                               startTime: booking.StartTime.ToString(),
                               endTime: booking.EndTime.ToString(),
                               status: booking.Status);
        }
    }
}
