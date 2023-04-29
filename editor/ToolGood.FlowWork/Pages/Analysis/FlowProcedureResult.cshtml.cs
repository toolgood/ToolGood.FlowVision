using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ToolGood.FlowWork.Pages.Analysis
{
	[IgnoreAntiforgeryToken]
	public class FlowProcedureResultModel : PageModel
	{
		public string AppCode { get; private set; }
		public string FactoryCode { get; private set; }
		public string Json { get; private set; }
		public string AttachData { get; private set; }
		public string MachineInfos { get; private set; }
		public string Formulas { get; private set; }
		public string Previous { get; private set; }

		public void OnPost(string appCode, string factoryCode, string json, string previous, string attachData, string machineInfos, string formulas)
		{
			AppCode = appCode;
			FactoryCode = factoryCode;
			Json = json ?? "";
			Formulas = formulas?.Trim() ?? "";
			AttachData = attachData ?? "";
			MachineInfos = machineInfos ?? "";
			Previous = previous ?? "";
		}

		public void OnGet(string appCode, string factoryCode, string json, string previous, string attachData, string machineInfos, string formulas)
		{
			AppCode = appCode;
			FactoryCode = factoryCode;
			Json = json ?? "";
			Formulas = formulas?.Trim() ?? "";
			AttachData = attachData ?? "";
			MachineInfos = machineInfos ?? "";
			Previous = previous ?? "";
		}
	}
}