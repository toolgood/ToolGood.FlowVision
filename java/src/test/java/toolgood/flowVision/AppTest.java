package toolgood.flowVision;

import static org.junit.Assert.assertTrue;

import org.junit.Test;
import toolgood.flowVision.Engines.FlowEngine;
import toolgood.flowVision.Engines.IFlowEngine;
import toolgood.flowVision.Flows.ProjectWork;

import javax.script.Bindings;
import javax.script.ScriptContext;
import javax.script.ScriptEngine;
import javax.script.ScriptEngineManager;
import java.util.HashMap;
import java.util.Map;
import java.util.function.BiConsumer;
import java.util.function.Consumer;
import java.util.function.Function;
import java.util.function.Supplier;

/**
 * Unit test for simple App.
 */
public class AppTest {

    @Test
    public void TestJsonFile() throws Exception {
        ProjectWork project = ProjectWork.LoadJson("dict/第一个项目_20221211121519.json");
        IFlowEngine flowEngine = new FlowEngine(project);
        flowEngine.BuildTreeNode("Flow", "Project1", "{\"数量\":80}");
        flowEngine.EvaluateInputNum();
        var t = flowEngine.TryEvaluate("总价", 0);

        project = ProjectWork.LoadJsonUsedRsa("dict/第一个项目_20221211121522.data");
        IFlowEngine flowEngine2 = new FlowEngine(project);
        flowEngine2.BuildTreeNode("Flow", "Project1", "{\"数量\":800}");
        flowEngine2.EvaluateInputNum();
        var t2 = flowEngine2.TryEvaluate("总价", 0);
    }

    @Test
    public void JsTest() throws Exception {
        map.put("aaa","123");
        EvaluateJs("""
                var tt= getValue("aaa");
                setValue("tt",ttt);
                var b= hasKey("aaa");
                setValue("b",b);
                """);

    }

    public Map<String, String> map = new HashMap<>();
    public String js_error = null;
    public String AttachData = null;

    private void EvaluateJs(String script) throws Exception {
        ScriptEngineManager manager = new ScriptEngineManager();
        ScriptEngine jsEngine = manager.getEngineByName("nashorn"); // 不知道为什么总是null

        Bindings bindings = jsEngine.getBindings(ScriptContext.ENGINE_SCOPE);
        bindings.put("getDatas", (Supplier<String>) this::js_getDatas);
        bindings.put("getValue", (Function<String, Object>) this::js_getValue);
        bindings.put("hasKey", (Function<String, Boolean>) this::js_hasKey);
        bindings.put("setValue", (BiConsumer<String, Object>) this::js_setValue);
        bindings.put("Error", (Consumer<String>) this::js_Error);
        jsEngine.eval(script);
        if (js_error != null) throw new Exception(js_error);
    }

    private void js_Error(String msg) {
        if (js_error != null) return;
        js_error = msg;
    }

    private String js_getDatas() {
        return AttachData;
    }

    private void js_setValue(String name, Object value) {
        map.put(name,value.toString());
    }

    private Object js_getValue(String name) {
        return map.get(name);
    }

    private Boolean js_hasKey(String name) {
        if (map.containsKey(name)) {
            return true;
        }
        return false;
    }


}
