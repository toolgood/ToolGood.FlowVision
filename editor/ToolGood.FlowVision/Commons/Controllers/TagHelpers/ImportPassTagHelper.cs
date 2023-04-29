using Microsoft.AspNetCore.Razor.TagHelpers;
using ToolGood.FlowVision.Dtos.Commons;

namespace ToolGood.FlowVision.Commons.TagHelpers
{
	[HtmlTargetElement("*", Attributes = "pass-import")]
	public class ImportPassTagHelper : TagHelper
	{
		[HtmlAttributeName("pass-import")]
		public IButtonPass Pass { get; set; }

		public override void Process(TagHelperContext context, TagHelperOutput output)
		{
			if (!Pass.HasButtonCode("import")) {
				output.SuppressOutput();
			}
		}
	}
}