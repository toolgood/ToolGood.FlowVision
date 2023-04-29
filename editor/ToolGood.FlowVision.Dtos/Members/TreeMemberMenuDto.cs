namespace ToolGood.FlowVision.Dtos.Members
{
	public class TreeMemberMenuDto
	{
		public int Id { get; set; }

		public string Url { get; set; }

		public string Name { get; set; }

		public bool IsOpen { get; set; }

		public bool Activity { get; set; }

		public List<TreeMemberMenuDto> Items { get; set; }

		public TreeMemberMenuDto()
		{
			Items = new List<TreeMemberMenuDto>();
		}
	}
}