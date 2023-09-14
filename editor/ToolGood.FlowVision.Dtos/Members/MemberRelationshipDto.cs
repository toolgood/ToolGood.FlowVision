using System.Text.Json.Serialization;

namespace ToolGood.FlowVision.Dtos.Members
{
	/// <summary>
	/// 权限关联表
	/// </summary>
	public class MemberRelationshipDto : Commons.IButtonPass
	{
		public int MemberId { get; set; }

		//public string AdminName { get; set; }

		public int GroupId { get; set; }

		public string GroupName { get; set; }

		public int MenuId { get; set; }

		public string MenuCode { get; set; }

		//public string MenuName { get; set; }

		public int ButtonId { get; set; }

		public string ButtonCode { get; set; }

		//public string ButtonName { get; set; }

		//public int GroupCount { get; set; }
		//public int Count { get; set; }

		//public int Count2 { get; set; }

		public List<MemberRelationshipDto> Items { get; set; }

		#region IButtonPass

		[JsonIgnore]
		private Dictionary<string, bool> buttonPass = new Dictionary<string, bool>();

		bool Commons.IButtonPass.CanEdit => HasButtonCode("edit");
		bool Commons.IButtonPass.CanDelete => HasButtonCode("delete");
		bool Commons.IButtonPass.CanEditOrDelete => HasButtonCode("edit") || HasButtonCode("delete");

		private bool HasButtonCode(string buttonCode)
		{
			bool result;
			if (buttonPass.TryGetValue(buttonCode, out result)) {
				return result;
			}
			result = Items.Any(q => q.ButtonCode.Equals(buttonCode, StringComparison.OrdinalIgnoreCase));
			return result;
		}

		bool Commons.IButtonPass.HasButtonCode(string buttonCode)
		{
			return HasButtonCode(buttonCode);
		}

		#endregion IButtonPass
	}
}