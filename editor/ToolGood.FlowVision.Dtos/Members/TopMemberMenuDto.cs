using ToolGood.Datas;

namespace ToolGood.FlowVision.Dtos.Members
{
	public class TopMemberMenuDto
	{
		public int Id { get; set; }

		public string Ids { get; set; }

		public string Url { get; set; }

		public string Name { get; set; }

		public string Code { get; set; }

		public bool Activity { get; set; }

		public TopMemberMenuDto()
		{ }

		public TopMemberMenuDto(DbSysMemberMenu adminMenu, string code)
		{
			Id = adminMenu.Id;
			Ids = adminMenu.Ids;
			Url = adminMenu.Url;
			Name = adminMenu.MenuName;
			Code = adminMenu.MenuCode;
			Activity = adminMenu.MenuCode == code;
		}
	}
}