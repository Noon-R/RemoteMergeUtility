using RemoteMergeUtility.Models;

namespace RemoteMergeUtility.Services
{
	public interface IUrlSchemeService
	{
		UrlSchemeRequest? ParseUrl(string url);
		bool IsValidScheme(string url);
	}
}