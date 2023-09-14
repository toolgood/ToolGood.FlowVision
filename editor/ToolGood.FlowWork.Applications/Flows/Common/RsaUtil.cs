using System.Numerics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace ToolGood.FlowWork.Commons
{
	/// <summary>
	/// RSA 加解密
	/// </summary>
	public sealed partial class RsaUtil
	{
		#region RSA 的密钥产生

		/// <summary>
		/// 获取参数
		/// </summary>
		/// <param name="xmlKeys"></param>
		/// <param name="Modulus"></param>
		/// <param name="Exponent"></param>
		public static void GetParams(string xmlKeys, out string Modulus, out string Exponent)
		{
			RSA rsa = new RSACryptoServiceProvider();
			LoadPrivateKey(rsa, xmlKeys);
			var p = rsa.ExportParameters(false);
			Modulus = BitConverter.ToString(p.Modulus).Replace("-", "");
			Exponent = BitConverter.ToString(p.Exponent).Replace("-", "");
			rsa = null;
		}

		/// <summary>
		/// RSA 的密钥产生 产生私钥 和公钥
		/// </summary>
		/// <param name="xmlKeys"></param>
		/// <param name="publicKey"></param>
		/// <param name="keySize"></param>
		public static void CreateXmlKey(out string privateKey, out string publicKey, int keySize = 1024)
		{
			RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(keySize);
			privateKey = SaveXmlString(rsa, true);
			publicKey = SaveXmlString(rsa, false);
			rsa = null;
		}

		public static void CreatePemKey(out string privateKey, out string publicKey, bool isPKCS8 = false, int keySize = 1024)
		{
			RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(keySize);
			privateKey = SavePemString(rsa, true, isPKCS8);
			publicKey = SavePemString(rsa, false, isPKCS8);
			rsa = null;
		}

		#endregion RSA 的密钥产生

		#region 公钥加密 私钥解密

		/// <summary>
		/// 公钥加密 返回Base64
		/// </summary>
		/// <param name="publicKey"></param>
		/// <param name="EncryptString"></param>
		/// <returns></returns>
		public static string PublicEncrypt(string publicKey, string EncryptString)
		{
			using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider()) {
				LoadPublicKey(rsa, publicKey);
				var bs = Encoding.UTF8.GetBytes(EncryptString);
				return Convert.ToBase64String(publicEncrypt(rsa, bs));
			}
		}

		/// <summary>
		/// 公钥加密
		/// </summary>
		/// <param name="publicKey"></param>
		/// <param name="bytes"></param>
		/// <returns></returns>
		public static byte[] PublicEncrypt(string publicKey, byte[] bytes)
		{
			using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider()) {
				LoadPublicKey(rsa, publicKey);
				return publicEncrypt(rsa, bytes);
			}
		}

		private static byte[] publicEncrypt(RSACryptoServiceProvider rsa, byte[] bytes)
		{
			var keySize = rsa.KeySize / 8 - 11;
			if (bytes.Length <= keySize) {
				return rsa.Encrypt(bytes, false);
			}
			using (MemoryStream ms = new MemoryStream()) {
				var index = 0;
				while (index < bytes.Length) {
					var length = Math.Min(keySize, bytes.Length - index);
					var bs = new byte[length];
					Array.Copy(bytes, index, bs, 0, length);
					var bs2 = rsa.Encrypt(bs, false);
					ms.Write(bs2, 0, bs2.Length);
					index += keySize;
				}
				return ms.ToArray();
			}
		}

		/// <summary>
		/// 私钥解密 返回 UTF8格式
		/// </summary>
		/// <param name="publicKey"></param>
		/// <param name="DecryptString">Base64格式</param>
		/// <returns></returns>
		public static string PrivateDecrypt(string publicKey, string DecryptString)
		{
			using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider()) {
				LoadPrivateKey(rsa, publicKey);

				var bytes = Base64.FromBase64ForUrlString(DecryptString);
				var b2 = privateDecrypt(rsa, bytes);
				return Encoding.UTF8.GetString(b2);
			}
		}

		/// <summary>
		/// 私钥解密
		/// </summary>
		/// <param name="privateKey"></param>
		/// <param name="bytes"></param>
		/// <returns></returns>
		public static byte[] PrivateDecrypt(string privateKey, byte[] bytes)
		{
			using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider()) {
				LoadPrivateKey(rsa, privateKey);
				return privateDecrypt(rsa, bytes);
			}
		}

		private static byte[] privateDecrypt(RSACryptoServiceProvider rsa, byte[] bytes)
		{
			var keySize = rsa.KeySize / 8;
			if (bytes.Length < keySize) {//修复 js base64化bug
				byte[] bs = new byte[keySize];
				Array.Copy(bytes, 0, bs, keySize - bytes.Length, bytes.Length);
				return rsa.Decrypt(bs, false);
			} else if (bytes.Length == keySize) {
				return rsa.Decrypt(bytes, false);
			}
			using (MemoryStream ms = new MemoryStream()) {
				var index = 0;
				while (index < bytes.Length) {
					var length = Math.Min(keySize, bytes.Length - index);
					var bs = new byte[length];
					Array.Copy(bytes, index, bs, 0, length);
					byte[] bs2 = rsa.Decrypt(bs, false);
					ms.Write(bs2, 0, bs2.Length);
					index += keySize;
				}
				return ms.ToArray();
			}
		}

		#endregion 公钥加密 私钥解密

		#region 私钥加密 公钥解密

		/// <summary>
		/// 私钥加密
		/// </summary>
		/// <param name="privateKey"></param>
		/// <param name="EncryptString"></param>
		/// <returns></returns>
		public static string PrivateEncrypt(string privateKey, string EncryptString)
		{
			using (RsaEncryption rsa = new RsaEncryption()) {
				rsa.LoadPrivateFromXml(privateKey);
				var bs = Encoding.UTF8.GetBytes(EncryptString);
				return Convert.ToBase64String(rsa.PrivateEncryption(bs));
			}
		}

		/// <summary>
		/// 私钥加密
		/// </summary>
		/// <param name="privateKey"></param>
		/// <param name="bytes"></param>
		/// <returns></returns>
		public static byte[] PrivateEncrypt(string privateKey, byte[] bytes)
		{
			using (RsaEncryption rsa = new RsaEncryption()) {
				rsa.LoadPrivateFromXml(privateKey);
				return rsa.PrivateEncryption(bytes);
			}
		}

		/// <summary>
		/// 公钥解密
		/// </summary>
		/// <param name="publicKey"></param>
		/// <param name="DecryptString"></param>
		/// <returns></returns>
		public static string PublicDecrypt(string publicKey, string DecryptString)
		{
			using (RsaEncryption rsa = new RsaEncryption()) {
				rsa.LoadPublicFromXml(publicKey);
				var bs = rsa.PublicDecryption(Convert.FromBase64String(DecryptString));
				return Encoding.UTF8.GetString(bs);
			}
		}

		/// <summary>
		/// 公钥解密
		/// </summary>
		/// <param name="publicKey"></param>
		/// <param name="bytes"></param>
		/// <returns></returns>
		public static byte[] PublicDecrypt(string publicKey, byte[] bytes)
		{
			using (RsaEncryption rsa = new RsaEncryption()) {
				rsa.LoadPublicFromXml(publicKey);
				return rsa.PublicDecryption(bytes);
			}
		}

		#endregion 私钥加密 公钥解密

		#region RSA签名 验签

		/// <summary>
		/// RSA签名,默认SHA256
		/// </summary>
		/// <param name="privateKey">私钥</param>
		/// <param name="HashbyteSignature"></param>
		/// <param name="EncryptedSignatureData"></param>
		/// <returns></returns>
		public static bool Sign(string privateKey, byte[] HashbyteSignature, out byte[] EncryptedSignatureData)
		{
			RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
			LoadPrivateKey(RSA, privateKey);
			SHA256 sh = SHA256.Create();
			EncryptedSignatureData = RSA.SignData(HashbyteSignature, sh);
			sh.Dispose();
			RSA.Dispose();
			return true;
		}

		/// <summary>
		/// RSA签名
		/// </summary>
		/// <param name="privateKey">私钥</param>
		/// <param name="hashType">md2,md5,sha1,sha224,sha256,sha384,sha512,ripemd160</param>
		/// <param name="HashbyteSignature"></param>
		/// <param name="EncryptedSignatureData"></param>
		/// <returns></returns>
		public static bool Sign(string privateKey, string hashType, byte[] HashbyteSignature, out byte[] EncryptedSignatureData)
		{
			RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
			LoadPrivateKey(RSA, privateKey);

			EncryptedSignatureData = RSA.SignData(HashbyteSignature, hashType);
			RSA.Dispose();
			return true;
		}

		/// <summary>
		/// 签名验签
		/// </summary>
		/// <param name="publicKey">公钥</param>
		/// <param name="HashbyteSignature"></param>
		/// <param name="signedData"></param>
		/// <param name="input_charset">编码格式</param>
		/// <returns>true(通过)，false(不通过)</returns>
		public static bool Verify(string publicKey, byte[] HashbyteSignature, byte[] signedData)
		{
			RSACryptoServiceProvider rsaPub = new RSACryptoServiceProvider();
			LoadPublicKey(rsaPub, publicKey);
			SHA256 sh = SHA256.Create();
			return rsaPub.VerifyData(HashbyteSignature, sh, signedData);
		}

		/// <summary>
		/// 签名验签
		/// </summary>
		/// <param name="publicKey">公钥</param>
		/// <param name="hashType">md2,md5,sha1,sha224,sha256,sha384,sha512,ripemd160</param>
		/// <param name="HashbyteSignature"></param>
		/// <param name="signedData"></param>
		/// <returns>true(通过)，false(不通过)</returns>
		/// 各种签名 填充值
		/// md2:"3020300c06082a864886f70d020205000410",
		/// md5:"3020300c06082a864886f70d020505000410",
		/// sha1:"3021300906052b0e03021a05000414",
		/// sha224:"302d300d06096086480165030402040500041c",
		/// sha256:"3031300d060960864801650304020105000420",
		/// sha384:"3041300d060960864801650304020205000430",
		/// sha512:"3051300d060960864801650304020305000440",
		/// ripemd160: "3021300906052b2403020105000414"
		public static bool Verify(string publicKey, string hashType, byte[] HashbyteSignature, byte[] signedData)
		{
			RSACryptoServiceProvider rsaPub = new RSACryptoServiceProvider();
			LoadPublicKey(rsaPub, publicKey);
			return rsaPub.VerifyData(HashbyteSignature, hashType, signedData);
		}

		#endregion RSA签名 验签

		#region 密钥解析

		private static Regex _PEMCode = new Regex(@"--+.+?--+|\s+", RegexOptions.IgnoreCase);
		private static byte[] _SeqOID = new byte[] { 0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00 };
		private static byte[] _Ver = new byte[] { 0x02, 0x01, 0x00 };

		#region LoadPublicKey LoadPrivateKey

		private static void LoadPrivateKey(RSA rsa, string key)
		{
			RSAParameters parameters;
			if (key.StartsWith('<')) {
				parameters = LoadXmlString(key);
			} else {
				try {
					parameters = FromPem(key, out bool pk);
					if (pk == false) { throw new Exception("It is not private key."); }
				} catch {
					parameters = LoadCertPrivateKey(key);
				}
			}
			rsa.ImportParameters(parameters);
		}

		private static void LoadPublicKey(RSA rsa, string key)
		{
			RSAParameters parameters;
			if (key.StartsWith('<')) {
				parameters = LoadXmlString(key);
			} else {
				try {
					parameters = FromPem(key, out bool _);
				} catch {
					parameters = LoadCertPublicKey(key);
				}
			}
			rsa.ImportParameters(parameters);
		}

		private static RSAParameters LoadXmlString(string xmlString)
		{
			RSAParameters parameters = new RSAParameters();
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.LoadXml(xmlString);
			if (xmlDoc.DocumentElement.Name.Equals("RSAKeyValue")) {
				foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes) {
					switch (node.Name) {
						case "Modulus": parameters.Modulus = Convert.FromBase64String(node.InnerText); break;
						case "Exponent": parameters.Exponent = Convert.FromBase64String(node.InnerText); break;
						case "P": parameters.P = Convert.FromBase64String(node.InnerText); break;
						case "Q": parameters.Q = Convert.FromBase64String(node.InnerText); break;
						case "DP": parameters.DP = Convert.FromBase64String(node.InnerText); break;
						case "DQ": parameters.DQ = Convert.FromBase64String(node.InnerText); break;
						case "InverseQ": parameters.InverseQ = Convert.FromBase64String(node.InnerText); break;
						case "D": parameters.D = Convert.FromBase64String(node.InnerText); break;
					}
				}
			} else {
				throw new Exception("Invalid XML RSA key.");
			}
			return parameters;
		}

		private static string SaveXmlString(RSAParameters parameters, bool includePrivateParameters)
		{
			if (includePrivateParameters) {
				return string.Format("<RSAKeyValue><Modulus>{0}</Modulus><Exponent>{1}</Exponent><P>{2}</P><Q>{3}</Q><DP>{4}</DP><DQ>{5}</DQ><InverseQ>{6}</InverseQ><D>{7}</D></RSAKeyValue>",
					Convert.ToBase64String(parameters.Modulus),
					Convert.ToBase64String(parameters.Exponent),
					Convert.ToBase64String(parameters.P),
					Convert.ToBase64String(parameters.Q),
					Convert.ToBase64String(parameters.DP),
					Convert.ToBase64String(parameters.DQ),
					Convert.ToBase64String(parameters.InverseQ),
					Convert.ToBase64String(parameters.D));
			}
			return string.Format("<RSAKeyValue><Modulus>{0}</Modulus><Exponent>{1}</Exponent></RSAKeyValue>",
				Convert.ToBase64String(parameters.Modulus),
				Convert.ToBase64String(parameters.Exponent));
		}

		private static RSAParameters LoadCertPublicKey(string certString)
		{
			var bytes = Encoding.Default.GetBytes(certString);
			X509Certificate2 c1 = new X509Certificate2(bytes);
			string keyPublic = c1.PublicKey.Key.ToXmlString(false);  // 公钥
			return LoadXmlString(keyPublic);
		}

		private static RSAParameters LoadCertPrivateKey(string certString)
		{
			var bytes = Encoding.Default.GetBytes(certString);
			X509Certificate2 c1 = new X509Certificate2(bytes);
			string keyPublic = c1.PrivateKey.ToXmlString(true);  // 公钥
			return LoadXmlString(keyPublic);
		}

		private static RSAParameters FromPem(string pem, out bool privateKey)
		{
			privateKey = false;
			var param = new RSAParameters();

			var base64 = _PEMCode.Replace(pem, "");
			var data = Convert.FromBase64String(base64);
			if (data == null) { throw new Exception("Pem content invalid "); }
			var idx = 0;

			//read  length
			Func<byte, int> readLen = (first) => {
				if (data[idx] == first) {
					idx++;
					if (data[idx] == 0x81) {
						idx++;
						return data[idx++];
					} else if (data[idx] == 0x82) {
						idx++;
						return (((int)data[idx++]) << 8) + data[idx++];
					} else if (data[idx] < 0x80) {
						return data[idx++];
					}
				}
				throw new Exception("Not found any content in pem file");
			};
			//read module length
			Func<byte[]> readBlock = () => {
				var len = readLen(0x02);
				if (data[idx] == 0x00) {
					idx++;
					len--;
				}
				var val = Sub(data, idx, len);
				idx += len;
				return val;
			};

			Func<byte[], bool> eq = (byts) => {
				for (var i = 0; i < byts.Length; i++, idx++) {
					if (idx >= data.Length) { return false; }
					if (byts[i] != data[idx]) { return false; }
				}
				return true;
			};

			if (pem.Contains("PUBLIC KEY")) {
				readLen(0x30);
				if (!eq(_SeqOID)) { throw new Exception("Unknown pem format"); }

				readLen(0x03);
				idx++;
				readLen(0x30);
				param.Modulus = readBlock();
				param.Exponent = readBlock();
			} else if (pem.Contains("PRIVATE KEY")) {
				readLen(0x30);
				//Read version
				if (!eq(_Ver)) { throw new Exception("Unknown pem version"); }
				//Check PKCS8
				var idx2 = idx;
				if (eq(_SeqOID)) {
					readLen(0x04);
					readLen(0x30);
					if (!eq(_Ver)) { throw new Exception("Pem version invalid"); }
				} else {
					idx = idx2;
				}
				param.Modulus = readBlock();
				param.Exponent = readBlock();
				param.D = readBlock();
				param.P = readBlock();
				param.Q = readBlock();
				param.DP = readBlock();
				param.DQ = readBlock();
				param.InverseQ = readBlock();
				privateKey = true;
			} else {
				throw new Exception("pem need 'BEGIN' and  'END'");
			}
			return param;
		}

		#endregion LoadPublicKey LoadPrivateKey

		#region SaveXmlString SavePemString

		private static string SaveXmlString(RSA rsa, bool includePrivateParameters)
		{
			RSAParameters parameters = rsa.ExportParameters(includePrivateParameters);
			return SaveXmlString(parameters, includePrivateParameters);
		}

		private static string SavePemString(RSA rsa, bool includePrivateParameters, bool isPKCS8 = false)
		{
			RSAParameters parameters = rsa.ExportParameters(includePrivateParameters);
			return ToPem(parameters, includePrivateParameters, isPKCS8);
		}

		/// <summary>
		/// Converter Rsa to pem ,
		/// </summary>
		/// <param name="rsa"><see cref="RSACryptoServiceProvider"/></param>
		/// <param name="includePrivateParameters">if false only return publick key</param>
		/// <param name="isPKCS8">default is false,if true return PKCS#8 pem else return PKCS#1 pem </param>
		/// <returns></returns>
		private static string ToPem(RSAParameters param, bool includePrivateParameters, bool isPKCS8 = false)
		{
			var ms = new MemoryStream();

			Action<int> writeLenByte = (len) => {
				if (len < 0x80) {
					ms.WriteByte((byte)len);
				} else if (len <= 0xff) {
					ms.WriteByte(0x81);
					ms.WriteByte((byte)len);
				} else {
					ms.WriteByte(0x82);
					ms.WriteByte((byte)(len >> 8 & 0xff));
					ms.WriteByte((byte)(len & 0xff));
				}
			};
			//write moudle data
			Action<byte[]> writeBlock = (byts) => {
				var addZero = (byts[0] >> 4) >= 0x8;
				ms.WriteByte(0x02);
				var len = byts.Length + (addZero ? 1 : 0);
				writeLenByte(len);

				if (addZero) { ms.WriteByte(0x00); }
				ms.Write(byts, 0, byts.Length);
			};

			Func<int, byte[], byte[]> writeLen = (index, byts) => {
				var len = byts.Length - index;
				ms.SetLength(0);
				ms.Write(byts, 0, index);
				writeLenByte(len);
				ms.Write(byts, index, len);
				return ms.ToArray();
			};

			if (!includePrivateParameters) {
				ms.WriteByte(0x30);
				var index1 = (int)ms.Length;

				// Encoded OID sequence for PKCS #1 rsaEncryption szOID_RSA_RSA = "1.2.840.113549.1.1.1"
				WriteAll(ms, _SeqOID);

				//Start with 0x00
				ms.WriteByte(0x03);
				var index2 = (int)ms.Length;
				ms.WriteByte(0x00);

				//Content length
				ms.WriteByte(0x30);
				var index3 = (int)ms.Length;

				//Write Modulus
				writeBlock(param.Modulus);

				//Write Exponent
				writeBlock(param.Exponent);

				var bytes = ms.ToArray();

				bytes = writeLen(index3, bytes);
				bytes = writeLen(index2, bytes);
				bytes = writeLen(index1, bytes);

				return "-----BEGIN PUBLIC KEY-----\n" + TextBreak(Convert.ToBase64String(bytes), 64) + "\n-----END PUBLIC KEY-----";
			} else {
				//Write total length
				ms.WriteByte(0x30);
				int index1 = (int)ms.Length;

				//Write version
				WriteAll(ms, _Ver);

				//PKCS8
				int index2 = -1, index3 = -1;
				if (isPKCS8) {
					WriteAll(ms, _SeqOID);
					ms.WriteByte(0x04);
					index2 = (int)ms.Length;
					ms.WriteByte(0x30);
					index3 = (int)ms.Length;
					WriteAll(ms, _Ver);
				}
				//Write data
				writeBlock(param.Modulus);
				writeBlock(param.Exponent);
				writeBlock(param.D);
				writeBlock(param.P);
				writeBlock(param.Q);
				writeBlock(param.DP);
				writeBlock(param.DQ);
				writeBlock(param.InverseQ);

				var bytes = ms.ToArray();

				if (index2 != -1) {
					bytes = writeLen(index3, bytes);
					bytes = writeLen(index2, bytes);
				}
				bytes = writeLen(index1, bytes);

				var flag = " PRIVATE KEY";
				if (!isPKCS8) { flag = " RSA" + flag; }
				return "-----BEGIN" + flag + "-----\n" + TextBreak(Convert.ToBase64String(bytes), 64) + "\n-----END" + flag + "-----";
			}
		}

		#endregion SaveXmlString SavePemString

		#region private WriteAll Sub TextBreak

		private static void WriteAll(Stream stream, byte[] byts)
		{
			stream.Write(byts, 0, byts.Length);
		}

		private static byte[] Sub(byte[] arr, int start, int count)
		{
			byte[] val = new byte[count];
			Array.Copy(arr, start, val, 0, count);
			return val;
		}

		private static string TextBreak(string text, int line)
		{
			var idx = 0;
			var len = text.Length;
			var str = new StringBuilder();
			while (idx < len) {
				if (idx > 0) {
					str.Append('\n');
				}
				if (idx + line >= len) {
					str.Append(text.Substring(idx));
				} else {
					str.Append(text.Substring(idx, line));
				}
				idx += line;
			}
			return str.ToString();
		}

		#endregion private WriteAll Sub TextBreak

		#endregion 密钥解析

		private class RsaEncryption : IDisposable
		{
			private BigInteger D;
			private BigInteger Exponent;
			private BigInteger Modulus;
			private RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();

			private bool isPrivateKeyLoaded = false;
			private bool isPublicKeyLoaded = false;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void LoadPublicFromXml(string publicString)
			{
				LoadPublicKey(rsa, publicString);

				RSAParameters rsaParams = rsa.ExportParameters(false);
				Modulus = FromBytes(rsaParams.Modulus);
				Exponent = FromBytes(rsaParams.Exponent);
				isPublicKeyLoaded = true;
				isPrivateKeyLoaded = false;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void LoadPrivateFromXml(string privateString)
			{
				LoadPrivateKey(rsa, privateString);

				RSAParameters rsaParams = rsa.ExportParameters(true);
				D = FromBytes(rsaParams.D);
				Exponent = FromBytes(rsaParams.Exponent);
				Modulus = FromBytes(rsaParams.Modulus);
				isPrivateKeyLoaded = true;
				isPublicKeyLoaded = true;
			}

			public byte[] PrivateEncryption(byte[] data)
			{
				if (!isPrivateKeyLoaded) throw new CryptographicException("Private Key must be loaded before using the Private Encryption method!");

				int len = (rsa.KeySize / 8) - 11;
				if (data.Length > len) {
					using (var ms = new MemoryStream()) {
						MemoryStream msInput = new MemoryStream(data);
						byte[] buffer = new byte[len];
						int readLen = msInput.Read(buffer, 0, len);

						while (readLen > 0) {
							byte[] dataToEnc = new byte[readLen];
							Array.Copy(buffer, 0, dataToEnc, 0, readLen);

							var bytes = PrivareEncryption2(dataToEnc);
							if (bytes.Length == rsa.KeySize / 8 + 1) {
								ms.Write(bytes, 0, bytes.Length);
							} else {
								ms.Write(bytes, 0, bytes.Length);
								var l = rsa.KeySize / 8 + 1 - bytes.Length;
								byte[] bs = new byte[l];
								ms.Write(bs, 0, l);
							}

							readLen = msInput.Read(buffer, 0, len);
						}
						msInput.Close();
						return ms.ToArray();
					}
				}
				return PrivareEncryption2(data);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private byte[] PrivareEncryption2(byte[] data)
			{
				return BigInteger.ModPow(FromBytes(data), D, Modulus).ToByteArray().Reverse().ToArray();
			}

			public byte[] PublicDecryption(byte[] data)
			{
				if (!isPublicKeyLoaded) throw new CryptographicException("Public Key must be loaded before using the Public Encryption method!");

				int len = (rsa.KeySize / 8) + 1;
				if (data.Length > len) {
					using (var ms = new MemoryStream()) {
						MemoryStream msInput = new MemoryStream(data);
						byte[] buffer = new byte[len];
						int readLen = msInput.Read(buffer, 0, len);

						while (readLen > 0) {
							while (buffer[readLen - 1] == 0) { readLen--; }
							byte[] dataToEnc = new byte[readLen];
							Array.Copy(buffer, 0, dataToEnc, 0, readLen);

							var bytes = PublicDecryption2(dataToEnc);
							ms.Write(bytes, 0, bytes.Length);

							readLen = msInput.Read(buffer, 0, len);
						}
						msInput.Close();
						return ms.ToArray();
					}
				}
				return PublicDecryption2(data);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public byte[] PublicDecryption2(byte[] data)
			{
				return BigInteger.ModPow(FromBytes(data), Exponent, Modulus).ToByteArray().Reverse().ToArray();
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private static BigInteger FromBytes(byte[] beBytes)
			{
				return new BigInteger(beBytes.Reverse().Concat(new byte[] { 0 }).ToArray());
			}

			public void Dispose()
			{
				rsa.Clear();
			}
		}
	}
}