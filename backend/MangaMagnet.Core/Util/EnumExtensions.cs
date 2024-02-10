using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace MangaMagnet.Core.Util;

public static class EnumExtensions
{
	/// <summary>
	///     A generic extension method that aids in reflecting 
	///     and retrieving any attribute that is applied to an `Enum`.
	/// </summary>
	public static string GetDisplayName(this Enum enumValue) 
	{
		return enumValue.GetType()
			.GetMember(enumValue.ToString())
			.First()
			.GetCustomAttribute<DisplayAttribute>()
			?.Name ?? throw new InvalidOperationException();
	}
}
