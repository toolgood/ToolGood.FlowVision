/*!
 *  版权所有(C) 2021 ToolGood(林知君)
 *  GPLv3 License - http://www.gnu.org/licenses/gpl-3.0.html
 */

using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace ToolGood.Attributes
{
	public class VaCountListAttribute : ValidationAttribute
	{
		public VaCountListAttribute(int min, int max)
		{
			CanBeNull = true;
			MinCount = min;
			MaxCount = max;
			ErrorMessage = "{0}集合个数要大于{1}并且小于{2}！";
		}

		protected VaCountListAttribute(int min, int max, bool canBeNull)
		{
			CanBeNull = canBeNull;
			MinCount = min;
			MaxCount = max;
			ErrorMessage = "{0}集合个数要大于{1}并且小于{2}！";
		}

		public bool CanBeNull { get; private set; }
		public int MinCount { get; private set; }
		public int MaxCount { get; private set; }

		public override string FormatErrorMessage(string name)
		{
			return string.Format(ErrorMessage, name, MinCount, MaxCount);
		}

		public override bool IsValid(object value)
		{
			if (object.Equals(null, value)) {
				return CanBeNull;
			}

			if (value is IList) {
				var v = value as IList;
				if (v.Count >= MinCount && v.Count <= MaxCount) {
					return true;
				}
			}

			if (value is IEnumerable) {
				var v = (value as IEnumerable);
				var length = 0;
				foreach (var item in v) {
					length++;
				}
				if (length >= MinCount && length <= MaxCount) {
					return true;
				}
			}
			return false;
		}
	}
}