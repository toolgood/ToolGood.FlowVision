namespace ToolGood.FlowVision.Dtos.Commons
{
	public abstract class PageRequestDto
	{
		public string Field { get; set; }
		public string Order { get; set; }
		public int PageIndex { get; set; }
		public int PageSize { get; set; }
	}
}