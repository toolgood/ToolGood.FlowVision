using System.Security.Cryptography;
using System.Text;
using ToolGood.RcxCrypto;

namespace ToolGood.FlowVision.Commons.Utils
{
	public static class DataUtil
	{
		#region 测试

		private const string _baseDateTime = "2020-03-09 17:24:23";
		private const string _password = "123456789";
		private const string _salt = "{E1BB881A-D9AA-4D12-ABC1-AD1951A2611D}";

		#endregion 测试

		public static string CreatePassword(string username, string salt, string password)
		{
			return GetMd5String($"{username}|{salt}|{password}|{_salt}");
		}

		public static long DateTimeToLong(DateTime dateTime)
		{
			return (long)(dateTime - GetBaseTime()).TotalSeconds;
		}

		public static DateTime LongToDateTime(long seconds)
		{
			return GetBaseTime().AddSeconds(seconds);
		}

		public static long DateTimeNullToLong(DateTime? dateTime)
		{
			if (dateTime == null) {
				return 0;
			}
			return (long)(dateTime.Value - GetBaseTime()).TotalSeconds;
		}

		public static DateTime? LongToDateTimeNull(long seconds)
		{
			if (seconds == 0) {
				return null;
			}
			return GetBaseTime().AddSeconds(seconds);
		}

		public static byte[] StringEncrypt(string t)
		{
			if (string.IsNullOrEmpty(t)) {
				return new byte[0];
			}
			return GetThreeRCX().Encrypt(t, Encoding.UTF8);
		}

		public static string BytesDecrypt(byte[] bs)
		{
			if (bs == null || bs.Length == 0) {
				return "";
			}
			var bytes = GetThreeRCX().Encrypt(bs);
			return Encoding.UTF8.GetString(bytes);
		}

		private static DateTime? _GetBaseTime;

		private static DateTime GetBaseTime()
		{
			if (_GetBaseTime == null) {
				_GetBaseTime = DateTime.Parse(_baseDateTime);
			}
			return _GetBaseTime.Value;
		}

		private static ThreeRCX threeRCX;

		private static ThreeRCX GetThreeRCX()
		{
			if (threeRCX == null) {
				threeRCX = new ThreeRCX(_password, Encoding.UTF8);
			}
			return threeRCX;
		}

		private static string GetMd5String(byte[] buffer)
		{
			if (buffer == null) throw new ArgumentNullException("buffer");

			System.Security.Cryptography.MD5 md5 = MD5.Create();
			byte[] retVal = md5.ComputeHash(buffer);
			md5.Dispose();
			return BitConverter.ToString(retVal).Replace("-", "");
		}

		private static string GetMd5String(string text)
		{
			if (text == null) throw new ArgumentNullException("text");

			var buffer = System.Text.Encoding.UTF8.GetBytes(text);
			return GetMd5String(buffer);
		}
	}
}