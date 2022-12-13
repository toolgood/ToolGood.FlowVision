package toolgood.flowVision.Engines.OutDatas;

import toolgood.algorithm.Operand;
import toolgood.flowVision.Engines.FlowEngine;
import toolgood.flowVision.Flows.Enums.CellType;
import toolgood.flowVision.Flows.Enums.InputNumberType;
import toolgood.flowVision.Flows.FactoryMachineWork;
import toolgood.flowVision.Flows.FactoryProcedureItemWork;
import toolgood.flowVision.Flows.Interfaces.IInputFormulaNodeWork;
import toolgood.flowVision.Flows.NodeWork;
import toolgood.flowVision.Flows.ProcedureFlowWork;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;


public class TreeNode {
    public List<ChannelNode> PrevNodes;
    public Map<String, TreeNode> NextNodes;
    public NodeWork CurrWork;
    public FactoryMachineWork MachineItem;
    public FactoryProcedureItemWork ProcedureItem;
    public Double InputNum;
    public Double OutputNum;

    public TreeNode(NodeWork work, FactoryMachineWork factoryMachineItem, FactoryProcedureItemWork factoryProcedureItem) {
        CurrWork = work;
        MachineItem = factoryMachineItem;
        ProcedureItem = factoryProcedureItem;
        PrevNodes = new ArrayList<>();
        NextNodes = new HashMap<>();
    }

    public String Id() {
        return CurrWork.Id;
    }

    public int Layer() {
        return CurrWork.Layer;
    }

    public boolean IsSubsidiaryCount() {
        if (CurrWork instanceof IInputFormulaNodeWork) {
            IInputFormulaNodeWork work = (IInputFormulaNodeWork) CurrWork;
            return work.IsSubsidiaryCount();
        }
        return false;
    }

    public void AddNextNode(TreeNode node, String channel, int index) {
        NextNodes.put(channel, node);
        ChannelNode nd = new ChannelNode();
        nd.Index = index;
        nd.Node = node;
        nd.Channel = channel;
        node.PrevNodes.add(nd);
    }

    public void SetError(String channel) {
        for (int i = 0; i < PrevNodes.size(); i++) {
            ChannelNode prevNode = PrevNodes.get(i);
            prevNode.Node.NextNodes.remove(channel);
        }
    }

    public void EvaluateInputNum(FlowEngine engine) throws Exception {
        if (InputNum != null) {
            return;
        }
        if (CurrWork.NodeType == CellType.End) {
            OutputNum = engine.GetNum().NumberValue();
            if (CurrWork instanceof ProcedureFlowWork) {
                ProcedureFlowWork procedure = (ProcedureFlowWork) CurrWork;
                OutputNum = GetNumber(procedure.NumberType, OutputNum.doubleValue());
            }
            InputNum = OutputNum;
            return;
        }
        if (OutputNum == null) {
            double num = 0;
            for (TreeNode nextNode : NextNodes.values()) {
                nextNode.EvaluateInputNum(engine);
                if (nextNode.IsSubsidiaryCount() == false) {
                    if (num < nextNode.InputNum) {
                        num = nextNode.InputNum;
                    }
                }
            }
            OutputNum = num;
        }
        engine.SetOutputNum(Operand.Create(OutputNum));
        if (InputNum == null) {
            if (CurrWork instanceof IInputFormulaNodeWork) {
                IInputFormulaNodeWork work = (IInputFormulaNodeWork) CurrWork;
                InputNum = work.EvaluateInputNum(engine);
            } else {
                InputNum = OutputNum;
            }
            if (CurrWork instanceof ProcedureFlowWork) {
                ProcedureFlowWork procedure = (ProcedureFlowWork) CurrWork;
                InputNum = GetNumber(procedure.NumberType, InputNum);
            }
        }
        engine.ClearOutputNum();
    }

    private double GetNumber(InputNumberType numberType, double n) {
        switch (numberType) {
            case Original:
                return n;
            case Ceil:
                return Math.ceil(n);
            case Floor:
                return Math.floor(n);
            case Round0:
                return Math.round(n);
            case Round1:
                return Math.round(n * 10) / 10d;
            case Round2:
                return Math.round(n * 100) / 100d;
            case Round3:
                return Math.round(n * 1000) / 1000d;
            case Round4:
                return Math.round(n * 10000) / 10000d;
            case Round5:
                return Math.round(n * 100000) / 100000d;
            case Round6:
                return Math.round(n * 1000000) / 1000000d;
            case Round7:
                return Math.round(n * 10000000) / 10000000d;
            case Round8:
                return Math.round(n * 100000000) / 100000000d;
            default:
                return n;
        }
    }

}
