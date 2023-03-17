using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ToolGood.Algorithm2.Enums;
using ToolGood.Algorithm2.LitJson;

namespace ToolGood.Algorithm2
{
	/// <summary>
	/// 操作数
	/// </summary>
	public abstract class Operand : IDisposable
	{
		/// <summary>
		/// FALSE
		/// </summary>
		public static readonly Operand False = new OperandBoolean(false);

		/// <summary>
		/// 1
		/// </summary>
		public static readonly Operand One = new OperandNumber(1);

		/// <summary>
		/// TRUE
		/// </summary>
		public static readonly Operand True = new OperandBoolean(true);

		/// <summary>
		/// 0
		/// </summary>
		public static readonly Operand Zero = new OperandNumber(0);

		internal static readonly CultureInfo cultureInfo = CultureInfo.GetCultureInfo("en-US");

		/// <summary>
		/// 数组值
		/// </summary>
		public virtual List<Operand> ArrayValue => throw new NotImplementedException();

		/// <summary>
		/// 布尔值
		/// </summary>
		public virtual bool BooleanValue => throw new NotImplementedException();

		/// <summary>
		/// 时间值
		/// </summary>
		public virtual MyDate DateValue => throw new NotImplementedException();

		/// <summary>
		/// 错误信息
		/// </summary>
		public virtual string ErrorMsg => null;

		/// <summary>
		/// int值
		/// </summary>
		public virtual int IntValue => throw new NotImplementedException();

		/// <summary>
		/// 是否出错
		/// </summary>
		public virtual bool IsError => false;

		/// <summary>
		/// 是否为空
		/// </summary>
		public virtual bool IsNull => false;

		/// <summary>
		/// 数字值
		/// </summary>
		public virtual double NumberValue => throw new NotImplementedException();

		/// <summary>
		/// 字符串值
		/// </summary>
		public virtual string TextValue => throw new NotImplementedException();

		/// <summary>
		/// 操作数类型
		/// </summary>
		public abstract OperandType Type { get; }

		internal virtual JsonData JsonValue => throw new NotImplementedException();

		#region Create

		/// <summary>
		/// 创建操作数
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static Operand Create(bool obj)
		{
			return obj ? True : False;
		}

		#region number

		/// <summary>
		/// 创建操作数
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static Operand Create(short obj)
		{
			return new OperandNumber(obj);
		}

		/// <summary>
		/// 创建操作数
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static Operand Create(int obj)
		{
			if (obj == 1) {
				return One;
			} else if (obj == 0) {
				return Zero;
			}
			return new OperandNumber(obj);
		}

		/// <summary>
		/// 创建操作数
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static Operand Create(long obj)
		{
			if (obj == 1) {
				return One;
			} else if (obj == 0) {
				return Zero;
			}
			return new OperandNumber((double)obj);
		}

		/// <summary>
		/// 创建操作数
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static Operand Create(ushort obj)
		{
			if (obj == 1) {
				return One;
			} else if (obj == 0) {
				return Zero;
			}
			return new OperandNumber((double)obj);
		}

		/// <summary>
		/// 创建操作数
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static Operand Create(uint obj)
		{
			if (obj == 1) {
				return One;
			} else if (obj == 0) {
				return Zero;
			}
			return new OperandNumber((double)obj);
		}

		/// <summary>
		/// 创建操作数
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static Operand Create(ulong obj)
		{
			if (obj == 1) {
				return One;
			} else if (obj == 0) {
				return Zero;
			}
			return new OperandNumber((double)obj);
		}

		/// <summary>
		/// 创建操作数
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static Operand Create(float obj)
		{
			return new OperandNumber((double)obj);
		}

		/// <summary>
		/// 创建操作数
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static Operand Create(double obj)
		{
			return new OperandNumber(obj);
		}

		/// <summary>
		/// 创建操作数
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static Operand Create(decimal obj)
		{
			return new OperandNumber((double)obj);
		}

