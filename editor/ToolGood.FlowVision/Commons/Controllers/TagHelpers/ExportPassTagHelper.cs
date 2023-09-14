using Microsoft.AspNetCore.Razor.TagHelpers;
using ToolGood.FlowVision.Dtos.Commons;

namespace ToolGood.FlowVision.Commons.TagHelpers
{
	[HtmlTargetElement("*", Attributes = "pass-export")]
	public class ExportPassTagHelper : TagHelper
	{
		[HtmlAttributeName("pass-export")]
		public IButtonPass Pass { get; set; }

		public override void Process(TagHelperContext context, TagHelperOutput output)
		{
			if (!Pass.HasButtonCode("export")) {
				output.SuppressOutput();
			}
		}
	}
}