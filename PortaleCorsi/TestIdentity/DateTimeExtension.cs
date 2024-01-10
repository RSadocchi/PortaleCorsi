namespace TestIdentity
{
	public static class DateTimeExtension
	{
		public static int GetCenturyYear(this DateTime dateTime)
			=> (int)(dateTime.Year / 100) * 100;

		public static long ToUnixEpochDate(this DateTime dateTime)
			=> (long)Math.Round((dateTime.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
	}
}
