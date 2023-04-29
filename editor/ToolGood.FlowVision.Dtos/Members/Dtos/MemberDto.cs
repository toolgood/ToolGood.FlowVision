using ToolGood.Datas;

namespace ToolGood.FlowVision.Dtos.Members
{
	public class MemberDto : DbSysMember
	{
		[ToolGood.ReadyGo3.Attributes.Ignore]
		public string GroupName { get; set; }

		[ToolGood.ReadyGo3.Attributes.Ignore]
		public string ProjectNames { get; set; }
	}
}