		#endregion number

		/// <summary>
		/// 创建操作数
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static Operand Create(string obj)
		{
			if (object.Equals(null, obj)) {
				return Operand.CreateNull();
			}
			return new OperandString(obj);
		}

		/// <summary>
		/// 创建操作数
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static Operand Create(MyDate obj)
		{
			return new OperandMyDate(obj);
		}

		/// <summary>
		/// 创建操作数
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static Operand Create(DateTime obj)
		{
			return new OperandMyDate(new MyDate(obj));
		}

		/// <summary>
		/// 创建操作数
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static Operand Create(TimeSpan obj)
		{
			return new OperandMyDate(new MyDate(obj));
		}

		/// <summary>
		/// 创建操作数
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static Operand Create(List<Operand> obj)
		{
			return new OperandArray(obj);
		}

		/// <summary>
		/// 创建操作数
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static Operand Create(ICollection<string> obj)
		{
			var array = new List<Operand>();
			foreach (var item in obj) {
				array.Add(Create(item));
			}
			return new OperandArray(array);
		}

		/// <summary>
		/// 创建操作数
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static Operand Create(ICollection<double> obj)
		{
			var array = new List<Operand>();
			foreach (var item in obj) {
				array.Add(Create(item));
			}
			return new OperandArray(array);
		}

		/// <summary>
		/// 创建操作数
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static Operand Create(ICollection<int> obj)
		{
			var array = new List<Operand>();
			foreach (var item in obj) {
				array.Add(Create(item));
			}
			return new OperandArray(array);
		}

		/// <summary>
		/// 创建操作数
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static Operand Create(ICollection<bool> obj)
		{
			var array = new List<Operand>();
			foreach (var item in obj) {
				array.Add(Create(item));
			}
			return new OperandArray(array);
		}

		/// <summary>
		/// 创建操作数
		/// </summary>
		/// <param name="txt"></param>
		/// <returns></returns>
		public static Operand CreateJson(string txt)
		{
			txt = txt.Trim();
			if ((txt.StartsWith('{') && txt.EndsWith('}')) || (txt.StartsWith('[') && txt.EndsWith(']'))) {
				try {
					var json = JsonMapper.ToObject(txt);
					return Operand.Create(json);
				} catch (Exception) { }
			}
			return Operand.Error("string to json is error!");
		}

		/// <summary>
		/// 创建操作数
		/// </summary>
		/// <returns></returns>
		public static Operand CreateNull()
		{
			return new OperandNull();
		}

		/// <summary>
		/// 创建操作数
		/// </summary>
		/// <param name="msg"></param>
		/// <returns></returns>
		public static Operand Error(string msg)
		{
			return new OperandError(msg);
		}

		/// <summary>
		/// 创建操作数
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		internal static Operand Create(JsonData obj)
		{
			return new OperandJson(obj);
		}

		#endregion Create

		void IDisposable.Dispose()
		{
		}

		/// <summary>
		/// 转Array类型
		/// </summary>
		/// <param name="errorMessage"></param>
		/// <returns></returns>
		public virtual Operand ToArray(string errorMessage = null)
		{ return Error(errorMessage); }

		/// <summary>
		/// 转bool类型
		/// </summary>
		/// <param name="errorMessage"></param>
		/// <returns></returns>
		public virtual Operand ToBoolean(string errorMessage = null)
		{ return Error(errorMessage); }

		/// <summary>
		/// 转Json类型
		/// </summary>
		/// <param name="errorMessage"></param>
		/// <returns></returns>
		public virtual Operand ToJson(string errorMessage = null)
		{ return Error(errorMessage); }

		/// <summary>
		/// 转MyDate类型
		/// </summary>
		/// <param name="errorMessage"></param>
		/// <returns></returns>
		public virtual Operand ToMyDate(string errorMessage = null)
		{ return Error(errorMessage); }

