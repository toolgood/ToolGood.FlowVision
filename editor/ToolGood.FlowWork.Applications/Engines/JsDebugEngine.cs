using Jint;
using ToolGood.Algorithm2;
using ToolGood.Algorithm2.Enums;
using ToolGood.JsonObject;

namespace ToolGood.FlowWork.Applications.Engines
{
	public sealed class JsDebugEngine
	{
		private string AttachData;
		private Dictionary<string, Operand> _tempdict = new Dictionary<string, Operand>();//保存 输入项
		private Dictionary<string, Operand> _inputDict = new Dictionary<string, Operand>();//保存 输入项

		public JsDebugEngine()
		{
		}

		public void SetInput(string json)
		{
			JObject j = JObject.Parse(json);
			foreach (var item in j.Properties()) {
				if (item.Value.Type == JTokenType.String) {
					_inputDict[item.Name] = Operand.Create(item.Value.Value<string>());
				} else if (item.Value.Type == JTokenType.Boolean) {
					_inputDict[item.Name] = Operand.Create(item.Value.Value<bool>());
				} else if (item.Value.Type == JTokenType.Integer) {
					_inputDict[item.Name] = Operand.Create(item.Value.Value<int>());
				} else if (item.Value.Type == JTokenType.Float) {
					_inputDict[item.Name] = Operand.Create(item.Value.Value<double>());
				} else if (item.Value.Type == JTokenType.Date) {
					_inputDict[item.Name] = Operand.Create(item.Value.Value<DateTime>());
				} else if (item.Value.Type == JTokenType.Undefined) {
					_inputDict[item.Name] = Operand.CreateNull();
				} else if (item.Value.Type == JTokenType.Null) {
					_inputDict[item.Name] = Operand.CreateNull();
				}
			}
		}

		public void SetAttachData(string attachData)
		{
			attachData = attachData.Trim();
			if (attachData.StartsWith('{') && attachData.EndsWith('}')) {
				try {
					attachData = JObject.Parse(attachData).ToString();
				} catch (Exception) { }
			} else if (attachData.StartsWith('[') && attachData.EndsWith(']')) {
				try {
					attachData = JArray.Parse(attachData).ToString();
				} catch (Exception) { }
			}
			AttachData = attachData;
		}

		public void EvaluateJs(string script)
		{
			var engine = new Engine()
				.SetValue("getDatas", new Func<object>(js_getDatas))
				.SetValue("hasKey", new Func<string, bool>(js_hasKey))
				.SetValue("setValue", new Action<string, object>(js_setValue))
				.SetValue("getValue", new Func<string, object>(js_getValue))
				.SetValue("error", new Action<string>(js_Error));
			engine.Evaluate(script);
			engine.Dispose();
			engine = null;
		}

		public Dictionary<string, string> GetTempData()
		{
			Dictionary<string, string> result = new Dictionary<string, string>();

			foreach (var item in _tempdict) {
				if (_inputDict.ContainsKey(item.Key)) { continue; }
				var value = item.Value;
				if (value.Type == OperandType.BOOLEAN) {
					result[item.Key] = value.BooleanValue.ToString();
				} else if (value.Type == OperandType.TEXT) {
					result[item.Key] = value.TextValue.ToString();
				} else if (value.Type == OperandType.DATE) {
					result[item.Key] = value.DateValue.ToString();
				} else if (value.Type == OperandType.NUMBER) {
					result[item.Key] = value.NumberValue.ToString();
				} else if (value.Type == OperandType.NULL) {
				}
			}
			return result;
		}

		private Operand GetTempParameter(string parameter)
		{
			if (_inputDict.TryGetValue(parameter, out Operand operand)) { // 输入值
				return operand;
			}
			if (_tempdict.TryGetValue(parameter, out Operand operand2)) { // 临时变量
				return operand2;
			}
			return Operand.Error($"{parameter}的公式未找到！");
		}

		private void js_Error(string msg)
		{
			throw new Exception(msg);
		}

		private string js_getDatas()
		{
			return AttachData;
		}

		private void js_setValue(string name, object value)
		{
			if (value == null) {
				_tempdict.Add(name, Operand.CreateNull());
			} else if (value is int numInt) {
				_tempdict.Add(name, Operand.Create(numInt));
			} else if (value is double numDouble) {
				_tempdict.Add(name, Operand.Create(numDouble));
			} else if (value is bool numBool) {
				_tempdict.Add(name, Operand.Create(numBool));
			} else if (value is string str) {
				_tempdict.Add(name, Operand.Create(str));
			} else if (value is DateTime date) {
				_tempdict.Add(name, Operand.Create(date));
			} else {
				throw new Exception("setValue is error!");
			}
		}

		private object js_getValue(string name)
		{
			var value = GetTempParameter(name);
			if (value.Type == OperandType.BOOLEAN) {
				return value.BooleanValue;
			} else if (value.Type == OperandType.TEXT) {
				return value.TextValue;
			} else if (value.Type == OperandType.DATE) {
				return (DateTime)value.DateValue;
			} else if (value.Type == OperandType.NUMBER) {
				return value.NumberValue;
			} else if (value.Type == OperandType.NULL) {
				return null;
			} else if (value.Type == OperandType.ARRARY) {
				var list = new object[value.ArrayValue.Count];
				for (int i = 0; i < value.ArrayValue.Count; i++) {
					var op = value.ArrayValue[i];
					if (op.Type == OperandType.BOOLEAN) {
						list[i] = value.BooleanValue;
					} else if (op.Type == OperandType.TEXT) {
						list[i] = value.TextValue;
					} else if (op.Type == OperandType.DATE) {
						list[i] = (DateTime)value.DateValue;
					} else if (op.Type == OperandType.NUMBER) {
						list[i] = value.NumberValue;
					} else if (op.Type == OperandType.NULL) {
						list[i] = null;
					} else {
						list[i] = value.TextValue;
					}
				}
				return list;
			}
			throw new Exception("getValue is error!");
		}

		private bool js_hasKey(string name)
		{
			if (_inputDict.ContainsKey(name)) {
				return true;
			} else if (_tempdict.ContainsKey(name)) {
				return true;
			}
			return false;
		}
	}
}