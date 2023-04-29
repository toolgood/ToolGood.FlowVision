using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.RegularExpressions;

namespace ToolGood.FlowVision.Pages.FlowProject.Projects.Toolkits
{
	[IgnoreAntiforgeryToken]
	public class InitValueToolModel : PageModel
	{
		public List<Tuple<string, string>> Table = new List<Tuple<string, string>>();

		public void OnGet()
		{
		}

		public void OnPost(string txt)
		{
			if (string.IsNullOrEmpty(txt)) { return; }

			List<Tuple<string, string>> temps = new List<Tuple<string, string>>();
			var ts = txt.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

			foreach (var t in ts) {
				GetItems(t, temps);
			}
			Table = temps;
		}

		public void GetItems(string txt, List<Tuple<string, string>> temps)
		{
			var reg = new Regex(@"^[0-9a-z]+$", RegexOptions.IgnoreCase);

			var ms = Regex.Match(txt.Trim(), @"^\{(.*?)\}\.has\((.*?)\)", RegexOptions.IgnoreCase);
			var t = ms.Groups[1].Value;
			var name = ms.Groups[2].Value.Trim();

			var sp = t.Split(',', StringSplitOptions.RemoveEmptyEntries);
			foreach (var s in sp) {
				var temp = s.Replace("\"", "").Replace("'", "").Replace("¡°", "").Replace("¡±", "").Replace("¡®", "").Replace("¡¯", "").Trim();
				if (temp == "·ñ") { continue; }
				if (temp == "ÊÇ") {
					temps.Add(Tuple.Create($"is{name}", $"{name}='{temp}'"));
				} else if (temp == "ÎÞ") {
					temps.Add(Tuple.Create($"isÎÞ{name}", $"{name}='{temp}'"));
				} else if (reg.IsMatch(temp)) {
					temps.Add(Tuple.Create($"is{name}{temp}", $"{name}='{temp}'"));
				} else {
					temps.Add(Tuple.Create($"is{temp}", $"{name}='{temp}'"));
				}
			}
		}
	}
}