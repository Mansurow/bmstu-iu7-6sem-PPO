namespace Portal.Graphql.Filters;

public class GraphQlErrorFilter: IErrorFilter
{
    public IError OnError(IError error)
    {
        return error.WithMessage(error.Exception.Message);
    }
}