		/// <summary>
		/// 转数值类型
		/// </summary>
		/// <param name="errorMessage"></param>
		/// <returns></returns>
		public virtual Operand ToNumber(string errorMessage = null)
		{ return Error(errorMessage); }

		/// <summary>
		/// 转String类型
		/// </summary>
		/// <param name="errorMessage"></param>
		/// <returns></returns>
		public virtual Operand ToText(string errorMessage = null)
		{ return Error(errorMessage); }

		#region Operand

		#region number

		/// <summary>
		/// 转 Operand
		/// </summary>
		/// <param name="obj"></param>
		public static implicit operator Operand(Int16 obj)
		{
			return Operand.Create((int)obj);
		}

		/// <summary>
		/// 转 Operand
		/// </summary>
		/// <param name="obj"></param>
		public static implicit operator Operand(Int32 obj)
		{
			return Operand.Create(obj);
		}

		/// <summary>
		/// 转 Operand
		/// </summary>
		/// <param name="obj"></param>
		public static implicit operator Operand(Int64 obj)
		{
			return Operand.Create((double)obj);
		}

		/// <summary>
		/// 转 Operand
		/// </summary>
		/// <param name="obj"></param>
		public static implicit operator Operand(UInt16 obj)
		{
			return Operand.Create((double)obj);
		}

		/// <summary>
		/// 转 Operand
		/// </summary>
		/// <param name="obj"></param>
		public static implicit operator Operand(UInt32 obj)
		{
			return Operand.Create((double)obj);
		}

		/// <summary>
		/// 转 Operand
		/// </summary>
		/// <param name="obj"></param>
		public static implicit operator Operand(UInt64 obj)
		{
			return Operand.Create((double)obj);
		}

		/// <summary>
		/// 转 Operand
		/// </summary>
		/// <param name="obj"></param>
		public static implicit operator Operand(float obj)
		{
			return Operand.Create((double)obj);
		}

		/// <summary>
		/// 转 Operand
		/// </summary>
		/// <param name="obj"></param>
		public static implicit operator Operand(double obj)
		{
			return Operand.Create(obj);
		}

		/// <summary>
		/// 转 Operand
		/// </summary>
		/// <param name="obj"></param>
		public static implicit operator Operand(decimal obj)
		{
			return Operand.Create((double)obj);
		}

		#endregion number

		/// <summary>
		/// 转 Operand
		/// </summary>
		/// <param name="obj"></param>
		public static implicit operator Operand(bool obj)
		{
			return Operand.Create(obj);
		}

		/// <summary>
		/// 转 Operand
		/// </summary>
		/// <param name="obj"></param>
		public static implicit operator Operand(string obj)
		{
			return Operand.Create(obj);
		}

		/// <summary>
		/// 转 Operand
		/// </summary>
		/// <param name="obj"></param>
		public static implicit operator Operand(DateTime obj)
		{
			return Operand.Create(obj);
		}

		/// <summary>
		/// 转 Operand
		/// </summary>
		/// <param name="obj"></param>
		public static implicit operator Operand(TimeSpan obj)
		{
			return Operand.Create(obj);
		}

		/// <summary>
		/// 转 Operand
		/// </summary>
		/// <param name="obj"></param>
		public static implicit operator Operand(List<string> obj)
		{
			return Operand.Create(obj);
		}

		/// <summary>
		/// 转 Operand
		/// </summary>
		/// <param name="obj"></param>
		public static implicit operator Operand(List<bool> obj)
		{
			return Operand.Create(obj);
		}

		/// <summary>
		/// 转 Operand
		/// </summary>
		/// <param name="obj"></param>
		public static implicit operator Operand(List<int> obj)
		{
			return Operand.Create(obj);
		}

		/// <summary>
		/// 转 Operand
		/// </summary>
		/// <param name="obj"></param>
		public static implicit operator Operand(List<double> obj)
		{
			return Operand.Create(obj);
		}

		#endregion Operand
	}

	internal class KeyValue
	{
		public int? Index;
		public string Key;
		public Operand Value;
	}

