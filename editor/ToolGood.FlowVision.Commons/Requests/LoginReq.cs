using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using ToolGood.FlowVision.Commons.Utils;
using ToolGood.RcxCrypto;

namespace ToolGood.FlowVision.Commons
{
	public class LoginReq<T> : IRequest
	{
		/// <summary>
		/// 时间截
		/// </summary>
		public long Timestamp { get; set; }

		/// <summary>
		/// 签名
		/// </summary>
		public string Sign { get; set; }

		/// <summary>
		/// 密文
		/// </summary>
		[System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string Ciphertext { get; set; }

		/// <summary>
		/// 钥匙（RSA公匙加密过的）
		/// </summary>
		public string RsaKey { get; set; }

		/// <summary>
		/// 通信密码
		/// </summary>
		[JsonIgnore]
		public string PasswordString { get; set; }

		/// <summary>
		/// 解密后的参数
		/// </summary>
		[JsonIgnore]
		public T Data { get; set; }

		[JsonIgnore]
		public string Ip { get; set; }

		[JsonIgnore]
		public string UserAgent { get; set; }

		[JsonIgnore]
		public string Fingerprint { get; set; }

		[JsonIgnore]
		public int MainMemberId { get; set; }

		[JsonIgnore]
		public int OperatorId { get; set; }

		[JsonIgnore]
		public string Message { get; set; }

		[JsonIgnore]
		public string OperatorName { get; set; }

		[JsonIgnore]
		public string OperatorTrueName { get; set; }

		/// <summary>
		/// 核对签名
		/// </summary>
		/// <returns></returns>
		public bool CheckSign(string privateKey, string modulus, string exponent, out string errMsg)
		{
			if (Timestamp <= 0) { errMsg = "timestamp is null."; return false; }
			var st = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(Timestamp);
			if (Math.Abs(st.TotalSeconds) >= 5) { errMsg = "timestamp is error."; return false; }

			if (string.IsNullOrWhiteSpace(RsaKey)) { errMsg = "rsaKey is null."; return false; }
			if (string.IsNullOrWhiteSpace(Ciphertext)) { errMsg = "ciphertext is null."; return false; }
			if (string.IsNullOrWhiteSpace(Sign)) { errMsg = "sign is null."; return false; }

			PasswordString = RsaUtil.PrivateDecrypt(privateKey, RsaKey);

			var txt = $"{Ciphertext.ToSafeString()}|{PasswordString}|{Timestamp.ToSafeString()}|{modulus}|{exponent}";
			var hash = HashUtil.GetMd5String(txt);
			if (Sign.Equals(hash, StringComparison.OrdinalIgnoreCase) == false) {
				errMsg = "sign is error.";
				return false;
			}
			errMsg = null;
			return true;
		}

		/// <summary>
		/// 核对签名
		/// </summary>
		/// <returns></returns>
		public bool CheckSign(string modulus, string exponent, out string errMsg)
		{
			// 如果 timestamp 为0  标签没有加 [FromBody, FromForm]
			if (Timestamp <= 0) { errMsg = "timestamp is null."; return false; }
			var st = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(Timestamp);
			if (Math.Abs(st.TotalSeconds) >= 15) { errMsg = "timestamp is error."; return false; }

			if (string.IsNullOrWhiteSpace(Ciphertext)) { errMsg = "ciphertext is null."; return false; }
			if (string.IsNullOrWhiteSpace(Sign)) { errMsg = "sign is null."; return false; }

			var txt = $"{Ciphertext.ToSafeString()}|{PasswordString}|{Timestamp.ToSafeString()}|{modulus}|{exponent}";
			var hash = HashUtil.GetMd5String(txt);
			if (Sign.Equals(hash, StringComparison.OrdinalIgnoreCase) == false) {
				errMsg = "sign is error.";
				return false;
			}
			errMsg = null;
			return true;
		}

		/// <summary>
		/// 解密
		/// </summary>
		/// <param name="password"></param>
		/// <returns></returns>
		public bool DecryptData()
		{
			string json = null;
			//JObject JData;
			try {
				var bs = RCX.Encrypt(Base64Util.FromBase64ForUrlString(Ciphertext), PasswordString);//解密
				json = Encoding.UTF8.GetString(bs);

				Data = JsonUtil.DeserializeObject<T>(json);
				//JData = JObject.Parse(json, new JsonLoadSettings() {
				//    CommentHandling = CommentHandling.Ignore,
				//    LineInfoHandling = LineInfoHandling.Ignore,
				//    DuplicatePropertyNameHandling = DuplicatePropertyNameHandling.Replace
				//});
				//Data = JData.ToObject<T>();
				return true;
			} catch (Exception) {
				//LogUtil.Error(ex, json);
			}
			return false;
		}

		/// <summary>
		/// 核对数据
		/// </summary>
		/// <param name="errMsg"></param>
		/// <returns></returns>
		public bool CheckData(out string errMsg)
		{
			try {
				return CheckData(typeof(T), Data, null, out errMsg);
			} catch (Exception ex) {
				StringBuilder errorBuilder = new StringBuilder();
				errorBuilder.AppendFormat("Message：{0}", ex.Message).AppendLine();
				if (ex.InnerException != null) {
					if (!string.Equals(ex.Message, ex.InnerException.Message, StringComparison.OrdinalIgnoreCase)) {
						errorBuilder.AppendFormat("InnerException：{0}", ex.InnerException.Message).AppendLine();
					}
				}
				errorBuilder.AppendFormat("Source：{0}", ex.Source).AppendLine();
				errorBuilder.AppendFormat("StackTrace：{0}", ex.StackTrace).AppendLine();
				errMsg = errorBuilder.ToString();
				return false;
			}
		}

		private bool CheckData(Type type, object value, string baseName, out string errMsg)
		{
			if (type.IsClass == false) { errMsg = null; return true; }
			if (SimpleTypes.Contains(type)) { errMsg = null; return true; }

			var pis = type.GetProperties();
			foreach (var pi in pis) {
				if (pi.CanRead == false) { continue; }
				object obj = pi.GetGetMethod().Invoke(value, null);
				if (obj is DateTime && (DateTime)obj == DateTime.MinValue) {
					errMsg = $"{GetPropertyName(pi)}为时间类型！";
					return false;
				}

				var atts = pi.GetCustomAttributes<System.ComponentModel.DataAnnotations.ValidationAttribute>(true).ToList();
				if (atts.Count > 0) {
					foreach (var att in atts) {
						if (att.IsValid(obj) == false) {
							errMsg = att.FormatErrorMessage(GetPropertyName(pi));
							return false;
						}
					}
				}

				if (pi.PropertyType.IsClass && obj != null && SimpleTypes.Contains(pi.PropertyType) == false) {
					if (obj is IList list) {
						for (int i = 0; i < list.Count; i++) {
							var item = list[i];
							if (object.Equals(null, item) == false) {
								var itemType = item.GetType();
								if (itemType.IsClass == false) { continue; }
								if (SimpleTypes.Contains(itemType)) { continue; }

								if (CheckData(itemType, item, GetPropertyName(pi), out errMsg) == false) {
									return false;
								}
							}
						}
					} else {
						if (CheckData(pi.PropertyType, obj, GetPropertyName(pi), out errMsg) == false) {
							return false;
						}
					}
				}
			}
			errMsg = null;
			return true;
		}

		private static string GetPropertyName(PropertyInfo pi)
		{
			var att = pi.GetCustomAttributes(typeof(DisplayNameAttribute), true);
			if (att.Length > 0) {
				return (att[0] as DisplayNameAttribute).DisplayName;
			}
			//att = pi.GetCustomAttributes(typeof(DescriptionAttribute), true);
			//if (att.Length > 0) {
			//    return (att[0] as DescriptionAttribute).Description;
			//}
			return pi.Name;
		}

		private static readonly HashSet<Type> SimpleTypes = new HashSet<Type>() {
			typeof(string)
			,typeof(byte),typeof(sbyte),typeof(char),typeof(Boolean),typeof(Guid)
			,typeof(UInt16),typeof(UInt32),typeof(UInt64),typeof(Int16),typeof(Int32),typeof(Int64)
			,typeof(Single),typeof(Double),typeof(Decimal)
			,typeof(DateTime),typeof(DateTimeOffset),typeof(TimeSpan)
			,typeof(IntPtr),typeof(UIntPtr)
			,typeof(byte?),typeof(sbyte?),typeof(char?), typeof(Boolean?),typeof(Guid?)
			,typeof(UInt16?),typeof(UInt32?),typeof(UInt64?),typeof(Int16?),typeof(Int32?),typeof(Int64?)
			,typeof(Single?),typeof(Double?),typeof(Decimal?)
			,typeof(DateTime?),typeof(DateTimeOffset?),typeof(TimeSpan?)
			,typeof(IntPtr?),typeof(UIntPtr?)
		};
	}
}