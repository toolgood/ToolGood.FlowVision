package toolgood.flowVision.Flows;

import org.antlr.v4.runtime.CharStreams;
import org.antlr.v4.runtime.CommonTokenStream;
import toolgood.algorithm.internals.AntlrCharStream;
import toolgood.algorithm.internals.AntlrErrorListener;
import toolgood.algorithm.math.mathLexer;
import toolgood.algorithm.math.mathParser;

import java.util.Map;

public class ProjectWork {
    public String Name;
    public String Code;

    public int ExcelIndex;
    public boolean NumberRequired;

    public Map<String, String> FormulaList;
    public Map<String, FactoryWork> FactoryList;
    public Map<String, FactoryMachineWork> FactoryMachineList;
    public Map<String, FactoryProcedureWork> FactoryProcedureList;
    public Map<String, AppWork> AppList;
    private Map<String, mathParser.ProgContext> ProgList;

    public mathParser.ProgContext CreateProgContext(String exp) throws Exception {
        if (ProgList.containsKey(exp)) {
            return ProgList.get(exp);
        }
        if (exp == null || exp.equals("")) {
            return null;
        }
        AntlrCharStream stream = new AntlrCharStream(CharStreams.fromString(exp));
        mathLexer lexer = new mathLexer(stream);
        CommonTokenStream tokens = new CommonTokenStream(lexer);
        mathParser parser = new mathParser(tokens);
        AntlrErrorListener antlrErrorListener = new AntlrErrorListener();
        parser.removeErrorListeners();
        parser.addErrorListener(antlrErrorListener);

        mathParser.ProgContext context = parser.prog();
        if (antlrErrorListener.IsError) {
            throw new Exception(antlrErrorListener.ErrorMsg);
        }
        ProgList.put(exp, context);
        return context;
    }

    public boolean TryGetFormula(String name, mathParser.ProgContext context) throws Exception {
        if (FormulaList.containsKey(name)) {
            context = CreateProgContext(FormulaList.get(name));
            return true;
        }
        context = null;
        return false;
    }

    public boolean HasFormula(String name) {
        if (FormulaList.containsKey(name)) {
            return true;
        }
        return false;
    }


}
