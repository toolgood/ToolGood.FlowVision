using ToolGood.FlowVision.Datas.Projects;
using ToolGood.FlowVision.Dtos.Projects.Dtos;

namespace ToolGood.FlowVision.Dtos.Flow
{
	public class ProjectInfo
	{
		public DbProject Project { get; set; }
		public List<DbFactory> FactoryList { get; set; }
		public List<FactoryMachineDto> MachineList { get; set; }
		public List<FactoryProcedureDto> ProcedureList { get; set; }
		public List<DbProjectFormula> FormulaList { get; set; }

		public List<ProjectAppInfo> AppList { get; set; }
	}
}