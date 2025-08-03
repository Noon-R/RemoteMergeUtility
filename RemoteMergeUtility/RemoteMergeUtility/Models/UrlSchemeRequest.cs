namespace RemoteMergeUtility.Models
{
	public class UrlSchemeRequest
	{
		public string Target { get; }
		public int Revision { get; }
		public string? Args { get; }
		public string OriginalUrl { get; }

		public bool IsValid => !string.IsNullOrEmpty(Target) && Revision > 0;

		public UrlSchemeRequest(string target, int revision, string? args, string originalUrl)
		{
			Target = target ?? string.Empty;
			Revision = revision;
			Args = args;
			OriginalUrl = originalUrl ?? string.Empty;
		}
	}
}