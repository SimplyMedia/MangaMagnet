using AspNetCore.ExceptionHandler;

namespace MangaMagnet.Api.Exceptions;

public class AlreadyExistException(string message) : HttpException(409), IExplainableException
{
	public object Explain()
		=> message;
}
