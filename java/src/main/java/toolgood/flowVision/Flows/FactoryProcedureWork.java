package toolgood.flowVision.Flows;

import com.alibaba.fastjson2.JSONObject;
import toolgood.algorithm.math.mathParser;
import toolgood.flowVision.Engines.FlowEngine;

import java.util.HashMap;
import java.util.Map;

public class FactoryProcedureWork {
    public String Category;
    public String Code;
    public String Name;
    public String CheckFormula;
    public Map<String, FactoryProcedureItemWork> Items;
    private ProjectWork Project;// { get; set; }

    final static FactoryProcedureWork parse(JSONObject jsonObject) {
        FactoryProcedureWork result = new FactoryProcedureWork();
        result.Category = jsonObject.getString("category");
        result.Code = jsonObject.getString("code");
        result.Name = jsonObject.getString("name");
        result.CheckFormula = jsonObject.getString("checkFormula");

        result.Items = new HashMap<>();
        JSONObject allNodeWork = jsonObject.getJSONObject("items");
        for (Map.Entry<String, Object> item : allNodeWork.entrySet()) {
            if (item.getValue() instanceof JSONObject jsonObject1) {
                FactoryProcedureItemWork work = FactoryProcedureItemWork.parse(jsonObject1);
                result.Items.put(item.getKey(), work);
            }
        }

        return result;
    }

    public void Init(ProjectWork work) {
        Project = work;
    }

    public boolean Check(FlowEngine engine) throws Exception {
        if (CheckFormula == null || CheckFormula.equals("")) {
            return true;
        }
        mathParser.ProgContext progContext = Project.CreateProgContext(CheckFormula);
        return engine.TryEvaluate(progContext, false);
    }
}
