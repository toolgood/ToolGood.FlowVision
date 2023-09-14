using ToolGood.ReadyGo3.Attributes;

namespace ToolGood.FlowVision.Datas.Projects
{
	[Table("flow_project_work_file")]
	public class DbProjectWorkFile
	{
		public int Id { get; set; }

		public int MainMemberId { get; set; }

		public string ProjectCode { get; set; }

		[LongText]
		public string Json { get; set; }

		public DateTime CreateTime { get; set; }
	}
}