using System.Text.RegularExpressions;

namespace ToolGood.FlowWork.Applications.Engines.Parameters
{
	public sealed class FormulaMatch
	{
		public string Name { get; set; }
		public string Text { get; set; }
		public List<FormulaMatchItem> Match { get; set; }

		public bool IsMatch(string txt)
		{
			if (string.IsNullOrWhiteSpace(Text) == false) {
				if (Text.Trim() == txt) { return true; }
			}
			if (Match != null) {
				for (int i = 0; i < Match.Count; i++) {
					var item = Match[i];
					if (item.Type.Equals("StartsWith", StringComparison.OrdinalIgnoreCase) || item.Type.Equals("start", StringComparison.OrdinalIgnoreCase)) {
						if (txt.StartsWith(item.Text.Trim())) { return true; }
					} else if (item.Type.Equals("EndsWith", StringComparison.OrdinalIgnoreCase) || item.Type.Equals("end", StringComparison.OrdinalIgnoreCase)) {
						if (txt.EndsWith(item.Text.Trim())) { return true; }
					} else if (item.Type.Equals("Contains", StringComparison.OrdinalIgnoreCase) || item.Type.Equals("in", StringComparison.OrdinalIgnoreCase)) {
						if (txt.Contains(item.Text.Trim())) { return true; }
					} else if (item.Type.Equals("match", StringComparison.OrdinalIgnoreCase) || item.Type.Equals("regex", StringComparison.OrdinalIgnoreCase) || item.Type.Equals("reg", StringComparison.OrdinalIgnoreCase)) {
						if (item.RegexMath == null) { item.RegexMath = new Regex(item.Text); }
						if (item.RegexMath.IsMatch(txt)) { return true; }
					} else {
						if (Text.Trim() == txt) { return true; }
					}
				}
			}
			return false;
		}
	}

	public sealed class FormulaMatchItem
	{
		public Regex RegexMath;
		public string Text { get; set; }
		public string Type { get; set; }
	}
}