	internal abstract class Operand<T> : Operand
	{
		public Operand(T obj)
		{
			Value = obj;
		}

		public T Value { get; private set; }
	}

	internal sealed class OperandArray : Operand<List<Operand>>
	{
		public OperandArray(List<Operand> obj) : base(obj)
		{
		}

		public override List<Operand> ArrayValue => Value;
		public override OperandType Type => OperandType.ARRARY;

		public override Operand ToArray(string errorMessage = null)
		{ return this; }
	}

	internal sealed class OperandBoolean : Operand<bool>
	{
		public OperandBoolean(bool obj) : base(obj)
		{
		}

		public override bool BooleanValue => Value;
		public override OperandType Type => OperandType.BOOLEAN;

		public override Operand ToArray(string errorMessage = null)
		{
			return Error(errorMessage ?? "Convert bool to array error!");
		}

		public override Operand ToBoolean(string errorMessage = null)
		{ return this; }

		public override Operand ToJson(string errorMessage = null)
		{
			return Error(errorMessage ?? "Convert bool to json error!");
		}

		public override Operand ToMyDate(string errorMessage = null)
		{
			return Error(errorMessage ?? "Convert bool to date error!");
		}

		public override Operand ToNumber(string errorMessage = null)
		{ return BooleanValue ? One : Zero; }

		public override Operand ToText(string errorMessage = null)
		{ return Create(BooleanValue ? "TRUE" : "FALSE"); }
	}

	internal sealed class OperandError : Operand
	{
		private readonly string _errorMsg;

		public OperandError(string msg)
		{
			_errorMsg = msg;
		}

		public override string ErrorMsg => _errorMsg;
		public override bool IsError => true;
		public override OperandType Type => OperandType.ERROR;

		public override Operand ToArray(string errorMessage = null)
		{ return this; }

		public override Operand ToBoolean(string errorMessage = null)
		{ return this; }

		public override Operand ToJson(string errorMessage = null)
		{ return this; }

		public override Operand ToMyDate(string errorMessage = null)
		{ return this; }

		public override Operand ToNumber(string errorMessage = null)
		{ return this; }

		public override Operand ToText(string errorMessage = null)
		{ return this; }
	}

	internal sealed class OperandJson : Operand<JsonData>
	{
		public OperandJson(JsonData obj) : base(obj)
		{
		}

		public override OperandType Type => OperandType.JSON;
		internal override JsonData JsonValue => Value;

		public override Operand ToArray(string errorMessage = null)
		{
			if (JsonValue.IsArray) {
				List<Operand> list = new List<Operand>();
				foreach (JsonData v in JsonValue) {
					if (v.IsString)
						list.Add(Operand.Create(v.StringValue));
					else if (v.IsBoolean)
						list.Add(Operand.Create(v.BooleanValue));
					else if (v.IsDouble)
						list.Add(Operand.Create(v.NumberValue));
					else if (v.IsNull)
						list.Add(Operand.CreateNull());
					else
						list.Add(Operand.Create(v));
				}
				return Create(list);
			}
			return Error(errorMessage ?? "Convert json to array error!");
		}

		public override Operand ToBoolean(string errorMessage = null)
		{
			return Error(errorMessage ?? "Convert json to bool error!");
		}

		public override Operand ToJson(string errorMessage = null)
		{ return this; }

		public override Operand ToMyDate(string errorMessage = null)
		{
			return Error(errorMessage ?? "Convert json to date error!");
		}

		public override Operand ToNumber(string errorMessage = null)
		{
			return Error(errorMessage ?? "Convert json to number error!");
		}

		public override Operand ToText(string errorMessage = null)
		{
			return Error(errorMessage ?? "Convert json to string error!");
		}
	}

	internal sealed class OperandKeyValue : Operand<KeyValue>
	{
		public OperandKeyValue(KeyValue obj) : base(obj)
		{
		}

