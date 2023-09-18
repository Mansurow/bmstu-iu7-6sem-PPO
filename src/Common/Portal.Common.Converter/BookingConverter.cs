using Portal.Common.Models;
using Portal.Database.Models;

namespace Portal.Common.Converter
{
    /// <summary>
    /// Конвертатор модели Booking
    /// </summary>
    public static class BookingConverter
    {
        /// <summary>
        /// Преобразовать из модели базы данных в модель бизнес логики приложения
        /// </summary>
        /// <param name="booking">Модель базы данных</param>
        /// <returns>Модель бизнес логики</returns>
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
                               status: booking.Status,
                               createDateTime: booking.CreateDateTime,
                               isPaid: booking.IsPaid,
                               totalPrice: booking.TotalPrice);
        }

        /// <summary>
        /// Преобразовать из модели бизнес логики в модели базы данных приложения
        /// </summary>
        /// <param name="booking">Модель бизнес логики</param>
        /// <returns>Модель базы данных </returns>
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
                               status: booking.Status,
                               createDateTime: booking.CreateDateTime,
                               isPaid: booking.IsPaid,
                               totalPrice: booking.TotalPrice);
        }
    }
}
