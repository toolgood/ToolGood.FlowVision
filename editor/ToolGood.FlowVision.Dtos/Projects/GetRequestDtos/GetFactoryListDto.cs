﻿using ToolGood.FlowVision.Dtos.Commons;

namespace ToolGood.FlowVision.Dtos
{
	public class GetFactoryListDto : PageRequestDto
	{
		public int ProjectId { get; set; }

		public string Category { get; set; }
		public string Name { get; set; }
	}
}