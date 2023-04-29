namespace ToolGood.FlowVision.Commons.Utils
{
	public static class RandomUtil
	{
		#region RandomInt

		/// <summary>
		/// 随机数
		/// </summary>
		/// <param name="maxValue"></param>
		/// <returns></returns>
		public static int RandomInt(int maxValue)
		{
			var rand = new Random(Guid.NewGuid().GetHashCode());
			var result = rand.Next(maxValue);
			return result;
		}

		/// <summary>
		/// 随机数
		/// </summary>
		/// <param name="minValue"></param>
		/// <param name="maxValue"></param>
		/// <returns></returns>
		public static int RandomInt(int minValue, int maxValue)
		{
			var rand = new Random(Guid.NewGuid().GetHashCode());
			var result = rand.Next(minValue, maxValue);
			return result;
		}

		#endregion RandomInt

		#region RandomDouble

		/// <summary>
		/// 随机数
		/// </summary>
		/// <param name="maxValue"></param>
		/// <returns></returns>
		public static double RandomDouble(int maxValue)
		{
			var rand = new Random(Guid.NewGuid().GetHashCode());
			var result = rand.NextDouble() * (double)maxValue;
			return result;
		}

		/// <summary>
		/// 随机数
		/// </summary>
		/// <param name="maxValue"></param>
		/// <returns></returns>
		public static double RandomDouble(double maxValue)
		{
			var rand = new Random(Guid.NewGuid().GetHashCode());
			var result = rand.NextDouble() * (double)maxValue;
			return result;
		}

		/// <summary>
		/// 随机数
		/// </summary>
		/// <param name="minValue"></param>
		/// <param name="maxValue"></param>
		/// <returns></returns>
		public static double RandomDouble(int minValue, int maxValue)
		{
			var rand = new Random(Guid.NewGuid().GetHashCode());
			var result = minValue + rand.NextDouble() * (double)(maxValue - minValue);
			return result;
		}

		/// <summary>
		/// 随机数
		/// </summary>
		/// <param name="minValue"></param>
		/// <param name="maxValue"></param>
		/// <returns></returns>
		public static double RandomDouble(double minValue, double maxValue)
		{
			var rand = new Random(Guid.NewGuid().GetHashCode());
			var result = minValue + rand.NextDouble() * (double)(maxValue - minValue);
			return result;
		}

		#endregion RandomDouble

		#region 生成指定长度的随机序列编号

		/// <summary>
		/// 生成指定长度的随机序列编号
		/// </summary>
		/// <param name="preFix">序列号前缀</param>
		///<param name="length">目标字符串的长度，最小长度为4，最大长度64</param>
		///<param name="useNum">是否包含数字，true=包含</param>
		///<param name="useLow">是否包含小写字母，true=包含</param>
		///<param name="useUpp">是否包含大写字母，true=包含</param>
		///<param name="useSpe">是否包含特殊字符，true=包含</param>
		///<param name="custom">要包含的自定义字符，直接输入要包含的字符列表</param>
		/// <returns>返回：前缀+DateTime.Now.Ticks+随机字符串</returns>
		public static string GenerateRandomSerialNo_UseTime(string preFix, int length, bool useNum = true, bool useLow = false, bool useUpp = false, bool useSpe = false, string custom = "")
		{
			string tickNo = DateTime.Now.ToString("yyyyMMddHHmmss").ToString();
			string randomNoStr = $"{preFix.Trim()}{tickNo}{GetRandomString(64, useNum, useLow, useUpp, useSpe, custom)}";
			if (length < 4) {
				length = 4;
			} else if (length > 64) {
				length = 64;
			}
			return (randomNoStr.Length > length ? randomNoStr.Substring(0, length) : randomNoStr);
		}

		#endregion 生成指定长度的随机序列编号

		#region 生成指定长度的随机序列编号

		/// <summary>
		/// 生成指定长度的随机序列编号
		/// </summary>
		/// <param name="preFix">序列号前缀</param>
		///<param name="length">目标字符串的长度，最小长度为4，最大长度64</param>
		///<param name="useNum">是否包含数字，true=包含</param>
		///<param name="useLow">是否包含小写字母，true=包含</param>
		///<param name="useUpp">是否包含大写字母，true=包含</param>
		///<param name="useSpe">是否包含特殊字符，true=包含</param>
		///<param name="custom">要包含的自定义字符，直接输入要包含的字符列表</param>
		/// <returns>返回：前缀+DateTime.Now.Ticks+随机字符串</returns>
		public static string GenerateRandomSerialNo(string preFix, int length, bool useNum = true, bool useLow = false, bool useUpp = false, bool useSpe = false, string custom = "")
		{
			string tickNo = DateTime.Now.Ticks.ToString();
			string randomNoStr = $"{preFix.Trim()}{tickNo}{GetRandomString(64, useNum, useLow, useUpp, useSpe, custom)}";
			if (length < 4) {
				length = 4;
			} else if (length > 64) {
				length = 64;
			}
			return (randomNoStr.Length > length ? randomNoStr.Substring(0, length) : randomNoStr);
		}

		#endregion 生成指定长度的随机序列编号

		#region 生成随机字符串

		///<summary>
		///生成随机字符串
		///</summary>
		///<param name="length">目标字符串的长度</param>
		///<param name="useNum">是否包含数字，true=包含</param>
		///<param name="useLow">是否包含小写字母，true=包含</param>
		///<param name="useUpp">是否包含大写字母，true=包含</param>
		///<param name="useSpe">是否包含特殊字符，true=包含</param>
		///<param name="custom">要包含的自定义字符，直接输入要包含的字符列表</param>
		///<returns>指定长度的随机字符串</returns>
		public static string GetRandomString(int length, bool useNum = true, bool useLow = false, bool useUpp = false, bool useSpe = false, string custom = "")
		{
			byte[] b = new byte[4];
			var r = new Random(Guid.NewGuid().GetHashCode());
			string s = null, str = custom;
			if (useNum == true) { str += "0123456789"; }
			if (useLow == true) { str += "abcdefghijklmnopqrstuvwxyz"; }
			if (useUpp == true) { str += "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; }
			if (useSpe == true) { str += "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~"; }
			for (int i = 0; i < length; i++) {
				s += str.Substring(r.Next(0, str.Length - 1), 1);
			}
			return s;
		}

		#endregion 生成随机字符串
	}
}