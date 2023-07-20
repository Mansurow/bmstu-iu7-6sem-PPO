using Portal.Common.Models;
using Portal.Database.Models;

namespace Portal.Common.Converter
{
    public static class BookingConverter
    {
        public static Booking ConvertDbModelToAppModel(BookingDbModel booking) 
        {
            return new Booking(id: booking.Id,
                               zoneId: booking.ZoneId,
                               userId: booking.UserId,
                               packageId: booking.PackageId,
                               amountPeople: booking.AmountPeople,
                               date: booking.Date,
                               startTime: booking.StartTime,
                               endTime: booking.EndTime,
                               status: booking.Status);
        }

        public static BookingDbModel ConvertAppModelToDbModel(Booking booking)
        {
            return new BookingDbModel(id: booking.Id,
                               zoneId: booking.ZoneId,
                               userId: booking.UserId,
                               packageId: booking.PackageId,
                               amountPeople: booking.AmountPeople,
                               date: booking.Date,
                               startTime: booking.StartTime,
                               endTime: booking.EndTime,
                               status: booking.Status);
        }
    }
}
