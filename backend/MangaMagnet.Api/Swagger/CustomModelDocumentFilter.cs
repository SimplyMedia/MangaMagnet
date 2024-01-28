using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MangaMagnet.Api.Swagger;

/// <summary>
/// Registers a custom model to the swagger document.
/// </summary>
/// <typeparam name="T">The type of the model.</typeparam>
public sealed class CustomModelDocumentFilter<T> : IDocumentFilter where T : class
{
	/// <inheritdoc />
	public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
	{
		context.SchemaGenerator.GenerateSchema(typeof(T), context.SchemaRepository);
	}
}
