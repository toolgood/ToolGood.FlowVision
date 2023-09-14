using System.Text;

namespace ToolGood.RcxCrypto
{
	internal class ThreeRCX
	{
		private RCX rcx;

		public ThreeRCX(string pass)
		{
			rcx = new RCX(pass);
		}

		public ThreeRCX(string pass, Encoding encoding)
		{
			rcx = new RCX(pass, encoding);
		}

		/// <summary>
		/// Encrypt
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public byte[] Encrypt(string data)
		{
			if (string.IsNullOrEmpty(data)) throw new ArgumentNullException("data");
			return rcx.Encrypt(rcx.Encrypt(rcx.Encrypt(data), OrderType.Desc));
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
			return rcx.Encrypt(rcx.Encrypt(rcx.Encrypt(data, encoding), OrderType.Desc));
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
			return rcx.Encrypt(rcx.Encrypt(rcx.Encrypt(data), OrderType.Desc));
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
			var rcx = new RCX(pass, encoding);
			return rcx.Encrypt(rcx.Encrypt(rcx.Encrypt(data), OrderType.Desc));
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
			var rcx = new RCX(pass);
			return rcx.Encrypt(rcx.Encrypt(rcx.Encrypt(data), OrderType.Desc));
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
			var rcx = new RCX(pass, encoding);
			return rcx.Encrypt(rcx.Encrypt(rcx.Encrypt(data), OrderType.Desc));
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
			var rcx = new RCX(pass);
			return rcx.Encrypt(rcx.Encrypt(rcx.Encrypt(data), OrderType.Desc));
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
			var rcx = new RCX(pass);
			return rcx.Encrypt(rcx.Encrypt(rcx.Encrypt(data), OrderType.Desc));
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
			var rcx = new RCX(pass, encoding);
			return rcx.Encrypt(rcx.Encrypt(rcx.Encrypt(data), OrderType.Desc));
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
			var rcx = new RCX(pass);
			return rcx.Encrypt(rcx.Encrypt(rcx.Encrypt(data), OrderType.Desc));
		}
	}
}