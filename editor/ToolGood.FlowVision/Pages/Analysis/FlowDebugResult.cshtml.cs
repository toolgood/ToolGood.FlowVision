using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ToolGood.FlowVision.Applications.Members;
using ToolGood.FlowVision.Commons;

namespace ToolGood.FlowWork.Pages.Analysis
{
	[IgnoreAntiforgeryToken]
	public class FlowDebugResultModel : PageModel
	{
		public string AppCode { get; private set; }
		public string FactoryCode { get; private set; }
		public string Json { get; private set; }
		public string AttachData { get; private set; }
		public string MachineInfos { get; private set; }
		public string Formulas { get; private set; }
		public string Previous { get; private set; }
		public string ProjectCode { get; private set; }
		private IMemberApplication _memberApplication;

		public FlowDebugResultModel(IMemberApplication memberApplication)
		{
			_memberApplication = memberApplication;
		}

		public async Task<IActionResult> OnPost(string appCode, string factoryCode, string json, string previous, string attachData, string machineInfos, string formulas, string projectCode)
		{
			AppCode = appCode;
			FactoryCode = factoryCode;
			Json = json ?? "";
			Formulas = formulas?.Trim() ?? "";
			AttachData = attachData ?? "";
			MachineInfos = machineInfos ?? "";
			Previous = previous ?? "";
			ProjectCode = projectCode ?? "";
			var dt = await _memberApplication.GetRealTime();
			return Page();
		}

		public async Task<IActionResult> OnGet(string appCode, string factoryCode, string json, string previous, string attachData, string machineInfos, string formulas, string projectCode)
		{
			AppCode = appCode;
			FactoryCode = factoryCode;
			Json = json ?? "";
			Formulas = formulas?.Trim() ?? "";
			AttachData = attachData ?? "";
			MachineInfos = machineInfos ?? "";
			Previous = previous ?? "";
			ProjectCode = projectCode ?? "";
			var dt = await _memberApplication.GetRealTime();
			return Page();
		}
	}
}