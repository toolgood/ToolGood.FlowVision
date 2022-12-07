
using System.Diagnostics;
using ToolGood.FlowVision.Engines;
using ToolGood.FlowVision.Flows;

namespace ToolGood.FlowVision.Test
{
	internal class Program
	{
		static void Main(string[] args)
		{
			// 使用的是快速上手 案例 生成导出的文件

			var text = File.ReadAllText("dict/项目_20221207133837.json");
			var project = ProjectWork.LoadJson(text);
			FlowEngine flowEngine = new FlowEngine(project);
			flowEngine.BuildTreeNode("Flow", "Project1", "{\"数量\":800}");
			flowEngine.EvaluateInputNum();
			var t = flowEngine.TryEvaluate("总价", 0);
			Debug.Assert(6400 == t);


			var fileBytes = File.ReadAllBytes("dict/项目_20221207133841.data");
			var project2 = ProjectWork.LoadJsonWithRsa(fileBytes);
			FlowEngine flowEngine2 = new FlowEngine(project2);
			flowEngine2.BuildTreeNode("Flow", "Project1", "{\"数量\":80}");
			flowEngine2.EvaluateInputNum();
			var t2 = flowEngine2.TryEvaluate("总价", 0);
			Debug.Assert(800 == t2);

		}
	}
}