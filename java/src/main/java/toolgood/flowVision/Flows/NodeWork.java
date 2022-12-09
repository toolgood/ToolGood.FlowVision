package toolgood.flowVision.Flows;

import com.alibaba.fastjson.JSONObject;
import toolgood.flowVision.Engines.FlowEngine;
import toolgood.flowVision.Flows.Enums.CellType;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public abstract class NodeWork {
    protected ProjectWork Project;
    public CellType NodeType;
    public String Id;
    public String Label;
    public int Layer;

    public Map<String, List<NodeWork>> NextNodes;
    public Map<String, List<String>> NextNodeIds;

    public boolean Check(FlowEngine engine, String factoryCode) throws Exception {
        return true;
    }

    public List<String> GetChannels() {
        return NextNodes.keySet().stream().toList();
    }

    public void Init(ProjectWork work, AppWork app) {
        Project = work;
        if (NextNodeIds == null) {
            return;
        }
        NextNodes = new HashMap<>();
        for (String key : NextNodeIds.keySet()) {
            List<String> ids = NextNodeIds.get(key);
            ArrayList<NodeWork> list = new ArrayList<NodeWork>();
            for (int i = 0; i < ids.size(); i++) {
                String id = ids.get(i);
                if (app.AllNodeWork.containsKey(id)) {
                    NodeWork nodeWork = app.AllNodeWork.get(id);
                    list.add(nodeWork);
                }
            }
            if (list.size() > 0) {
                NextNodes.put(key, list);
            }
        }
    }

    final static NodeWork parse(JSONObject jsonObject) throws Exception {
        CellType type = CellType.intToEnum(jsonObject.getIntValue("nodeType"));
        if (type == CellType.Start)
            return StartFlowWork.parse2(jsonObject);
        if (type == CellType.End)
            return EndFlowWork.parse2(jsonObject);
        if (type == CellType.Error)
            return ErrorFlowWork.parse2(jsonObject);
        if (type == CellType.Procedure)
            return ProcedureFlowWork.parse2(jsonObject);
        if (type == CellType.Custom)
            return CustomFlowWork.parse2(jsonObject);
        if (type == CellType.Jump)
            return JumpFlowWork.parse2(jsonObject);
        if (type == CellType.Merge)
            return MergeFlowWork.parse2(jsonObject);
        if (type == CellType.Status)
            return StatusFlowWork.parse2(jsonObject);
        throw new Exception("NodeType类型出错了");
    }
}
