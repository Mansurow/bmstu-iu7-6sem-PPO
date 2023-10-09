using BookingDB = Portal.Database.Models.BookingDbModel;
using BookingDto = Portal.Common.Dto.Booking.Booking;
using BookingCore = Portal.Common.Core.Booking;

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
        public static BookingCore ConvertDBToCoreModel(BookingDB booking) 
        {
            return new BookingCore(id: booking.Id,
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
        /// <returns>Модель базы данных</returns>
        public static BookingDB ConvertCoreToDBModel(BookingCore booking)
        {
            return new BookingDB(id: booking.Id,
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
        /// Преобразовать из модели бизнес логики в модели DTO
        /// </summary>
        /// <param name="booking">Модель бизнес логики</param>
        /// <returns>Модель DTO</returns>
        public static BookingDto ConvertCoreToDtoModel(BookingCore booking)
        {
            return new BookingDto(id: booking.Id,
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
