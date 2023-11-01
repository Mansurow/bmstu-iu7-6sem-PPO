using Portal.Services.BookingService.Exceptions;
using Portal.Services.FeedbackService.Exceptions;
using Portal.Services.InventoryServices.Exceptions;
using Portal.Services.MenuService.Exceptions;
using Portal.Services.OauthService.Exceptions;
using Portal.Services.PackageService.Exceptions;
using Portal.Services.UserService.Exceptions;
using Portal.Services.ZoneService.Exceptions;

namespace Portal.Graphql.Filters;

public class GraphQlErrorFilter: IErrorFilter
{
    public IError OnError(IError error)
    {
        if (error.Exception is ZoneNotFoundException 
            or UserNotFoundException
            or BookingNotFoundException 
            or FeedbackNotFoundException 
            or InventoryNotFoundException 
            or DishNotFoundException 
            or PackageNotFoundException)
        {
            return error.WithCode("NOT_FOUND")
                .WithMessage(error.Exception.Message);
        }
        
        if (error.Exception is BookingExistsException
            or BookingReversedException
            or ZonePackageExistsException
            or BookingNotSuitableStatusException
            or BookingExceedsLimitException
            or BookingChangeDateTimeException)
        {
            return error.WithCode("BAD_REQUEST")
                .WithMessage(error.Exception.Message);
        }
        
        if (error.Exception is IncorrectPasswordException
            or UserLoginNotFoundException)
        {
            return error.WithCode("AUTH_NOT_AUTHORIZED")
                .WithMessage("The current user is not authorized to access this resource.");
        }
        
        if (error.Exception is BookingUpdateException 
            or BookingCreateException
            or BookingRemoveException
            or FeedbackCreateException
            or FeedbackUpdateException
            or FeedbackRemoveException
            or InventoryCreateException
            or InventoryUpdateException
            or InventoryRemoveException
            or DishCreateException
            or DishUpdateException
            or DishRemoveException
            or PackageCreateException
            or PackageUpdateException
            or PackageRemoveException
            or UserCreateException
            or UserUpdateException
            or ZoneCreateException
            or ZoneUpdateException
            or ZoneRemoveException)
        {
            return error.WithCode("INTERNAL SERVER ERROR")
                .WithMessage(error.Exception.Message);
        }
        
        return error;
    }
}