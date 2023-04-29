using ToolGood.FlowVision.Commons.My;
using ToolGood.FlowVision.Commons.Utils;

namespace ToolGood.FlowVision.Commons
{
	public class RsaHelper
	{
		private const string _rsaPath = "/App_Data/Rsa.xml";
		private static string _privateKey = null;
		private static string _exponent = null;
		private static string _modulus = null;
		//private static DateTime? _timeout = null;

		public string RsaExponent { get; private set; }
		public string RsaModulus { get; private set; }
		public string PrivateKey { get; private set; }

		public string CookiePassword { get; private set; }

		public static RsaHelper Instance {
			get
			{
				GetPrivateKey();
				return new RsaHelper() {
					PrivateKey = _privateKey,
					RsaExponent = _exponent,
					RsaModulus = _modulus,
					CookiePassword = HashUtil.GetMd5String(_privateKey)
				};
			}
		}

		private static void GetPrivateKey()
		{
			if (_privateKey == null /*|| _timeout == null || _timeout.Value < DateTime.Now*/) {
				var path = MyHostingEnvironment.MapPath(_rsaPath);
				string privateKey;
				if (System.IO.File.Exists(path) == false) {
					RsaUtil.CreateKey(out privateKey);
					System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path));
					System.IO.File.WriteAllText(path, privateKey);
				} else {
					privateKey = System.IO.File.ReadAllText(path);
				}

				RsaUtil.GetParams(privateKey, out string modulus, out string exponent);
				_exponent = exponent;
				_modulus = modulus;
				_privateKey = privateKey;
				//_timeout = DateTime.Now.AddSeconds(60);
			}
		}

		public static void BuildPrivateKey()
		{
			var path = MyHostingEnvironment.MapPath(_rsaPath);
			string privateKey;
			RsaUtil.CreateKey(out privateKey);
			System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path));
			System.IO.File.WriteAllText(path, privateKey);

			RsaUtil.GetParams(privateKey, out string modulus, out string exponent);
			_exponent = exponent;
			_modulus = modulus;
			_privateKey = privateKey;
			//_timeout = DateTime.Now.AddSeconds(60);
		}

		public byte[] PrivateDecrypt(byte[] bytes)
		{
			return RsaUtil.PrivateDecrypt(_privateKey, bytes);
		}

		public byte[] PrivateDecrypt(string txt)
		{
			var bytes = Base64Util.FromBase64ForUrlString(txt);
			return RsaUtil.PrivateDecrypt(_privateKey, bytes);
		}

		public byte[] PrivateEncrypt(string txt)
		{
			return RsaUtil.PrivateEncrypt(_privateKey, System.Text.Encoding.UTF8.GetBytes(txt));
		}

		public byte[] PrivateEncrypt(byte[] bytes)
		{
			return RsaUtil.PrivateEncrypt(_privateKey, bytes);
		}
	}
}