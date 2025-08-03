using System;

namespace RemoteMergeUtility.Services
{
	public static class LogService
	{
		private const string APP_PREFIX = "[APP]";

		public static void Debug(string message)
		{
			System.Diagnostics.Debug.WriteLine($"{APP_PREFIX} DEBUG: {message}");
		}

		public static void Information(string message)
		{
			System.Diagnostics.Debug.WriteLine($"{APP_PREFIX} INFO: {message}");
		}

		public static void Warning(string message)
		{
			System.Diagnostics.Debug.WriteLine($"{APP_PREFIX} WARN: {message}");
		}

		public static void Error(string message)
		{
			System.Diagnostics.Debug.WriteLine($"{APP_PREFIX} ERROR: {message}");
		}

		public static void Error(string message, Exception exception)
		{
			System.Diagnostics.Debug.WriteLine($"{APP_PREFIX} ERROR: {message}");
			System.Diagnostics.Debug.WriteLine($"{APP_PREFIX} EXCEPTION: {exception}");
		}
	}
}