		public override OperandType Type => OperandType.ARRARYJSON;
	}

	internal sealed class OperandKeyValueList : Operand<KeyValue>
	{
		private int listNum;

		private List<KeyValue> TextList = new List<KeyValue>();

		public OperandKeyValueList(KeyValue obj, int excelIndex) : base(obj)
		{
			listNum = excelIndex;
		}

		public override List<Operand> ArrayValue => TextList.Select(q => q.Value).ToList();
		public override OperandType Type => OperandType.ARRARYJSON;

		public void AddValue(KeyValue keyValue)
		{
			if (keyValue.Index != null) {
				listNum = keyValue.Index.Value;
				keyValue.Key = keyValue.Index.Value.ToString();
			} else {
				keyValue.Index = listNum;
			}
			TextList.Add(keyValue);
			listNum++;
		}

		public bool ContainsValue(Operand value)
		{
			foreach (var item in TextList) {
				var op = item.Value;
				if (value.Type != op.Type) { continue; }
				if (value.Type == OperandType.TEXT) {
					if (value.TextValue == op.TextValue) {
						return true;
					}
				}
				if (value.Type == OperandType.NUMBER) {
					if (value.TextValue == op.TextValue) {
						return true;
					}
				}
			}
			return false;
		}

		public override Operand ToArray(string errorMessage = null)
		{
			return Create(this.ArrayValue);
		}

		public bool TryGetValue(int key, out Operand value)
		{
			foreach (var item in TextList) {
				if (item.Index == key) {
					value = item.Value;
					return true;
				}
			}
			foreach (var item in TextList) {
				if (item.Key == key.ToString()) {
					value = item.Value;
					return true;
				}
			}
			value = null;
			return false;
		}

		public bool TryGetValue(string key, out Operand value)
		{
			foreach (var item in TextList) {
				if (item.Key == key.ToString()) {
					value = item.Value;
					return true;
				}
			}
			value = null;
			return false;
		}

		public bool TryGetValueFloor(double key, bool range_lookup, out Operand value)
		{
			value = null;
			foreach (var item in TextList) {
				if (double.TryParse(item.Key, out double num) == false) continue;
				var t = Math.Round(key - num, 10, MidpointRounding.AwayFromZero);
				if (t == 0) {
					value = item.Value;
					return true;
				} else if (range_lookup) {
					if (t > 0) {
						value = item.Value;
					} else if (value != null) {
						return true;
					}
				}
			}
			return value != null;
		}
	}

	internal sealed class OperandMyDate : Operand<MyDate>
	{
		public OperandMyDate(MyDate obj) : base(obj)
		{
		}

		public override MyDate DateValue => Value;
		public override OperandType Type => OperandType.DATE;

		public override Operand ToArray(string errorMessage = null)
		{
			return Error(errorMessage ?? "Convert date to array error!");
		}

		public override Operand ToBoolean(string errorMessage = null)
		{
			return ((double)DateValue) != 0 ? True : False;
		}

		public override Operand ToJson(string errorMessage = null)
		{
			return Error(errorMessage ?? "Convert date to json error!");
		}

		public override Operand ToMyDate(string errorMessage = null)
		{ return this; }

		public override Operand ToNumber(string errorMessage = null)
		{
			return Create((double)DateValue);
		}

		public override Operand ToText(string errorMessage = null)
		{
			return Create(DateValue.ToString());
		}
	}

	internal sealed class OperandNull : Operand
	{
		public override bool IsNull => true;
		public override OperandType Type => OperandType.NULL;

		public override Operand ToArray(string errorMessage = null)
		{
			return Error(errorMessage ?? "Convert null to array error!");
		}

		public override Operand ToBoolean(string errorMessage = null)
		{
			return Error(errorMessage ?? "Convert null to bool error!");
		}

		public override Operand ToJson(string errorMessage = null)
		{
			return Error(errorMessage ?? "Convert null to json error!");
		}

