package toolgood.flowVision.Engines;

import org.joda.time.DateTime;

import java.util.List;

public interface IFlowEngine {
    List<String> BuildTreeNode(String appCode, String factoryCode, String json ) throws Exception;
    List<String> BuildTreeNode(String appCode, String factoryCode, String json, String previous) throws Exception;
    void EvaluateInputNum() throws Exception;
    double TryEvaluate(String exp, double def);
    boolean TryEvaluate(String exp, boolean def);
    DateTime TryEvaluate(String exp, DateTime def);
    String TryEvaluate(String exp, String def);

}
