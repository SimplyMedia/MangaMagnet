namespace MangaMagnet.Core.Util;

public static class DoubleExtensions
{
	public static bool HasNoFloatingPoint(this double value)
		=> Math.Abs(value % 1) <= (double.Epsilon * 100);

}
