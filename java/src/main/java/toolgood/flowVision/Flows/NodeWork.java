package toolgood.flowVision.Flows;

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
    public String Comment;

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


}
