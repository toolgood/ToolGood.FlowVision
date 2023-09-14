using System.Text;

namespace ToolGood.RcxCrypto
{
	internal class RC4
	{
		private byte[] keybox;
		private const int keyLen = 256;
		private Encoding encoding;

		public RC4(string pass)
		{
			if (string.IsNullOrEmpty(pass)) throw new ArgumentNullException("pass");
			var ps = Encoding.UTF8.GetBytes(pass);
			encoding = Encoding.UTF8;
			keybox = GetKey(ps, keyLen);
		}

		public RC4(string pass, Encoding encoding)
		{
			if (string.IsNullOrEmpty(pass)) throw new ArgumentNullException("pass");
			var ps = encoding.GetBytes(pass);
			this.encoding = encoding;
			keybox = GetKey(ps, keyLen);
		}

		/// <summary>
		/// Encrypt
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public byte[] Encrypt(string data)
		{
			if (string.IsNullOrEmpty(data)) throw new ArgumentNullException("data");
			return encrypt(encoding.GetBytes(data));
		}

		/// <summary>
		/// Encrypt
		/// </summary>
		/// <param name="data"></param>
		/// <param name="encoding"></param>
		/// <returns></returns>
		public byte[] Encrypt(string data, Encoding encoding)
		{
			if (string.IsNullOrEmpty(data)) throw new ArgumentNullException("data");
			return encrypt(encoding.GetBytes(data));
		}

		/// <summary>
		/// Encrypt
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public byte[] Encrypt(byte[] data)
		{
			if (data == null) throw new ArgumentNullException("data");
			if (data.Length == 0) throw new ArgumentNullException("data");
			return encrypt(data);
		}

		private unsafe byte[] encrypt(byte[] data)
		{
			byte[] mBox = new byte[keyLen];
			//Buffer.BlockCopy(keybox, 0, mBox, 0, keyLen);

			Array.Copy(keybox, mBox, keyLen);
			byte[] output = new byte[data.Length];

			fixed (byte* _mBox = &mBox[0])
			fixed (byte* _data = &data[0])
			fixed (byte* _output = &output[0]) {
				var length = data.Length;
				int i = 0, j = 0;
				for (Int64 offset = 0; offset < length; offset++) {
					i = (++i) & 0xFF;
					j = (j + *(_mBox + i)) & 0xFF;
					byte temp = *(_mBox + i);
					*(_mBox + i) = *(_mBox + j);
					*(_mBox + j) = temp;

					byte a = *(_data + offset);
					byte b = *(_mBox + ((*(_mBox + i) + *(_mBox + j)) & 0xFF));
					*(_output + offset) = (byte)((int)a ^ (int)b);
				}
			}

			//for (Int64 offset = 0; offset < data.Length; offset++) {
			//    i = (++i) & 0xFF;
			//    j = (j + mBox[i]) & 0xFF;
			//    byte temp = mBox[i];
			//    mBox[i] = mBox[j];
			//    mBox[j] = temp;

			//    byte a = data[offset];
			//    byte b = mBox[(mBox[i] + mBox[j]) & 0xFF];
			//    output[offset] = (byte)((int)a ^ (int)b);
			//}
			return output;
		}

		/// <summary>
		/// Encrypt
		/// </summary>
		/// <param name="data"></param>
		/// <param name="pass"></param>
		/// <param name="encoding"></param>
		/// <returns></returns>
		public static byte[] Encrypt(string data, string pass, Encoding encoding)
		{
			if (string.IsNullOrEmpty(data)) throw new ArgumentNullException("data");
			if (string.IsNullOrEmpty(pass)) throw new ArgumentNullException("pass");

			return encrypt(encoding.GetBytes(data), encoding.GetBytes(pass));
		}

		/// <summary>
		/// Encrypt
		/// </summary>
		/// <param name="data"></param>
		/// <param name="pass"></param>
		/// <returns></returns>
		public static byte[] Encrypt(string data, string pass)
		{
			if (string.IsNullOrEmpty(data)) throw new ArgumentNullException("data");
			if (string.IsNullOrEmpty(pass)) throw new ArgumentNullException("pass");

			return encrypt(Encoding.UTF8.GetBytes(data), Encoding.UTF8.GetBytes(pass));
		}

		/// <summary>
		/// Encrypt
		/// </summary>
		/// <param name="data"></param>
		/// <param name="pass"></param>
		/// <returns></returns>
		public static byte[] Encrypt(byte[] data, string pass, Encoding encoding)
		{
			if (data == null) throw new ArgumentNullException("data");
			if (data.Length == 0) throw new ArgumentNullException("data");
			if (string.IsNullOrEmpty(pass)) throw new ArgumentNullException("pass");

			return encrypt(data, encoding.GetBytes(pass));
		}

