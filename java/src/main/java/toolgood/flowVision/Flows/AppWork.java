package toolgood.flowVision.Flows;

import com.alibaba.fastjson2.JSONArray;
import com.alibaba.fastjson2.JSONObject;
import toolgood.flowVision.Flows.Enums.CellType;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class AppWork {
    private ProjectWork Project;
    public String Code;
    public String Name;
    public List<AppInitValueWork> InitValueList;
    public List<AppInputWork> InputList;
    public Map<String, NodeWork> AllNodeWork;
    public Map<String, List<NodeWork>> ParentNodeWorks;
    public StartFlowWork Start;

    public void Init(ProjectWork work) {
        Project = work;
        for (int i = 0; i < InitValueList.size(); i++) {
            AppInitValueWork item = InitValueList.get(i);
            item.Init(work);
        }
        for (int i = 0; i < InputList.size(); i++) {
            AppInputWork item = InputList.get(i);
            item.Init(work);
        }
        for (NodeWork item : AllNodeWork.values()) {
            item.Init(work, this);
            if (item.NodeType == CellType.Start) {
                Start = (StartFlowWork) item;
            }
        }

        ParentNodeWorks = new HashMap<>();
        for (NodeWork item : AllNodeWork.values()) {
            for (List<NodeWork> nextNode : item.NextNodes.values()) {
                for (int i = 0; i < nextNode.size(); i++) {
                    NodeWork node = nextNode.get(i);
                    if (node.NodeType == CellType.Procedure) {
                        List<NodeWork> list;
                        if (ParentNodeWorks.containsKey(node.Id)) {
                            list = ParentNodeWorks.get(node.Id);
                            list.add(item);
                        } else {
                            list = new ArrayList<>();
                            ParentNodeWorks.put(node.Id, list);
                        }
                    }
                }
            }
        }


    }

    final static AppWork parse(JSONObject jsonObject) throws Exception {
        AppWork result = new AppWork();
        result.Code = jsonObject.getString("code");
        result.Name = jsonObject.getString("name");

        result.InitValueList = new ArrayList<>();
        JSONArray initValueList = jsonObject.getJSONArray("initValueList");
        for (Object item : initValueList) {
            if (item instanceof JSONObject jsonObject1) {
                AppInitValueWork work = AppInitValueWork.parse(jsonObject1);
                result.InitValueList.add(work);
            }
        }

        result.InputList = new ArrayList<>();
        JSONArray inputList = jsonObject.getJSONArray("inputList");
        for (Object item : inputList) {
            if (item instanceof JSONObject jsonObject1) {
                AppInputWork work = AppInputWork.parse(jsonObject1);
                result.InputList.add(work);
            }
        }

        result.AllNodeWork = new HashMap<>();
        JSONObject allNodeWork = jsonObject.getJSONObject("allNodeWork");
        for (Map.Entry<String, Object> item : allNodeWork.entrySet()) {
            if (item.getValue() instanceof JSONObject jsonObject1) {
                NodeWork work = NodeWork.parse(jsonObject1);
                result.AllNodeWork.put(item.getKey(), work);
            }
        }

        return result;
    }
}
