using Microsoft.AspNetCore.Razor.TagHelpers;
using ToolGood.FlowVision.Dtos.Commons;

namespace ToolGood.FlowVision.Commons.TagHelpers
{
	[HtmlTargetElement("*", Attributes = "pass-edit")]
	public class EditPassTagHelper : TagHelper
	{
		[HtmlAttributeName("pass-edit")]
		public IButtonPass Pass { get; set; }

		public override void Process(TagHelperContext context, TagHelperOutput output)
		{
			if (!Pass.CanEdit) {
				output.SuppressOutput();
			}
		}
	}
}