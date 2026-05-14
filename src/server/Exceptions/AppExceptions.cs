public abstract class AppException : Exception
{
    public int StatusCode { get; set; }
    public AppException(string message, int statusCode) : base(message)
    {
        StatusCode = statusCode;
    }
}   

public class BadRequestException : AppException
{
    public BadRequestException(string message = "Bad Request") : base(message, 400)
    {
    }
}

public class UnauthorizedException : AppException
{
    public UnauthorizedException(string message = "Unauthorized") : base(message, 401)
    {
    }
}

public class ForbiddenException : AppException
{
    public ForbiddenException(string message = "Forbidden") : base(message, 403)
    {
    }
}

public class NotFoundException : AppException
{
    public NotFoundException(string message = "Not Found") : base(message, 404)
    {
    }
}

public class ConflictException : AppException
{
    public ConflictException(string message = "Conflict") : base(message, 409)
    {
    }
}

public class InternalServerException : AppException
{
    public InternalServerException(string message = "Internal Server Error") : base(message, 500)
    {
    }
}