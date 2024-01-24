using AspNetCore.ExceptionHandler;

namespace MangaMagnet.Api.Exceptions;

public class NotFoundException(string message) : HttpException(404), IExplainableException
{
	public object Explain()
		=> message;
}
