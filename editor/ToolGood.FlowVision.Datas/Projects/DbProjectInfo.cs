namespace ToolGood.FlowVision.Datas.Projects
{
	[ReadyGo3.Attributes.Table("flow_project_file")]
	[ReadyGo3.Attributes.Index("MainMemberId", "ProjectId")]
	public class DbProjectFile
	{
		public int Id { get; set; }

		[System.Text.Json.Serialization.JsonIgnore]
		public int MainMemberId { get; set; }

		public int ProjectId { get; set; }

		public DateTime CreateTime { get; set; }

		[System.Text.Json.Serialization.JsonIgnore]
		public int CreateUserId { get; set; }
	}
}