using System;
using System.Collections.Specialized;
using System.Web;
using RemoteMergeUtility.Models;

namespace RemoteMergeUtility.Services
{
	public class UrlSchemeService : IUrlSchemeService
	{
		private const string EXPECTED_SCHEME = "mergeutil";

		public bool IsValidScheme(string url)
		{
			if (string.IsNullOrWhiteSpace(url))
				return false;

			try
			{
				var uri = new Uri(url);
				return string.Equals(uri.Scheme, EXPECTED_SCHEME, StringComparison.OrdinalIgnoreCase);
			}
			catch
			{
				return false;
			}
		}

		public UrlSchemeRequest? ParseUrl(string url)
		{
			if (!IsValidScheme(url))
				return null;

			try
			{
				var uri = new Uri(url);
				var queryParams = HttpUtility.ParseQueryString(uri.Query);

				// targetパラメータの取得
				var target = queryParams["target"] ?? string.Empty;

				// revisionパラメータの取得
				var revisionString = queryParams["revision"];
				TryParseRevision(revisionString, out var revision);

				// argsパラメータの取得（オプション）
				var args = queryParams["args"];

				var request = new UrlSchemeRequest(target, revision, args, url);

				return request.IsValid ? request : null;
			}
			catch
			{
				return null;
			}
		}

		private static bool TryParseRevision(string? revisionString, out int revision)
		{
			revision = 0;

			if (string.IsNullOrWhiteSpace(revisionString))
				return false;

			if (!int.TryParse(revisionString, out revision))
				return false;

			if (revision <= 0)
			{
				revision = 0;
				return false;
			}

			return true;
		}
	}
}