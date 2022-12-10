using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolGood.FlowVision.Common
{
	/// <summary>
	/// RCY algorithm
	/// Author : Lin ZhiJun
	/// NickName : ToolGood
	/// Email : toolgood@qq.com
	/// 
	/// See https://github.com/toolgood/RCY
	/// </summary>
	static class RCY
	{
		private const int keyLen = 256;

		/// <summary>
		/// Encrypt
		/// </summary>
		/// <param name="data"></param>
		/// <param name="pass"></param>
		/// <returns></returns>
		public static byte[] Encrypt(byte[] data, byte[] pass)
		{
			if (data == null) throw new ArgumentNullException("data");
			if (data.Length == 0) throw new ArgumentNullException("data");
			if (pass == null) throw new ArgumentNullException("pass");
			if (pass.Length == 0) throw new ArgumentNullException("pass");

			return encrypt(data, pass);
		}
		private unsafe static byte[] encrypt(byte[] data, byte[] pass)
		{
			byte[] mBox = GetKey(pass, keyLen);
			byte[] output = new byte[data.Length];
			int i = 0, j = 0;
			var length = data.Length;


			fixed (byte* _mBox = &mBox[0])
			fixed (byte* _data = &data[0])
			fixed (byte* _output = &output[0]) {
				for (long offset = 0; offset < length; offset++) {
					i = ++i & 0xFF;
					j = j + *(_mBox + i) & 0xFF;

					byte a = *(_data + offset);
					byte c = (byte)(a ^ *(_mBox + (*(_mBox + i) & *(_mBox + j))));
					*(_output + offset) = c;

					j = j + a + c;
				}
			}

			return output;
		}



		private static unsafe byte[] GetKey(byte[] pass, int kLen)
		{
			byte[] mBox = new byte[kLen];
			fixed (byte* _mBox = &mBox[0]) {
				for (long i = 0; i < kLen; i++) {
					*(_mBox + i) = (byte)i;
				}
				long j = 0;
				int lengh = pass.Length;
				fixed (byte* _pass = &pass[0]) {
					for (long i = 0; i < kLen; i++) {
						j = (j + *(_mBox + i) + *(_pass + i % lengh)) % kLen;
						byte temp = *(_mBox + i);
						*(_mBox + i) = *(_mBox + j);
						*(_mBox + j) = temp;
					}
				}
			}
			return mBox;
		}
	}
}
