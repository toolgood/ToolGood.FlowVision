package toolgood.flowVision;

import static org.junit.Assert.assertTrue;

import org.junit.Test;
import toolgood.flowVision.Engines.FlowEngine;
import toolgood.flowVision.Engines.IFlowEngine;
import toolgood.flowVision.Flows.ProjectWork;

import java.io.BufferedReader;
import java.io.File;
import java.io.FileReader;

/**
 * Unit test for simple App.
 */
public class AppTest {
    /**
     * Rigorous Test :-)
     */
    @Test
    public void shouldAnswerWithTrue() {
        assertTrue(true);
    }

    @Test
    public void TestJsonFile() throws Exception {
        String json = getFileDataStr("dict/项目_20221207133837.json");
        ProjectWork project = ProjectWork.LoadJson(json);
        IFlowEngine flowEngine = new FlowEngine(project);
        flowEngine.BuildTreeNode("Flow", "Project1", "{\"数量\":800}");
        flowEngine.EvaluateInputNum();
        var t = flowEngine.TryEvaluate("总价", 0);



    }

    private String getFileDataStr(String filePath) {
        File file = new File(filePath);
        if (!file.exists()) {
            return null;
        }
        try {
            FileReader fileReader = new FileReader(file);
            BufferedReader reader = new BufferedReader(fileReader);
            StringBuilder stringBuilder = new StringBuilder();
            String line;
            while ((line = reader.readLine()) != null) {
                stringBuilder.append(line).append('\n');
            }
            reader.close();
            return stringBuilder.toString();
        } catch (Exception e) {
            e.printStackTrace();
            return null;
        }
    }

}
