package toolgood.flowVision;

import static org.junit.Assert.assertTrue;

import org.junit.Test;
import toolgood.flowVision.Engines.FlowEngine;
import toolgood.flowVision.Engines.IFlowEngine;
import toolgood.flowVision.Flows.ProjectWork;

/**
 * Unit test for simple App.
 */
public class AppTest {

    @Test
    public void TestJsonFile() throws Exception {
        ProjectWork project = ProjectWork.LoadJson("dict/项目_20221207133837.json");
        IFlowEngine flowEngine = new FlowEngine(project);
        flowEngine.BuildTreeNode("Flow", "Project1", "{\"数量\":80}");
        flowEngine.EvaluateInputNum();
        var t = flowEngine.TryEvaluate("总价", 0);

        project = ProjectWork.LoadJsonUsedRsa("dict/项目_20221207133841.data");
        IFlowEngine flowEngine2 = new FlowEngine(project);
        flowEngine2.BuildTreeNode("Flow", "Project1", "{\"数量\":80}");
        flowEngine2.EvaluateInputNum();
        var t2 = flowEngine2.TryEvaluate("总价", 0);
    }


}
