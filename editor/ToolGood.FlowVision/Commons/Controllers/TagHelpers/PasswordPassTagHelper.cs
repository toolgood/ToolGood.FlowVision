using Microsoft.AspNetCore.Razor.TagHelpers;
using ToolGood.FlowVision.Dtos.Commons;

namespace ToolGood.FlowVision.Commons.TagHelpers
{
	[HtmlTargetElement("*", Attributes = "pass-password")]
	public class PasswordPassTagHelper : TagHelper
	{
		[HtmlAttributeName("pass-password")]
		public IButtonPass Pass { get; set; }

		public override void Process(TagHelperContext context, TagHelperOutput output)
		{
			if (!Pass.CanDelete) {
				output.SuppressOutput();
			}
		}
	}
}