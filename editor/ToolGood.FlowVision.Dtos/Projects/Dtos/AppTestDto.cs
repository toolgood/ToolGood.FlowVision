using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ToolGood.FlowVision.Dtos.Projects.Dtos
{
	public enum AppTestJsonType
	{
		Number,
		String,
		Bool,
		Date,
		List,
	}

	public class AppTestJsonDto
	{
		public string Name { get; set; }
		public bool IsRequired { get; set; }
		public AppTestJsonType Type { get; set; }
		public HashSet<string> TextList { get; set; } = new HashSet<string>();

		public void BuildNumber()
		{
			var DecimalList = new List<decimal>();
			DecimalList.Add(0);
			foreach (var item in TextList) {
				if (decimal.TryParse(item, out decimal dec)) {
					DecimalList.Add(dec);
				}
			}

			DecimalList = DecimalList.OrderBy(q => q).Distinct().ToList();
			var temp = new List<decimal>();
			for (int i = 0; i < DecimalList.Count; i++) {
				temp.Add(DecimalList[i]);
				if (i + 1 < DecimalList.Count) {
					temp.Add((DecimalList[i] + DecimalList[i + 1]) / 2);
				}
			}
			temp.Add(int.MinValue);
			temp.Add(int.MaxValue);
			temp = temp.OrderBy(q => q).Distinct().ToList();

			DecimalList = temp;

			TextList.Clear();
			foreach (var item in DecimalList) {
				TextList.Add(item.ToString());
			}
		}

 
 

	}





}
