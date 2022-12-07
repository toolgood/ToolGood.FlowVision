package toolgood.flowVision.Flows;

import toolgood.flowVision.Flows.Enums.CellType;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class AppWork {
    public ProjectWork Project;// { get; set; }
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
        for(NodeWork item : AllNodeWork.values()) {
            item.Init(work, this);
            if (item.NodeType== CellType.Start){
                Start=(StartFlowWork)item;
            }
        }

        ParentNodeWorks = new HashMap<>();
        for(NodeWork item : AllNodeWork.values()) {
            for(List<NodeWork> nextNode : item.NextNodes.values()) {
                for (int i = 0; i < nextNode.size(); i++) {
                    NodeWork node = nextNode.get(i);
                    if (node.NodeType==CellType.Procedure){
                        List<NodeWork> list;
                        if (ParentNodeWorks.containsKey(node.Id)){
                            list=ParentNodeWorks.get(node.Id);
                            list.add(item);
                        }else{
                            list = new ArrayList<>();
                            ParentNodeWorks.put(node.Id,list);
                        }
                    }
                }
            }
        }


    }
}
