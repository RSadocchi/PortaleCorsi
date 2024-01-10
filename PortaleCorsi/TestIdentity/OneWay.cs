using System.Text;
using System.Security.Cryptography;

namespace TestIdentity
{
	public static class OneWay
	{
		public static string MD5Hash(string text)
		{
			var sb = new StringBuilder();
			MD5CryptoServiceProvider md5cs = new MD5CryptoServiceProvider();
			byte[] bs = md5cs.ComputeHash(Encoding.UTF8.GetBytes(text));
			foreach (byte b in bs)
				sb.Append(b.ToString("x2"));
			return sb.ToString();
		}
		public static string ToMD5(this string source)
		{
			if (string.IsNullOrWhiteSpace(source)) return null;
			return OneWay.MD5Hash(source);
		}

		public static string SHA1Hash(string text)
		{
			var sb = new StringBuilder();
			SHA1CryptoServiceProvider sha1cs = new SHA1CryptoServiceProvider();
			byte[] bs = sha1cs.ComputeHash(Encoding.UTF8.GetBytes(text));
			foreach (byte b in bs)
				sb.Append(b.ToString());
			return sb.ToString();
		}
		public static string? ToSHA1(this string source)
		{
			if (string.IsNullOrWhiteSpace(source)) return null;
			return OneWay.SHA1Hash(source);
		}
	}
}
