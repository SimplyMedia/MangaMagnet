namespace MangaMagnet.Core.CBZ.ComicInfo;

public interface IVersioned<out T>
{
	T GetForVersion(Version version);
}