		public override Operand ToMyDate(string errorMessage = null)
		{
			return Error(errorMessage ?? "Convert null to date error!");
		}

		public override Operand ToNumber(string errorMessage = null)
		{
			return Error(errorMessage ?? "Convert null to number error!");
		}

		public override Operand ToText(string errorMessage = null)
		{
			return Error(errorMessage ?? "Convert null to string error!");
		}
	}

	internal sealed class OperandNumber : Operand<double>
	{
		public OperandNumber(double obj) : base(obj)
		{
		}

		public override int IntValue => (int)Value;
		public override double NumberValue => Value;
		public override OperandType Type => OperandType.NUMBER;

		public override Operand ToArray(string errorMessage = null)
		{
			return Error(errorMessage ?? "Convert number to array error!");
		}

		public override Operand ToBoolean(string errorMessage = null)
		{ return NumberValue != 0 ? True : False; }

		public override Operand ToJson(string errorMessage = null)
		{
			return Error(errorMessage ?? "Convert number to json error!");
		}

		public override Operand ToMyDate(string errorMessage = null)
		{ return Create((MyDate)NumberValue); }

		public override Operand ToNumber(string errorMessage = null)
		{ return this; }

		public override Operand ToText(string errorMessage = null)
		{ return Create(Math.Round(NumberValue, 10).ToString(cultureInfo)); }
	}

	internal sealed class OperandString : Operand<string>
	{
		public OperandString(string obj) : base(obj)
		{
		}

		public override string TextValue => Value;
		public override OperandType Type => OperandType.TEXT;

		public override Operand ToArray(string errorMessage = null)
		{
			return Error(errorMessage ?? "Convert string to array error!");
		}

		public override Operand ToBoolean(string errorMessage = null)
		{
			if (TextValue.Equals("true", StringComparison.OrdinalIgnoreCase)) { return True; }
			if (TextValue.Equals("yes", StringComparison.OrdinalIgnoreCase)) { return True; }
			if (TextValue.Equals("false", StringComparison.OrdinalIgnoreCase)) { return False; }
			if (TextValue.Equals("no", StringComparison.OrdinalIgnoreCase)) { return False; }
			if (TextValue.Equals("1") || TextValue.Equals("是") || TextValue.Equals("有") || TextValue.Equals("真")) { return True; }
			if (TextValue.Equals("0") || TextValue.Equals("否") || TextValue.Equals("不是") || TextValue.Equals("无") || TextValue.Equals("没有") || TextValue.Equals("假")) { return False; }
			if (errorMessage == null) {
				return Error("Convert string to bool error!");
			}
			return Error(errorMessage);
		}

		public override Operand ToJson(string errorMessage = null)
		{
			var txt = TextValue.Trim();
			if ((txt.StartsWith('{') && txt.EndsWith('}')) || (txt.StartsWith('[') && txt.EndsWith(']'))) {
				try {
					var json = JsonMapper.ToObject(txt);
					return Operand.Create(json);
				} catch (Exception) { }
			}
			if (errorMessage == null) {
				return Error("Convert string to json error!");
			}
			return Error(errorMessage);
		}

		public override Operand ToMyDate(string errorMessage = null)
		{
			if (TimeSpan.TryParse(TextValue, cultureInfo, out TimeSpan t)) { return Create(new MyDate(t)); }
			if (DateTime.TryParse(TextValue, cultureInfo, DateTimeStyles.None, out DateTime d)) { return Create(new MyDate(d)); }
			if (errorMessage == null) {
				return Error("Convert string to date error!");
			}
			return Error(errorMessage);
		}

		public override Operand ToNumber(string errorMessage = null)
		{
			if (double.TryParse(TextValue, out double d)) {
				return Operand.Create(d);
			}
			if (errorMessage == null) {
				return Error("Convert string to number error!");
			}
			return Error(errorMessage);
		}

		public override Operand ToText(string errorMessage = null)
		{
			return this;
		}
	}
}