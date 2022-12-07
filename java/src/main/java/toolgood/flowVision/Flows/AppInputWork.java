package toolgood.flowVision.Flows;

import toolgood.algorithm.Operand;
import toolgood.algorithm.math.mathParser;
import toolgood.flowVision.Engines.FlowEngine;
import toolgood.flowVision.Flows.Enums.InputType;

public class AppInputWork {
    public ProjectWork Project;// { get; set; }

    public String InputName;// { get; set; } // 输入名
    public String Unit;// { get; set; }
    public toolgood.flowVision.Flows.Enums.InputType InputType ;//{ get; set; }// 输入类型
    public int InputTypeNum;// { get { return (int)InputType; } }
    public String CheckInput ;//{ get; set; } //检查类型
    public boolean IsRequired ;//{ get; set; } // 非空
    public boolean UseDefaultValue;// { get; set; } // 默认值
    public String DefaultValue;// { get; set; } // 默认值
    public String ErrorMessage ;//{ get; set; } // 抛出错误信息
    public String Comment ;//{ get; set; }

    public boolean Check(FlowEngine engine) throws Exception {
        if (CheckInput==null || CheckInput.equals("")) { return true; }
        mathParser.ProgContext progContext = Project.CreateProgContext(CheckInput);
        Operand operand = engine.EvaluateFormula(progContext, (int)InputType.Bool);
        return operand.BooleanValue();
    }

    public void Init(ProjectWork work)
    {
        Project = work;
    }

}
