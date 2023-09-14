namespace ToolGood.FlowVision.Commons.Controllers
{
	public class MemberSessionDto
	{
		public MemberSessionDto()
		{ }

		public MemberSessionDto(int id, int mainMemberId, string name, string trueName, string projectIds)
		{
			Id = id;
			MainMemberId = mainMemberId;
			Name = name;
			TrueName = trueName;
			ProjectIds = projectIds;
		}

		public int MainMemberId { get; set; }
		public string ProjectIds { get; set; }
		public int Id { get; set; }

		/// <summary>
		/// 账号
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 真名
		/// </summary>
		public string TrueName { get; set; }

		public bool AllowProject(int id)
		{
			if (Id == MainMemberId) {
				return true;
			}
			if (string.IsNullOrEmpty(ProjectIds)) {
				return false;
			}
			return ProjectIds.Split(',').Contains(id.ToString());
		}
	}
}