		/// <summary>
		/// Encrypt
		/// </summary>
		/// <param name="data"></param>
		/// <param name="pass"></param>
		/// <returns></returns>
		public static byte[] Encrypt(byte[] data, string pass)
		{
			if (data == null) throw new ArgumentNullException("data");
			if (data.Length == 0) throw new ArgumentNullException("data");
			if (string.IsNullOrEmpty(pass)) throw new ArgumentNullException("pass");

			return encrypt(data, Encoding.UTF8.GetBytes(pass));
		}

		/// <summary>
		/// Encrypt
		/// </summary>
		/// <param name="data"></param>
		/// <param name="pass"></param>
		/// <returns></returns>
		public static byte[] Encrypt(string data, byte[] pass)
		{
			if (string.IsNullOrEmpty(data)) throw new ArgumentNullException("data");
			if (pass == null) throw new ArgumentNullException("pass");
			if (pass.Length == 0) throw new ArgumentNullException("pass");

			return encrypt(Encoding.UTF8.GetBytes(data), pass);
		}

		/// <summary>
		/// Encrypt
		/// </summary>
		/// <param name="data"></param>
		/// <param name="pass"></param>
		/// <param name="encoding"></param>
		/// <returns></returns>
		public static byte[] Encrypt(string data, byte[] pass, Encoding encoding)
		{
			if (string.IsNullOrEmpty(data)) throw new ArgumentNullException("data");
			if (pass == null) throw new ArgumentNullException("pass");
			if (pass.Length == 0) throw new ArgumentNullException("pass");

			return encrypt(encoding.GetBytes(data), pass);
		}

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

		private static unsafe byte[] encrypt(byte[] data, byte[] pass)
		{
			if (data == null) throw new ArgumentNullException("data");
			if (pass == null) throw new ArgumentNullException("pass");

			byte[] mBox = GetKey(pass, keyLen);
			byte[] output = new byte[data.Length];

			fixed (byte* _mBox = &mBox[0])
			fixed (byte* _data = &data[0])
			fixed (byte* _output = &output[0]) {
				var length = data.Length;
				int i = 0, j = 0;
				for (Int64 offset = 0; offset < length; offset++) {
					i = (++i) & 0xFF;
					j = (j + *(_mBox + i)) & 0xFF;
					byte temp = *(_mBox + i);
					*(_mBox + i) = *(_mBox + j);
					*(_mBox + j) = temp;

					byte a = *(_data + offset);
					byte b = *(_mBox + ((*(_mBox + i) + *(_mBox + j)) & 0xFF));
					*(_output + offset) = (byte)((int)a ^ (int)b);
				}
			}

			//for (Int64 offset = 0; offset < data.Length; offset++) {
			//    i = (++i) & 0xFF;
			//    j = (j + mBox[i]) & 0xFF;
			//    byte temp = mBox[i];
			//    mBox[i] = mBox[j];
			//    mBox[j] = temp;

			//    byte a = data[offset];
			//    byte b = mBox[(mBox[i] + mBox[j]) & 0xFF];
			//    output[offset] = (byte)((int)a ^ (int)b);
			//}
			return output;
		}

		private static unsafe byte[] GetKey(byte[] pass, int kLen)
		{
			byte[] mBox = new byte[kLen];
			fixed (byte* _mBox = &mBox[0]) {
				for (Int64 i = 0; i < kLen; i++) {
					*(_mBox + i) = (byte)i;
				}
				Int64 j = 0;
				int lengh = pass.Length;
				fixed (byte* _pass = &pass[0]) {
					for (Int64 i = 0; i < kLen; i++) {
						j = (j + *(_mBox + i) + *(_pass + (i % lengh))) % kLen;
						byte temp = *(_mBox + i);
						*(_mBox + i) = *(_mBox + j);
						*(_mBox + j) = temp;
					}
				}
			}

			//for (Int64 i = 0; i < kLen; i++) {
			//    mBox[i] = (byte)i;
			//}
			//Int64 j = 0;
			//for (Int64 i = 0; i < kLen; i++) {
			//    j = (j + mBox[i] + pass[i % pass.Length]) % kLen;
			//    byte temp = mBox[i];
			//    mBox[i] = mBox[j];
			//    mBox[j] = temp;
			//}
			return mBox;
		}
	}
}