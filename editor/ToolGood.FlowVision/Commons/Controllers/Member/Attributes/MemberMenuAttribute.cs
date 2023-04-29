namespace ToolGood.FlowVision.Commons.Controllers
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
	public class MemberMenuAttribute : Attribute
	{
		public string MenuCode { get; private set; }
		public string ButtonCode { get; private set; }

		public MemberMenuAttribute(string menuCode, string buttonCode)
		{
			MenuCode = menuCode.Trim();
			ButtonCode = buttonCode.ToLower().Trim();
		}
	}
}